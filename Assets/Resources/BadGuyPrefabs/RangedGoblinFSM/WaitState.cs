using UnityEngine;
using System.Collections;

public class WaitState : IGoblinState {


	private readonly StatePattern rangeGoblin;
	private float distanceToAttack = 75;

	public WaitState(StatePattern pattern){
		rangeGoblin = pattern;

	}

	public void UpdateState (){
		
		LookForTarget ();
		rangeGoblin.startWait ();

		if (rangeGoblin.canChangeState) {
			rangeGoblin.canChangeState = false;
			ToPathFindingState ();
		}
	}
		

	public void ToPathFindingState(){
		rangeGoblin.currentState = rangeGoblin.pathFindState;
	}

	public void ToAttackState(){
		rangeGoblin.currentState = rangeGoblin.attackState;
	}

	public void ToWaitState(){
		Debug.Log ("current state");
	}

	private void LookForTarget(){
		if (Vector3.Distance (rangeGoblin.transform.position, rangeGoblin.playerTarget) <= distanceToAttack) {
			ToAttackState ();

		}
	}
}
