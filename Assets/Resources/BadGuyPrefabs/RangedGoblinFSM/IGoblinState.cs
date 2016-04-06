using UnityEngine;
using System.Collections;

public interface IGoblinState{


		void UpdateState ();

		void ToPathFindingState();

		void ToAttackState();

		void ToWaitState();

}
