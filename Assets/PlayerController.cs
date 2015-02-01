using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 3f;

	public Vector3 previousTile;
	public Vector3 currentTile;
	public Vector3 wayPoint = new Vector3();

	public GameObject tile;

	public GameObject preview;
	private GameObject[] previews = new GameObject[4];

	public GameObject activeTile;
	Color activeTileColor = new Color(1,1,1);
	Color inActiveTileColor = new Color(0.6f,0.6f,0.6f);

	void Start() { 
		//create preview tiles
		for (int c = 0; c < 4; c++) {
			GameObject p = (GameObject) Instantiate(preview,
			                                             transform.position,
			                                             transform.rotation);
			p.SetActive(false);
			previews[c] = p;
		}


		updateTile();
		wayPoint = currentTile;
	}

	void FixedUpdate()
	{
		updateTile ();

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		bool shift = Input.GetKey (KeyCode.LeftShift);

		if ( (h != 0 || v != 0) && !shift )  {
			rigidbody.velocity = new Vector3 (h * speed, rigidbody.velocity.y, v * speed);
		} else {
			rigidbody.velocity = new Vector3 (0, rigidbody.velocity.y, 0);
		}

		//take a tile
		if (shift && tile == null) {
			GameObject closestTile = getLeNearestTile(0.1f, currentTile);

			if(closestTile && closestTile.tag == "tile") {
				tile = closestTile;
				tile.SetActive(false);

				closestTile = getLeNearestTile(1f, currentTile);
				float yy = closestTile.transform.position.y + 0.2f;
				transform.position = new Vector3((float) closestTile.transform.position.x, (float) yy, (float) closestTile.transform.position.z);
			}
		}

		if (tile != null) {
			previewGhostTiles();		
		}

		deathHandler ();
	}

	void deathHandler() {
		if (rigidbody.position.y < -2 ) {
			//Application.LoadLevel(0);
			transform.position = new Vector3(wayPoint.x, wayPoint.y + 1f, wayPoint.z);
			updateTile();
			resetTiles();
		}
	}

	void resetTiles() {
		Vector3[] checkTiles = {
			new Vector3 (currentTile.x, 0, currentTile.z),
			new Vector3 (currentTile.x, 0, currentTile.z + 1),
			new Vector3 (currentTile.x, 0, currentTile.z - 1),
			new Vector3 (currentTile.x - 1, 0, currentTile.z),
			new Vector3 (currentTile.x + 1 , 0, currentTile.z)
		};

		GameObject[] tiles = GameObject.FindGameObjectsWithTag("tile");
		foreach (GameObject checkTile in tiles) {
			checkTile.SetActive(false);
		}

		int tileIndex = 0;
		foreach (Vector3 tilePos in checkTiles) {
			GameObject existingTile = getLeNearestTile(0.45f, tilePos);

			if(tileIndex < tiles.Length && existingTile == null) {
				tiles[tileIndex].transform.position = tilePos;
				tiles[tileIndex].SetActive(true);
				tileIndex++;
			}
		}
	}

	void previewGhostTiles() {
		Vector3[] checkTiles = {
			new Vector3 (currentTile.x, 0, currentTile.z + 1),
			new Vector3 (currentTile.x, 0, currentTile.z - 1),
			new Vector3 (currentTile.x - 1, 0, currentTile.z),
			new Vector3 (currentTile.x + 1 , 0, currentTile.z)
		};

		int previewIndex = 0;
		int s = 0;

		foreach (Vector3 tilePos in checkTiles) {
			GameObject ga =  getLeNearestTile(0.45f, new Vector3(tilePos.x, tilePos.y, tilePos.z));
			if(ga == null) {
				previews[previewIndex].transform.position = new Vector3(tilePos.x, tilePos.y, tilePos.z);
				previews[previewIndex].SetActive(true);

				if(Vector3.Distance(tilePos, transform.position) <= 0.8) {
					tile.transform.position = new Vector3(tilePos.x, tilePos.y, tilePos.z);
					tile.SetActive(true);
					tile = null;
					
					foreach(GameObject prev in previews) {
						prev.SetActive(false);
					}
					return;
				}

				s++;
			} else {
				previews[previewIndex].SetActive(false);
			}

			previewIndex++;
		}
	}

	void updateTile() {
		currentTile = new Vector3 (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.y), Mathf.RoundToInt (transform.position.z));
	}

	public GameObject getLeNearestTile(float range,  Vector3 from) {
		Collider[] colliders;
		colliders = Physics.OverlapSphere (from, range);

		Collider resultCollider = null;
		if (colliders.Length >= 1) { //Presuming the object you are testing also has a collider 0 otherwise
			int c = 0;
			foreach (var col in colliders) {	
				if (col.tag == "tile" || col.tag == "waypoint_tile") {
					if(resultCollider == null) {
						resultCollider = col;
					}
					
					if(resultCollider.tag != "tile" && col.tag == "tile") {
						resultCollider = col;
					}
					//return collider.gameObject;
				}
			}
		}

		if (!resultCollider) {
			return null;
		}

		return resultCollider.gameObject;
	}

}
