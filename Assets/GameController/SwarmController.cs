using UnityEngine;
using System.Collections;

public class SwarmController : MonoBehaviour {

	private GameObject[] swarmSpiders;
	private Vector3 wanderVector;
	public GameObject spawnEngine;
	private WaveEngine targetGenerator;

	void Start () {
		swarmSpiders = GameObject.FindGameObjectsWithTag ("SwarmSpider");
		InvokeRepeating ("SetWanderTarget", 0.0f, 8.0f); //randomly sets the wander target for the swarm spiders every 8 seconds
		targetGenerator = spawnEngine.GetComponent<WaveEngine> ();

	}
	
	public void SetWanderTarget(){
		float[] XZ = targetGenerator.getXZ (); //uses the spawn engines random positon generator to find a position on the map that is traversable
		wanderVector = new Vector3 (XZ [0], -10.0f, XZ [1]);

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
