﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	//empty game object used to find tree position
	public GameObject treeMarker;
	private TreeInstance[] arrayOfTrees;
	private GameObject[] treeGameObjects;
	private Vector3 tempPosition;
    public bool cursorVisible = true;

	void Start () {

		Debug.Log ("Creating");
        
        //Comment this if you don't like how the cursor appears
        Cursor.visible = cursorVisible;
       // Cursor.lockState = CursorLockMode.Locked;
		
        StartCoroutine (runTree ());
	}


	IEnumerator runTree(){ //this method is called after 2 seconds (once all trees have been painted onto terrain) it then goes through each tree and finds the correct position
		//of the tree by scaling the position to the size and orgin of the terrain, and empty game object is then instaniated at that position to be used as a marker for the tree
		yield return new WaitForSeconds (2);
		arrayOfTrees = Terrain.activeTerrain.terrainData.treeInstances;

		foreach (TreeInstance tree in arrayOfTrees) {
			tempPosition = Vector3.Scale (tree.position, Terrain.activeTerrain.terrainData.size) + Terrain.activeTerrain.transform.position;
			Instantiate (treeMarker, tempPosition, Quaternion.identity);
		}

		//the empty game object for the tree marker is tagged with tree we get an array of all the gameobjects to be used for pathfinding
		treeGameObjects = GameObject.FindGameObjectsWithTag ("Tree");

	}



	public GameObject[] GetTreeMarkers(){

		return treeGameObjects;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
