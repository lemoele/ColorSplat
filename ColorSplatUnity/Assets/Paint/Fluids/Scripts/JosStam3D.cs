using UnityEngine;
using System.Collections;

public class JosStam3D : MonoBehaviour {

	public int N;
	public float dt = 0.8f;
	public float visc = 0.2f;
	public int iterations = 10;

	int size;
	float[] u, v, w;
	float[] u_prev, v_prev, w_prev;
	float[] dens, dens_prev;

	void Start () {
		size = (N+2)*(N+2)*(N+2);
		dens = new float[size];
		dens_prev = new float[size];
		u = new float[size];
		u_prev = new float[size];
		v = new float[size];
		v_prev = new float[size];
		w = new float[size];
		w_prev = new float[size];
	}

	void Update () {
		// TODO : input

//		get_from_UI ( dens_prev, u_prev, v_prev); 
		vel_step (u, v, w, u_prev, v_prev, w_prev, dt); 
		dens_step (dens, dens_prev, u, v, w, dt); 
//		draw_dens (dens);
	}

	int IJK(int i, int j, int k) {
		return (i + j*(N+2) + k*(N+2)*(N+2));
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
		for (int i=1 ; i<=N ; i++) { 
			for (int j=1; j<=N; j++) {
				x[IJK(0,i,j)] 	= b==1 ? (-1f)*x[IJK(1,i,j)] : x[IJK(1,i,j)];
				x[IJK(N+1,i,j)] = b==1 ? (-1f)*x[IJK(N,i,j)] : x[IJK(N,i,j)]; 
				x[IJK(i,0,j)] 	= b==2 ? (-1f)*x[IJK(i,1,j)] : x[IJK(i,1,j)];
				x[IJK(i,N+1,j)] = b==2 ? (-1f)*x[IJK(i,N,j)] : x[IJK(i,N,j)]; 
				x[IJK(i,j,0)] 	= b==3 ? (-1f)*x[IJK(i,j,1)] : x[IJK(i,j,1)];
				x[IJK(i,j,N+1)] = b==3 ? (-1f)*x[IJK(i,j,N)] : x[IJK(i,j,N)]; 
			}
		}
		x[IJK(0,0,0)] 		= 0.5f * (x[IJK(1,0,0)]		+ x[IJK(0,1,0)]		+ x[IJK(0,0,1)]);
		x[IJK(0,0,N+1)] 	= 0.5f * (x[IJK(1,0,N+1)]	+ x[IJK(0,1,N+1)]	+ x[IJK(0,0,N)]);
		x[IJK(0,N+1,0)] 	= 0.5f * (x[IJK(1,N+1,0)]	+ x[IJK(0,N,0)]		+ x[IJK(0,N+1,1)]);
		x[IJK(0,N+1,N+1)] 	= 0.5f * (x[IJK(1,N+1,N+1)]	+ x[IJK(0,N,N+1)]	+ x[IJK(0,N+1,N)]);
		x[IJK(N+1,0,0)] 	= 0.5f * (x[IJK(N,0,0)]		+ x[IJK(N+1,1,0)]	+ x[IJK(N+1,0,1)]);
		x[IJK(N+1,0,N+1)] 	= 0.5f * (x[IJK(N,0,N+1)]	+ x[IJK(N+1,1,N+1)]	+ x[IJK(N+1,0,N)]);
		x[IJK(N+1,N+1,0)] 	= 0.5f * (x[IJK(N,N+1,0)]	+ x[IJK(N+1,N,0)]	+ x[IJK(N+1,N+1,1)]);
		x[IJK(N+1,N+1,N+1)] = 0.5f * (x[IJK(N,N+1,N+1)]	+ x[IJK(N+1,N,N+1)]	+ x[IJK(N+1,N+1,N)]);
	}

	void diffuse (int b, float[] x, float[] x0, float dt) {
		float a = dt*visc*N*N*N;
		for (int it=0 ; it<iterations ; it++ ) {
			for (int i=1 ; i<=N ; i++ ) {
				for (int j=1 ; j<=N ; j++ ) {
					for (int k=1 ; k<=N ; k++ ) {
						float sumx = x[IJK(i-1,j,k)]+x[IJK(i+1,j,k)];
						float sumy = x[IJK(i,j-1,k)]+x[IJK(i,j+1,k)];
						float sumz = x[IJK(i,j,k-1)]+x[IJK(i,j,k-1)];
						x[IJK(i,j,k)] = (x0[IJK(i,j,k)] + a*(sumx+sumy+sumz))/(1+6*a);
					}
				}
			}
			set_bnd (b, x);
		}
	}
	
