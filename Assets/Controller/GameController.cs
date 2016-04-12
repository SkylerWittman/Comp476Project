//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class GameController : MonoBehaviour {


//		//empty game object used to find tree position
//		public GameObject treeMarker;
//		private TreeInstance[] arrayOfTrees;
//		private GameObject[] treeGameObjects;
//		private List<TreeNode> listOfTreeNodes = new List<TreeNode>();
//		private Vector3 tempPosition;
//		private bool canSlowTime = true;
//		public bool cursorVisible = true;
//		private Camera theCamera;

//		void Start()
//		{

			
//			Terrain.activeTerrain.tag = "Terrain";

//			Cursor.visible = cursorVisible;
//			Cursor.lockState = CursorLockMode.Locked;

//			theCamera = Camera.main;
//			StartCoroutine(runTree());
//		}


//		IEnumerator runTree()
//		{ //this method is called after 2 seconds (once all trees have been painted onto terrain) it then goes through each tree and finds the correct position
//			//of the tree by scaling the position to the size and orgin of the terrain, and empty game object is then instaniated at that position to be used as a marker for the tree
//			yield return new WaitForSeconds(3);
//			arrayOfTrees = Terrain.activeTerrain.terrainData.treeInstances;

//			foreach (TreeInstance tree in arrayOfTrees)
//			{
//				tempPosition = Vector3.Scale(tree.position, Terrain.activeTerrain.terrainData.size) + Terrain.activeTerrain.transform.position;
//				Instantiate(treeMarker, tempPosition, Quaternion.identity);
//			}

//			//the empty game object for the tree marker is tagged with tree we get an array of all the gameobjects to be used for pathfinding
//			treeGameObjects = GameObject.FindGameObjectsWithTag("TreeMarker");

			
//			foreach (GameObject tree in treeGameObjects) {
//				TreeNode node = new TreeNode(tree.transform.position);
//				listOfTreeNodes.Add (node);
//			}

//		FindVisibleTrees ();

//		}


//	public void ResetGCostOfNeighbours(){
//		foreach (TreeNode nodeA in listOfTreeNodes) {
//			foreach (TreeNode nodeB in nodeA.GetNeighbourNodes()) {
//				nodeB.gCost = 0;
//			}
//		}
//	}


//	private void FindVisibleTrees(){
//		foreach (TreeNode nodeA in listOfTreeNodes) {
//			foreach (TreeNode nodeB in listOfTreeNodes) {
				
//				if (nodeA == nodeB) {
//					continue;
//				}

//				if (Vector3.Distance (nodeA.getPosition (), nodeB.getPosition ()) < 300.0f) {
//					nodeA.AddToNeighbourNodes (nodeB);

//				}


//			}
//		}

//	}

//		public List<TreeNode> GetTreeNodes()
//		{

//			return listOfTreeNodes;
//		}


//		IEnumerator SlowTime()
//		{
//			Time.timeScale = 0.4f;
//			canSlowTime = false;
//			yield return new WaitForSeconds(3.0f);
//			Time.timeScale = 1.0f;
//			yield return new WaitForSeconds(10.0f);
//			canSlowTime = true;
//		}

//		private void ForcePower()
//		{

//			Ray ray = theCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
//			RaycastHit hit;
//			if (Physics.Raycast(ray, out hit, 30))
//			{
//				if (hit.transform.tag == "BadGuy" || hit.transform.tag == "SwarmSpider") {
//					Debug.Log ("im looking at " + hit.transform.name);
//					hit.rigidbody.velocity = Vector3.zero;
//					hit.rigidbody.AddForce (Vector3.up * 100, ForceMode.VelocityChange);
//				}
//			}
//		}

//		private void toggleCursor()
//		{
//			cursorVisible = !cursorVisible;
//			Cursor.visible = cursorVisible;
//			Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
//		}

//		// Update is called once per frame
//		void Update()
//		{

//		if ((Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("SlowTime")) && canSlowTime)
//			{
//				StartCoroutine(SlowTime());
//			}

//			if (Input.GetButton("Fire2"))
//			{
//				ForcePower();
//			}

//			if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("RightAnalogClick"))
//			{
//				toggleCursor();
//			}
//		}

//}
