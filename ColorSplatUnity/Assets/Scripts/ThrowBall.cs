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
			Debug.Log ("Mouse pos : " + mousePos);
			launchBall(mousePos);
		}

		if (X > 0 && Y > 0) {
//			Vector3 tuioPos = new Vector3(X * 1000, Y * 1000, cam.nearClipPlane);
			// Works on my computer for x and y inverted, weird.
			Vector3 tuioPos = new Vector3(Y * 1000, X * 1000, cam.nearClipPlane);
			Debug.Log ("Tuio pos : " + tuioPos.ToString("F7"));
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

		Debug.Log ("Ball pos : " + ballPos.ToString("F7"));
		// Works on my computer for x inverted, weird.
		ballPos.x = -ballPos.x;

		ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
		//			Color randColor = new Color(Random.Range(0, 255)/255, Random.Range(0, 255)/255, Random.Range(0, 255)/255);
		//			ballClone.GetComponent<MeshRenderer>().material.SetColor("_Albedo", randColor);
		ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);

	}
}
