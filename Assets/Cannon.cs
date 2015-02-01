using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public CannonBall cannonBall;
	public float interval = 1;
	Vector3 dir = new Vector3 (0, 0, 1);

	private GameObject barrel;

	private float enabledTime = 0;
	private bool chargin = false;

	private Animator barrelAnimator;
	private int shootHash = Animator.StringToHash("shoot");

	void Start() {
		//cannonBall.position = new Vector3 (40, 40, 40);
		enabledTime = Time.time;

		barrel = transform.Find("barrel").gameObject;
		barrelAnimator = barrel.GetComponent<Animator> ();

	}

	void shoot() {

		CannonBall bullet = (CannonBall) Instantiate(cannonBall,
		                                                transform.position,
		                                                transform.rotation);
		if (barrel != null) {
			barrelAnimator.StopPlayback();
			barrelAnimator.Play(shootHash, -1, 0);
		}

		bullet.dir = dir;
	}

	// Update is called once per frame
	void Update () {
		if(Time.time > enabledTime+interval - 1 && !chargin && barrel != null) {
			barrelAnimator.Play("charge", -1, 0);
			chargin = true;
		}

		if(Time.time > enabledTime+interval) {
			enabledTime = Time.time; 
			chargin = false;
			shoot();
		}
	}
}
