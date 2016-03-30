using UnityEngine;
using System.Collections;

public class SwarmController : MonoBehaviour {

	private GameObject[] SwarmGoblins;
	private Vector3 wanderVector;
	public GameObject spawnEngine;
	private WaveEngine targetGenerator;

	void Start () {
		SwarmGoblins = GameObject.FindGameObjectsWithTag ("SwarmGoblin");
		InvokeRepeating ("UpdateSwarm", 10.0f, 20.0f);
		InvokeRepeating ("SetWanderTarget", 0.0f, 8.0f);
		targetGenerator = spawnEngine.GetComponent<WaveEngine> ();

	}
	
	public void SetWanderTarget(){
		float[] XZ = targetGenerator.getXZ ();
		wanderVector = new Vector3 (XZ [0], -3.0f, XZ [1]);

	}


	public void UpdateSwarm(){
		SwarmGoblins = GameObject.FindGameObjectsWithTag ("SwarmGoblin");
	}

	public GameObject[] getSwarm(){

		return SwarmGoblins;
	}

	public Vector3 GetWanderTarget(){

		return wanderVector;
	}
		
}
