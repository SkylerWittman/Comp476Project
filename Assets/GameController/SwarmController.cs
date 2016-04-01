using UnityEngine;
using System.Collections;

public class SwarmController : MonoBehaviour {

	private GameObject[] swarmSpiders;
	private Vector3 wanderVector;
	public GameObject spawnEngine;
	private WaveEngine targetGenerator;

	void Start () {
		swarmSpiders = GameObject.FindGameObjectsWithTag ("SwarmSpider");
		InvokeRepeating ("SetWanderTarget", 0.0f, 8.0f);
		targetGenerator = spawnEngine.GetComponent<WaveEngine> ();

	}
	
	public void SetWanderTarget(){
		float[] XZ = targetGenerator.getXZ ();
		wanderVector = new Vector3 (XZ [0], -3.0f, XZ [1]);

	}


	public void UpdateSwarm(){
		swarmSpiders = GameObject.FindGameObjectsWithTag ("SwarmSpider");
	}

	public GameObject[] getSwarm(){

		return swarmSpiders;
	}

	public Vector3 GetWanderTarget(){

		return wanderVector;
	}
		
}
