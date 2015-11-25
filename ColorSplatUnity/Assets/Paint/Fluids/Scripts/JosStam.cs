using UnityEngine;
using System.Collections;

public class JosStam : MonoBehaviour {

	public int N;
	public float dt = 0.8f;
	public float visc = 0.2f;
	public int iterations = 10;
	
	Texture2D tex;
	bool add = true;
	int size;
	int rowSize;
	float[] u, v;
	float[] u_prev, v_prev;
	float[] dens, dens_prev;
	
	// Use this for initialization
	void Start () {
		tex = Instantiate(GetComponent<Renderer>().material.mainTexture) as Texture2D;
		GetComponent<Renderer>().material.mainTexture = tex;

		size = (N+2)*(N+2);
		rowSize = N + 2;
		dens = new float[size];
		dens_prev = new float[size];
		u = new float[size];
		u_prev = new float[size];
		v = new float[size];
		v_prev = new float[size];
	}
	
	// Update is called once per frame
	void Update () {
		// reset values
		for (int i = 0; i < size; i++) {
			dens_prev[i] = 0;
			u_prev[i] = 0;
			v_prev[i] = 0;
		}
		UserInput();
		vel_step (u, v, u_prev, v_prev, dt); 
		dens_step (dens, dens_prev, u, v, dt); 
		Draw();
	}
	
	int IJK(int i, int j) {
		return (i + j*(N+2));
	}

	void SWAP(float[] x0, float[] x) {
		float[] tmp=x0;
		x0 = x;
		x = tmp;
	}

	void add_source (float[] x, float[] s, float dt) {
		for (int i=0 ; i<size ; i++ ) 
			x[i] += dt*s[i]; 
	}

	void set_bnd (int b, float[] x) {
		int k = 0;
		for (int i=1 ; i<=N ; i++ ) { 
			x[IJK(0,i)] = b==1 ? (-1f)*x[IJK(1,i)] : x[IJK(1,i)];
			x[IJK(N+1,i)] = b==1 ? (-1f)*x[IJK(N,i)] : x[IJK(N,i)]; 
			x[IJK(i,0)] = b==2 ? (-1f)*x[IJK(i,1)] : x[IJK(i,1)];
			x[IJK(i,N+1)] = b==2 ? (-1f)*x[IJK(i,N)] : x[IJK(i,N)]; 
		}
		x[IJK(0 ,0 )] = 0.5f*(x[IJK(1,0 )]+x[IJK(0 ,1)]);
		x[IJK(0 ,N+1)] = 0.5f*(x[IJK(1,N+1)]+x[IJK(0 ,N )]); 
		x[IJK(N+1,0 )] = 0.5f*(x[IJK(N,0 )]+x[IJK(N+1,1)]); 
		x[IJK(N+1,N+1)] = 0.5f*(x[IJK(N,N+1)]+x[IJK(N+1,N)]);
	}

	void diffuse (int b, float[] x, float[] x0, float dt) {
		float a=dt*visc*N*N;
		for (int it=0 ; it<iterations ; it++ ) {
			for (int i=1 ; i<=N ; i++ ) {
				for (int j=1 ; j<=N ; j++ ) {
					x[IJK(i,j)] = (x0[IJK(i,j)] + a*(x[IJK(i-1,j)]+x[IJK(i+1,j)]+x[IJK(i,j-1)]+x[IJK(i,j+1)]))/(1+4*a);
				}
			}
			set_bnd (b, x);
		}
	}
	
	void advect (int b, float[] d, float[] d0, float[] u, float[] v, float dt) {
		int i0, j0, i1, j1;
		float x, y, s0, t0, s1, t1, dt0;
		dt0 = dt*N;
		for (int i=1 ; i<=N ; i++ ) {
			for (int j=1 ; j<=N ; j++ ) {
				x = i-dt0*u[IJK(i,j)]; y = j-dt0*v[IJK(i,j)];
				if (x<0.5f) x=0.5f; 
				if (x>N+0.5f) x=N+0.5f; i0=(int)x; i1=i0+1; 
				if (y<0.5f) y=0.5f; 
				if (y>N+0.5f) y=N+0.5f; j0=(int)y; j1=j0+1; 
				s1 = x-i0; s0 = 1-s1; 
				t1 = y-j0; t0 = 1-t1;
				d[IJK(i,j)] = s0*(t0*d0[IJK(i0,j0)]+t1*d0[IJK(i0,j1)])+s1*(t0*d0[IJK(i1,j0)]+t1*d0[IJK(i1,j1)]);
			}
		}
		set_bnd (b, d); 
	}

