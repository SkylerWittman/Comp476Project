using UnityEngine;
using System.Collections;

public class SwarmController : MonoBehaviour {

	private GameObject[] SwarmGoblins;


	void Start () {
		SwarmGoblins = GameObject.FindGameObjectsWithTag ("SwarmGoblin");
		InvokeRepeating ("UpdateSwarm", 10.0f, 20.0f);
	}
	


	public void UpdateSwarm(){
		SwarmGoblins = GameObject.FindGameObjectsWithTag ("SwarmGoblin");
	}

	public GameObject[] getSwarm(){

		return SwarmGoblins;
	}
		
}
