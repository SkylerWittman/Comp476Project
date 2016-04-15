using UnityEngine;
using System.Collections;

public class AttackState : IGoblinState {

	private readonly StatePattern rangeGoblin;
	private float distanceToAttack = 75;
	private float timer = 0;
	private float rotateSpeed = 8.0f;
	private Vector3 directionToLook;

	public AttackState(StatePattern pattern){
		rangeGoblin = pattern;

	}

	public void UpdateState (){
		timer -= Time.deltaTime;
		LookForTarget ();
	
		Vector3 newRotation = Vector3.RotateTowards (rangeGoblin.transform.forward, rangeGoblin.playerTarget - rangeGoblin.transform.position, rotateSpeed * Time.deltaTime, 0.0f);
		newRotation.y = 0.0f;
		rangeGoblin.transform.rotation = Quaternion.LookRotation (newRotation);

		if (timer <= 0) {
			Attack ();
			timer = 2.5f;
		}
	}

	//changes states
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

		//throwing an axe or a spear is choosen randomly
		int rand = Random.Range (0, 2);

		if (rand == 0) {
			rangeGoblin.ThrowAxe ();
		}

		if (rand == 1) {
			rangeGoblin.ThrowSpear ();
		}


		rangeGoblin.anim.Play(rangeGoblin.throwClip.name);


	}

	private void LookForTarget(){ //if player is out of attacking range go back to pathfinding
		if (Vector3.Distance (rangeGoblin.transform.position, rangeGoblin.playerTarget) > distanceToAttack) {
			ToPathFindingState ();

		}
	}
}
