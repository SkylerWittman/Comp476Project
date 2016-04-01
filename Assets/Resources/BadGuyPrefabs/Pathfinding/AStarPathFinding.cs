using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathFinding : MonoBehaviour {



	private TreeNode currentNode;
	private TreeNode targetNode;
	private TreeNode startNode;


	public List<TreeNode> FindPathEuclidean(TreeNode start, TreeNode end){ 

		List<TreeNode> openList = new List<TreeNode>();
		HashSet<TreeNode> closedList = new HashSet<TreeNode>();
		List<TreeNode> finalPath = new List<TreeNode>();


		startNode = start;
		targetNode = end;

		Debug.Log ("cost of start node" + start.gCost);

		openList.Add (startNode);

		while (openList.Count > 0) {
			currentNode = openList[0];

			for(int i = 1; i < openList.Count; i++){

				if(openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost){
					currentNode = openList[i];

				}
			}

			Debug.Log ("cost of current node" + currentNode.gCost);

			openList.Remove(currentNode);
			closedList.Add(currentNode);


			if(currentNode == targetNode){
				finalPath = RetracePath(startNode, targetNode);
				Debug.Log ("PathFound");
				return finalPath;

			}

			foreach(TreeNode neighbour in currentNode.GetNeighbourNodes() ){

				if(closedList.Contains(neighbour)){
					continue;
				}


				float movementCostToVisibleNode = currentNode.gCost + GetGCost(currentNode, neighbour);

				if(movementCostToVisibleNode < neighbour.gCost || !openList.Contains(neighbour)){
					neighbour.gCost = movementCostToVisibleNode;
					neighbour.hCost = GetHCost(neighbour);
					neighbour.SetParent(currentNode);
					openList.Add(neighbour);


				}

			}

		}

		return null;
	}


	private List<TreeNode> RetracePath(TreeNode startNode, TreeNode endNode){ //this method retraces the nodes from target node to startNode by looking at the parents, adds nodes to a path and reverses it to get the path in order

		List<TreeNode> path = new List<TreeNode>();
		TreeNode currentNode = endNode;

		while (currentNode != startNode) {

			path.Add (currentNode);
			currentNode = currentNode.getParent();

		}

		path.Reverse ();
		return path;
	}



	private float GetGCost(TreeNode nodeA, TreeNode nodeB){
		return Vector3.Distance (nodeA.getPosition (), nodeB.getPosition ());

	}


	private float GetHCost(TreeNode nodeA){
		return Vector3.Distance (nodeA.getPosition (), targetNode.getPosition ());

	}

}

