using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public GameObject splatterPrefab;

	void OnCollisionEnter(Collision collision) {
		Vector3 new_pos = collision.contacts[0].point + new Vector3(0, 0, -0.1f);
		Instantiate(splatterPrefab, new_pos, Quaternion.identity);
		Destroy(this.gameObject);
	}
}
