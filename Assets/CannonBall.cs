using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	public Vector3 dir = new Vector3 (1, 0, 0);
	public uint speed = 10;

	void Update() {
		transform.Translate (new Vector3 (dir.x, dir.y, dir.z) * speed * Time.deltaTime);
	}

}
