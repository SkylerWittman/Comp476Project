using UnityEngine;
using System.Collections;

public class RangedGoblinThrow : MonoBehaviour {


	public GameObject throwingAxe;
	public GameObject spear;
	private GameObject player;
	private Vector3 playerTarget;
	private float distanceToAttack = 40;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerTarget = player.transform.position;
		Random.seed = System.DateTime.Now.Millisecond;
	}


	private void ThrowWeapon(){
		int rand = Random.Range (0, 2);

		if (rand == 0) {

		}

		if (rand == 1) {

		}



	}


	// Update is called once per frame
	void Update () {
	
		if (Vector3.Distance (transform.position, playerTarget) < distanceToAttack) {
			//throw weapon
		}

	}
}
