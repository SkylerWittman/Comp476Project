using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {


		public GameObject treeMarker;
		private TreeInstance[] arrayOfTrees;
		private GameObject[] treeGameObjects;
		private List<TreeNode> listOfTreeNodes = new List<TreeNode>();
		private Vector3 tempPosition;
		private bool canSlowTime = true;
		private bool canUseForce = true;
		private Camera theCamera;
		private float forceToApply = 2;	

		void Start()
		{
			Cursor.visible = false; //used to hide and lock cursor at middle of screen
			Cursor.lockState = CursorLockMode.Locked;
			Terrain.activeTerrain.tag = "Terrain";
			theCamera = Camera.main;
			StartCoroutine(runTree());
		}


		IEnumerator runTree()
		{ //this method is called after 3 seconds (once all trees have been painted onto terrain) it then goes through each tree and finds the correct position
			//of the tree by scaling the position to the size and orgin of the terrain, an empty game object is then instaniated at that position to be used as a marker for the tree
			yield return new WaitForSeconds(3);
			arrayOfTrees = Terrain.activeTerrain.terrainData.treeInstances; //gets trees on the map

			foreach (TreeInstance tree in arrayOfTrees)
			{
				tempPosition = Vector3.Scale(tree.position, Terrain.activeTerrain.terrainData.size) + Terrain.activeTerrain.transform.position;
				Instantiate(treeMarker, tempPosition, Quaternion.identity);
			}

			//the empty game object for the tree marker is tagged with tree we get an array of all the gameobjects to be used for pathfinding
			treeGameObjects = GameObject.FindGameObjectsWithTag("TreeMarker");

			
			foreach (GameObject tree in treeGameObjects) { //create tree nodes for pathfinding at each tree position on the map
				TreeNode node = new TreeNode(tree.transform.position);
				listOfTreeNodes.Add (node);
			}

		FindVisibleTrees ();

		}


	public void ResetGCostOfNeighbours(){
		foreach (TreeNode nodeA in listOfTreeNodes) {
			foreach (TreeNode nodeB in nodeA.GetNeighbourNodes()) {
				nodeB.gCost = 0;
			}
		}
	}


	private void FindVisibleTrees(){ //used for the LOS tree nodes, only trees that are within 300 units are visible to each other
		foreach (TreeNode nodeA in listOfTreeNodes) {
			foreach (TreeNode nodeB in listOfTreeNodes) {
				
				if (nodeA == nodeB) {
					continue;
				}

				if (Vector3.Distance (nodeA.getPosition (), nodeB.getPosition ()) < 300.0f) { 
					nodeA.AddToNeighbourNodes (nodeB);

				}
			}
		}
	}

		public List<TreeNode> GetTreeNodes()
		{
			return listOfTreeNodes;
		}


		IEnumerator SlowTime() //slow mo effect used in the game
		{
			Time.timeScale = 0.4f;
			canSlowTime = false;
			yield return new WaitForSeconds(3.0f);
			Time.timeScale = 1.0f;
			yield return new WaitForSeconds(10.0f);
			canSlowTime = true;
		}

		void ResetForceUse(){
			
			canUseForce = true;
		}

		private void ForcePower() //a feauture that we didnt have time to finish :(
		{

			Ray ray = theCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 30))
			{
				if (hit.transform.tag == "BadGuy" || hit.transform.tag == "SwarmSpider") {
					Debug.Log ("im looking at " + hit.transform.name);
					hit.rigidbody.velocity = Vector3.zero;
					canUseForce = false;
					//Instantiate (auraEffect, hit.transform.position, Quaternion.identity);
					
				for (int i = 0; i < 10000; i++) {
						if (i % 1000 == 0) {
							forceToApply += 2;
						}

						hit.rigidbody.AddForce (Vector3.up * forceToApply, ForceMode.Force);
					}
				}
			}
		}
		

		// Update is called once per frame
		void Update()
		{

		if (Input.GetKeyDown (KeyCode.F) && canSlowTime) {
			StartCoroutine (SlowTime ());

		}
			
		}
	}
