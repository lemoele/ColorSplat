using UnityEngine;
using System.Collections;

public class ThrowBall : MonoBehaviour {

	public Camera cam;
	public GameObject ball;
	public Vector3 throwSpeed; 
	private Vector3 ballPos; 
	private GameObject ballClone; 
	private float X, Y;
	float ScreenSize;
	float newXAngle;
	float newYAngle;

	void Start () {
		X = Y = -1.0f;
		ScreenSize = Screen.width;
	}

	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);
			//Debug.Log ("Mouse pos : " + mousePos);

			newXAngle = Input.mousePosition.x / 100 -20;
			newYAngle = Input.mousePosition.y /100 - 10;

			//Debug.Log("Screen " + ScreenSize + " " + "Mouse" + Input.mousePosition.x);

			throwSpeed = new Vector3 (newXAngle, newYAngle, 20);

			ballPos = cam.ScreenToWorldPoint(mousePos) + new Vector3(0, 0, 0.1f);
			//Debug.Log ("Ball pos : " + ballPos.ToString("F7"));
			ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
			ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
		}

		if (X > 0 && Y > 0) {
			Vector3 tuioPos = new Vector3(X * 4055, Y * 2430, cam.nearClipPlane);
			// Works on my computer for x and y inverted, weird.
			//Vector3 tuioPos = new Vector3(Y * 4000, X * 2000, cam.nearClipPlane);
			Debug.Log ("Tuio pos : " + tuioPos.ToString("F7"));
			
			newXAngle = tuioPos.x / 130*1.4f -21;
			newYAngle = -tuioPos.y*tuioPos.y/100000+4.5f;

			Debug.Log("Screen " + ScreenSize + " " + "Tuio" + -tuioPos.x);
			Debug.Log("newYAngle" + newYAngle);

			throwSpeed = new Vector3 (newXAngle, newYAngle, 30);

			ballPos = cam.ScreenToWorldPoint(tuioPos) + new Vector3(0, 0, 0.1f);
			
			Debug.Log ("Ball pos : " + ballPos.ToString("F7"));
			// Works on my computer for x inverted, weird.
			//ballPos.x = -ballPos.x;
			ballPos.x = ballPos.x * 1.36f;
			ballPos.y = -ballPos.y * 1.33f + 20.15f;
			Debug.Log ("Ball pos : " + ballPos.ToString("F7"));
			
			ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
			//			Color randColor = new Color(Random.Range(0, 255)/255, Random.Range(0, 255)/255, Random.Range(0, 255)/255);
			//			ballClone.GetComponent<MeshRenderer>().material.SetColor("_Albedo", randColor);
			ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
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
}
