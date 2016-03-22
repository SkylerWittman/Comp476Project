using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	//empty game object used to find tree position
	public GameObject treeMarker;
	private TreeInstance[] arrayOfTrees;
	private GameObject[] treeGameObjects;
	private Vector3 tempPosition;

	void Start () {
		runTreeMapper ();
	}


	 //this method  goes through each tree and finds the correct position
	//of the tree by scaling the position to the size and orgin of the terrain, and empty game object is then instaniated at that position to be used as a marker for the tree
	void runTreeMapper(){
		arrayOfTrees = Terrain.activeTerrain.terrainData.treeInstances;

		foreach (TreeInstance tree in arrayOfTrees) {
			tempPosition = Vector3.Scale (tree.position, Terrain.activeTerrain.terrainData.size) + Terrain.activeTerrain.transform.position;
			Instantiate (treeMarker, tempPosition, Quaternion.identity);
		}

		//the empty game object for the tree marker is tagged with tree we get an array of all the gameobjects to be used for pathfinding
		//treeGameObjects = GameObject.FindGameObjectsWithTag ("Tree");
		GetComponent<NodeCreation> ().CreateNodeSystem (); //this method creates Node objects wit hpositions that correlate to each tree marker, these nodes are used for pathfinding
	}

	public GameObject[] GetTreeMarkers(){
		return treeGameObjects;
	}


}
