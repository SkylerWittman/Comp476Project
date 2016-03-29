using UnityEngine;
using System.Collections;

public class SwarmController : MonoBehaviour {

	private GameObject[] SwarmGoblins;
	private Vector3 wanderVector = new Vector3();

	void Start () {
		SwarmGoblins = GameObject.FindGameObjectsWithTag ("SwarmGoblin");
		InvokeRepeating ("UpdateSwarm", 10.0f, 20.0f);
		InvokeRepeating ("SetWanderTarget", 1.0f, 8.0f);
	}
	
	public void SetWanderTarget(){

		//find random position on the map
	}


	public void UpdateSwarm(){
		SwarmGoblins = GameObject.FindGameObjectsWithTag ("SwarmGoblin");
	}

	public GameObject[] getSwarm(){

		return SwarmGoblins;
	}

	public Vector3 GetWanderDirection(){

		return wanderVector;
	}
		
}
