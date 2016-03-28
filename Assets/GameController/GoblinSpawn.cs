using UnityEngine;
using System.Collections;

public class GoblinSpawn : MonoBehaviour {

	public GameObject goblin;
	private GameObject target; //player is the target
	public float distanceToSpawn = 10;
	private bool canSpawn = true;
	public float timeToRespawn = 30.0f;


	void Start () {
		target = GameObject.FindGameObjectWithTag("Player");
		Random.seed = System.DateTime.Now.Millisecond;
	}
	
	//only one goblin can spawn from a certain tree every 30 seconds, this resets the spawn restriction every 30 seconds
	IEnumerator resetSpawn(){
		yield return new WaitForSeconds (timeToRespawn); 
		canSpawn = true;
	}

	void Update () {
		//if the player comes within 5 units of a tree then a goblin will spawn from above in the tree
		if (Vector3.Distance (this.transform.position, target.transform.position) < distanceToSpawn && canSpawn) {
			canSpawn = false;

			//tree marker is at base of tree so just spawn the goblins a bit higher and to the right, we can implement random values here if we think its nessecary
			//generate a random number and modd it by 2, instantiate a goblin if number = 0, if 1 do nothing

			int rand = Random.Range (0, 200);
			rand %= 2;

			if (rand == 0) {
				Instantiate (goblin, new Vector3 (this.transform.position.x, this.transform.position.y + 40, this.transform.position.z + 10), Quaternion.identity);
				Debug.Log ("Goblin");
			}

			StartCoroutine (resetSpawn ());
		}
	}
}
