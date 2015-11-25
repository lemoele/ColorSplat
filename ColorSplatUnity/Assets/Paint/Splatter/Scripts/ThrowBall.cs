using UnityEngine;
using System.Collections;

public class ThrowBall : MonoBehaviour {

	public Camera cam;
	public GameObject ball;
	public Vector3 throwSpeed; 
	private Vector3 ballPos; 
	private GameObject ballClone; 
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0))
		{
			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);
			ballPos = cam.ScreenToWorldPoint(mousePos) + new Vector3(0, 0, 0.1f);
			ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;

//			Color randColor = new Color(Random.Range(0, 255)/255, Random.Range(0, 255)/255, Random.Range(0, 255)/255);
//			ballClone.GetComponent<MeshRenderer>().material.SetColor("_Albedo", randColor);

			ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
		}
	}
}
