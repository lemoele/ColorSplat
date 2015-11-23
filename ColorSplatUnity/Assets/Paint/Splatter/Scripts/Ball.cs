using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public GameObject splatter;
	public GameObject particles;
	private GameObject particlesClone;

	void OnCollisionEnter(Collision collision) {
//		Vector3 new_pos = collision.contacts[0].point + new Vector3(0, 0, -0.5f);
//		Instantiate(splatter, new_pos, Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal));
//		Instantiate(splatter, new_pos, Quaternion.identity);
		particlesClone = Instantiate(particles, collision.contacts[0].point, Quaternion.identity) as GameObject;
		Destroy(particlesClone, 12);
		Destroy(this.gameObject);

	}
}
