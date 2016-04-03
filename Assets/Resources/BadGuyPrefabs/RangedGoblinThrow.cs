using UnityEngine;
using System.Collections;

public class RangedGoblinThrow : MonoBehaviour {


	public GameObject throwingAxe;
	public GameObject spear;
	private GameObject player;
	public GameObject throwPosition;
	private Vector3 playerTarget;
	private float distanceToAttack = 100;
	private Animation anim;
	private AnimationClip throwClip;
	private Vector3 throwDirection;
	private float attackTimer = 120.0f;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerTarget = player.transform.position;
		Random.seed = System.DateTime.Now.Millisecond;
	}


	private void ThrowWeapon(){
		int rand = Random.Range (0, 2);

		if (rand == 0) {
			Instantiate (throwingAxe, throwPosition.transform.position, Quaternion.identity);
		}

		if (rand == 1) {
			Instantiate (spear, throwPosition.transform.position, Quaternion.identity);
		}



	}


	// Update is called once per frame
	void Update () {
		attackTimer--;

		if (Vector3.Distance (transform.position, playerTarget) < distanceToAttack && attackTimer < 0.0f) {
			ThrowWeapon ();
			attackTimer = 120.0f;
		}

	}
}