	void project (float[] u, float[] v, float[] p, float[] div) {
		float h = 1.0f/N;
		for (int i=1 ; i<=N ; i++ ) {
			for (int j=1 ; j<=N ; j++ ) {
				div[IJK(i,j)] = -0.5f*h*(u[IJK(i+1,j)]-u[IJK(i-1,j)]+ v[IJK(i,j+1)]-v[IJK(i,j-1)]); 
				p[IJK(i,j)] = 0;
			}
		}
		set_bnd (0, div); set_bnd (0, p);

		for (int it=0 ; it<iterations ; it++) {
			for (int i=1 ; i<=N ; i++ ) {
				for (int j=1 ; j<=N ; j++ ) {
					p[IJK(i,j)] = (div[IJK(i,j)]+p[IJK(i-1,j)]+p[IJK(i+1,j)]+p[IJK(i,j-1)]+p[IJK(i,j+1)])/4 ;
				}
			}
			set_bnd (0, p); 
		}

		for (int i=1 ; i<=N ; i++) {
			for (int j=1 ; j<=N ; j++) {
				u[IJK(i,j)] -= 0.5f*(p[IJK(i+1,j)]-p[IJK(i-1,j)])/h;
				v[IJK(i,j)] -= 0.5f*(p[IJK(i,j+1)]-p[IJK(i,j-1)])/h;
			}
		}
		set_bnd (1, u); set_bnd (2, v); 
	}
	
	void dens_step (float[] x, float[] x0, float[] u, float[] v, float dt ) {
		add_source (x, x0, dt);
		SWAP (x0, x); 
		diffuse (0, x, x0, dt); 
		SWAP (x0, x); 
		advect (0, x, x0, u, v, dt);
	}

	void vel_step (float[] u, float[] v, float[] u0, float[] v0,
	               float dt ) {
		add_source (u, u0, dt); 
		add_source (v, v0, dt);
		SWAP (u0, u); 
		SWAP (v0, v); 
		diffuse (1, u, u0, dt);
		diffuse (2, v, v0, dt);
		project (u, v, u0, v0);
		SWAP (u0, u); 
		SWAP (v0, v);
		advect (1, u, u0, u0, v0, dt); 
		advect (2, v, v0, u0, v0, dt); 
		project (u, v, u0, v0);
	}

	void UserInput() {
		// draw on the water
		if (Input.GetMouseButton(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100)) {
				// determine indices where the user clicked
				int x = (int)(hit.point.x * N);
				int y = (int)(hit.point.z * N);
				int i = (x + 1) + (y + 1) * rowSize;
				if (x < 1 || x > N-1 || y < 1 || y > N-1) return;
				// add or dec density
				dens_prev[i] += add ? 3f : -3f;
				add = !add;
				// add velocity
				u_prev[i] += Input.GetAxis("Mouse X") * 0.5f;
				v_prev[i] += Input.GetAxis("Mouse Y") * 0.5f;
			}
		}
	}

	void Draw() {
		// visualize water
		for (int y = 0; y < N; y++) {
			for (int x = 0; x < N; x++) {
				int i = (x + 1) + (y + 1) * rowSize;
				float d = 5f * dens[i];
				tex.SetPixel(x, y, new Color(u[i]*20 + 0.5f, v[i]*20 + 0.5f + d * 0.5f, 1 + d));
				/*float d = 5f * dens[(x + 1) + (y + 1) * rowSize];
				tex.SetPixel(x, y, new Color(0, d * 0.5f, d));*/
			}
		}
		tex.Apply(false);
	}
}
