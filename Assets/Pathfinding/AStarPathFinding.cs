using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathFinding : MonoBehaviour {



	private Node currentNode;
	private Node targetNode;
	private Node startNode;
	private GameObject nodeSystem;

	void Start(){
		nodeSystem = GameObject.FindGameObjectWithTag ("controller");
	}




	 void resetNodeGCost(){ //used to reset costs of nodes between each path finding call
		foreach (Node n in nodeSystem.GetComponent<NodeCreation>().allNodeObjects) {
			n.gCost = Mathf.Infinity;
		}

	}



	public List<Node> FindPathEuclidean(Node start, Node end){ 

		List<Node> openList = new List<Node>();
		List<Node> closedList = new List<Node>();
		List<Node> finalPath = new List<Node>();

		resetNodeGCost ();

		startNode = start;
		targetNode = end;
		startNode.gCost = 0;


		openList.Add (startNode);

		while (openList.Count > 0) {
			currentNode = openList[0];

			for(int i = 1; i < openList.Count; i++){

				if(openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost){
					currentNode = openList[i];

				}
			}


			openList.Remove(currentNode);
			closedList.Add(currentNode);


			if(currentNode == targetNode){
				finalPath = RetracePath(startNode, targetNode);
				Debug.Log ("HI");
				return finalPath;
			}

			foreach(Node visibleNode in currentNode.visibleNodes ){

				if(closedList.Contains(visibleNode)){
					continue;
				}


				float movementCostToVisibleNode = currentNode.gCost + GetGCost(currentNode, visibleNode);

				if(movementCostToVisibleNode < visibleNode.gCost || !openList.Contains(visibleNode)){
					visibleNode.gCost = movementCostToVisibleNode;
					visibleNode.hCost = GetHCost(visibleNode);
					visibleNode.SetParent(currentNode);
					openList.Add(visibleNode);
					Debug.Log ("IM IN");

				}

			}

		}

		return null;
	}


	private List<Node> RetracePath(Node startNode, Node endNode){ //this method retraces the nodes from target node to startNode by looking at the parents, adds nodes to a path and reverses it to get the path in order

		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {

			path.Add (currentNode);
			currentNode = currentNode.getParent();

		}

		path.Reverse ();
		return path;
	}



	private float GetGCost(Node nodeA, Node nodeB){
		return Vector3.Distance (nodeA.getPosition (), nodeB.getPosition ());

	}


	private float GetHCost(Node nodeA){
		return Vector3.Distance (nodeA.getPosition (), targetNode.getPosition ());

	}

}