	void advect (int b, float[] d, float[] d0, float[] u, float[] v, float[] w, float dt) {
		int i0, j0, k0, i1, j1, k1;
		float x, y, z, r0, s0, t0, r1, s1, t1, dt0;
		dt0 = dt*N;
		for (int i=1 ; i<=N ; i++ ) {
			for (int j=1 ; j<=N ; j++ ) {
				for (int k=1 ; k<=N ; k++ ) {
					x = i-dt0*u[IJK(i,j,k)]; 
					y = j-dt0*v[IJK(i,j,k)];
					z = k-dt0*w[IJK(i,j,k)];
					if (x<0.5f) x=0.5f; 
					if (x>N+0.5f) x=N+0.5f; i0=(int)x; i1=i0+1;
					if (y<0.5f) y=0.5f; 
					if (y>N+0.5f) y=N+0.5f; j0=(int)y; j1=j0+1; 
					if (z<0.5f) z=0.5f; 
					if (z>N+0.5f) z=N+0.5f; k0=(int)z; k1=k0+1; 
					r1 = x-i0; r0 = 1-r1; 
					s1 = y-j0; s0 = 1-s1;
					t1 = z-k0; t0 = 1-t1;
					float da = r0*(s0*d0[IJK(i0,j0,k0)]+s1*d0[IJK(i0,j1,k0)])
						+ r1*(s0*d0[IJK(i1,j0,k0)]+s1*d0[IJK(i1,j1,k0)]);
					float db = r0*(s0*d0[IJK(i0,j0,k1)]+s1*d0[IJK(i0,j1,k1)])
						+ r1*(s0*d0[IJK(i1,j0,k1)]+s1*d0[IJK(i1,j1,k1)]);
					d[IJK(i,j,k)] = t0*da+t1*db;
				}
			}
		}
		set_bnd (b, d); 
	}

	void project (float[] u, float[] v, float[] w, float[] p, float[] div, float[] div2) {
		// TODO : find out what div2 should be doing !!!
		float h = 1.0f/N;
		for (int i=1 ; i<=N ; i++ ) {
			for (int j=1 ; j<=N ; j++ ) {
				for (int k=1 ; k<=N ; k++ ) {
					div[IJK(i,j,k)] = -0.5f*h*(	u[IJK(i+1,j,k)]-u[IJK(i-1,j,k)] + 
					                           	v[IJK(i,j+1,k)]-v[IJK(i,j-1,k)] + 
					                           	w[IJK(i,j,k+1)]-w[IJK(i,j,k-1)] ); 
					p[IJK(i,j,k)] = 0;
				}
			}
		}
		set_bnd (0, div); 
		set_bnd (0, p);

		for (int it=0 ; it<iterations ; it++) {
			for (int i=1 ; i<=N ; i++ ) {
				for (int j=1 ; j<=N ; j++ ) {
					for (int k=1 ; k<=N ; k++ ) {
						p[IJK(i,j,k)] = (	div[IJK(i,j,k)] + 
						               		p[IJK(i-1,j,k)] + p[IJK(i+1,j,k)] + 
						               		p[IJK(i,j-1,k)] + p[IJK(i,j+1,k)] + 
						               		p[IJK(i,j,k-1)] + p[IJK(i,j,k+1)]	)/6 ;
					}
				}
			}
			set_bnd (0, p); 
		}

		for (int i=1 ; i<=N ; i++) {
			for (int j=1 ; j<=N ; j++) {
				for (int k=1 ; k<=N ; k++ ) {
					u[IJK(i,j,k)] -= 0.5f*(p[IJK(i+1,j,k)]-p[IJK(i-1,j,k)])/h;
					v[IJK(i,j,k)] -= 0.5f*(p[IJK(i,j+1,k)]-p[IJK(i,j-1,k)])/h;
					w[IJK(i,j,k)] -= 0.5f*(p[IJK(i,j,k+1)]-p[IJK(i,j,k-1)])/h;
				}
			}
		}
		set_bnd (1, u); 
		set_bnd (2, v); 
		set_bnd (3, w); 
	}
	
	void dens_step (float[] x, float[] x0, float[] u, float[] v, float[] w, float dt ) {
		add_source (x, x0, dt);
		SWAP (x0, x); 
		diffuse (0, x, x0, dt); 
		SWAP (x0, x); 
		advect (0, x, x0, u, v, w, dt);
	}

	void vel_step (float[] u, float[] v, float[] w, float[] u0, float[] v0, float[] w0, float dt )
	{
		add_source (u, u0, dt); 
		add_source (v, v0, dt);
		add_source (w, w0, dt);
		SWAP (u0, u); 
		SWAP (v0, v); 
		SWAP (w0, w); 
		diffuse (1, u, u0, dt);
		diffuse (2, v, v0, dt);
		diffuse (3, w, w0, dt);
		project (u, v, w, u0, v0, w0);
		SWAP (u0, u); 
		SWAP (v0, v);
		SWAP (w0, w); 
		advect (1, u, u0, u0, v0, w0, dt); 
		advect (2, v, v0, u0, v0, w0, dt); 
		advect (3, w, w0, u0, v0, w0, dt); 
		project (u, v, w, u0, v0, w0);
	}
}
