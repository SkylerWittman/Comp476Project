using UnityEngine;
using System.Collections;

public class AttackState : IGoblinState {

	private readonly StatePattern rangeGoblin;
	private float distanceToAttack = 75;

	public AttackState(StatePattern pattern){
		rangeGoblin = pattern;

	}

	public void UpdateState (){
		Attack ();
		LookForTarget ();
	}

	public void ToAttackState(){
	}

	public void ToPathFindingState(){
		rangeGoblin.currentState = rangeGoblin.pathFindState;
	}
		

	public void ToWaitState(){
		rangeGoblin.currentState = rangeGoblin.waitState;
	}

	 private void Attack(){
		//trhow axe
	}

	private void LookForTarget(){
		if (Vector3.Distance (rangeGoblin.transform.position, rangeGoblin.playerTarget) > distanceToAttack) {
			ToPathFindingState ();
		}
	}

}
