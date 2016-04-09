using UnityEngine;
using System.Collections;

public class AttackState : IGoblinState {

	private readonly StatePattern rangeGoblin;
	private float distanceToAttack = 75;
	private float timer = 0;
	private Vector3 directionToLook;

	public AttackState(StatePattern pattern){
		rangeGoblin = pattern;

	}

	public void UpdateState (){
		timer -= Time.deltaTime;
		LookForTarget ();
	
		Quaternion newRotation = Quaternion.LookRotation (rangeGoblin.playerTarget - rangeGoblin.transform.position);
		rangeGoblin.transform.rotation = Quaternion.RotateTowards (rangeGoblin.transform.rotation, newRotation, 400.0f* Time.deltaTime);

		if (timer <= 0) {
			Attack ();
			timer = 2.5f;
		}
	}

	public void ToAttackState(){
		Debug.Log ("currently in state");
	}

	public void ToPathFindingState(){
		rangeGoblin.currentState = rangeGoblin.pathFindState;
	}
		

	public void ToWaitState(){
		rangeGoblin.currentState = rangeGoblin.waitState;
	}

	 private void Attack(){
		
		int rand = Random.Range (0, 2);

		if (rand == 0) {
			rangeGoblin.ThrowAxe ();
		}

		if (rand == 1) {
			rangeGoblin.ThrowSpear ();
		}


		rangeGoblin.anim.Play(rangeGoblin.throwClip.name);


	}

	private void LookForTarget(){
		if (Vector3.Distance (rangeGoblin.transform.position, rangeGoblin.playerTarget) > distanceToAttack) {
			ToPathFindingState ();

		}
	}
}
