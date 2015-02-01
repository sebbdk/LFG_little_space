using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {

	// Update is called once per frame
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			PlayerController c = collider.gameObject.GetComponent<PlayerController>();
			c.wayPoint = transform.position;
			Destroy(gameObject);
		}
	}

}
