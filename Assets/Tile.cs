using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	Color activeColor = new Color(1,1,1);
	Color inActiveColor = new Color(0.6f,0.6f,0.6f);

	Vector3 currentTile;

	void updateTile() {
		currentTile = new Vector3 (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.y), Mathf.RoundToInt (transform.position.z));
	}
	
	// Update is called once per frame
	void Update () {
		GameObject playerObject = GameObject.Find ("Player");

		if(playerObject != null) {
			PlayerController playerCon = (PlayerController) playerObject.GetComponent ("PlayerController");

			if(playerCon) {
				updateTile();

				if(playerCon.currentTile.Equals(currentTile)) {
					gameObject.renderer.material.color = activeColor;
				} else {
					gameObject.renderer.material.color = inActiveColor;
				}
			}
		}
	}
}
