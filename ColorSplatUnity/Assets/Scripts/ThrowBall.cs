using UnityEngine;
using System.Collections;

public class ThrowBall : MonoBehaviour {

	public Camera cam;
	public GameObject ball;
	public Vector3 throwSpeed; 
	private Vector3 ballPos; 
	private GameObject ballClone; 
	private float X, Y;

	void Start () {
		X = Y = -1.0f;
	}

	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);
			launchBall(mousePos);
		}

		if (X > 0 && Y > 0) {
			Vector3 tuioPos = new Vector3(X, Y, cam.nearClipPlane);
			launchBall(tuioPos);
		}
	}

	public float tuioX {
		get {return X;}
		set {X = value;}
	}
	
	public float tuioY {
		get {return Y;}
		set {Y = value;}
	}

	void launchBall (Vector3 pos) {
		ballPos = cam.ScreenToWorldPoint(pos) + new Vector3(0, 0, 0.1f);
		ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
		//			Color randColor = new Color(Random.Range(0, 255)/255, Random.Range(0, 255)/255, Random.Range(0, 255)/255);
		//			ballClone.GetComponent<MeshRenderer>().material.SetColor("_Albedo", randColor);
		ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);

	}
}
