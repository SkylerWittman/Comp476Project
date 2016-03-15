using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	//empty game object used to find tree position
	public GameObject treeMarker;
	private TreeInstance[] arrayOfTrees;
	private GameObject[] treeGameObjects;
	private Vector3 tempPosition;

	void Start () {

		Debug.Log ("Creating");
		StartCoroutine (runTree ());
	}


	IEnumerator runTree(){
		yield return new WaitForSeconds (4);
		arrayOfTrees = Terrain.activeTerrain.terrainData.treeInstances;
		Debug.Log (arrayOfTrees.Length);
		Debug.Log ("Creating");

		foreach (TreeInstance tree in arrayOfTrees) {

			tempPosition = Vector3.Scale (tree.position, Terrain.activeTerrain.terrainData.size) + Terrain.activeTerrain.transform.position;
			Instantiate (treeMarker, tempPosition, Quaternion.identity);
			Debug.Log ("Creating");
		}

		treeGameObjects = GameObject.FindGameObjectsWithTag ("Tree");

		foreach (GameObject tree in treeGameObjects) {
			Debug.Log (tree.transform.position);
		}

	}







	GameObject[] GetTreeMarkers(){

		return treeGameObjects;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
