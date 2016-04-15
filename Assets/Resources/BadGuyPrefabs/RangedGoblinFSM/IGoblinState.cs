using UnityEngine;
using System.Collections;

public interface IGoblinState{

		//interface used for the goblin FSM state machien pattern
		void UpdateState ();

		void ToPathFindingState();

		void ToAttackState();

		void ToWaitState();

}
