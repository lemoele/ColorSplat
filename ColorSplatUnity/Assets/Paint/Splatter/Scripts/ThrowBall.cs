using UnityEngine;
using System.Collections;

public class ThrowBall : MonoBehaviour {

	public GameObject ball;
	public Vector3 throwSpeed; 
	public Vector3 ballPos; 
	private GameObject ballClone; 
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown("space"))
		{
			ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
			Color randColor = new Color(Random.Range(0, 255)/255, Random.Range(0, 255)/255, Random.Range(0, 255)/255);
			ballClone.GetComponent<MeshRenderer>().material.SetColor("_Albedo", randColor);
			ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
		}
	}
}
