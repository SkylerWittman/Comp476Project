using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeCreation : MonoBehaviour {

	private Node currentNode;
	private Node targetNode;
	private Node startNode;
	private GameObject[] allNodes;
	public List<Node> allNodeObjects = new List<Node> ();
	public float maxNodeDistance = 400;


	public void CreateNodeSystem(){
		allNodes = GameObject.FindGameObjectsWithTag ("node");
		GameObjectsToNodes ();
		FindVisibleNodes ();

	}

	public List<Node> getList(){
		return allNodeObjects;
	}

	private void GameObjectsToNodes(){ // takes the node gameobjects placed on the map and converts them to a Node object
		foreach (GameObject gNode in allNodes) {
			Node newNode = new Node(gNode.transform.position);
			allNodeObjects.Add(newNode);

		}

	}

	private void FindVisibleNodes(){ // this method creates edges between the nodes...it checks every node on the map, seeing what nodes are visible to it, if visible they get added to the nodes visible list

		foreach (Node aNode in allNodeObjects) {
			foreach(Node bNode in allNodeObjects){

				if(Vector3.Distance(aNode.getPosition(), bNode.getPosition()) < maxNodeDistance){

					if(aNode == bNode){
						continue;
					}

					aNode.visibleNodes.Add(bNode);
				

				}
			}
		}

	}
}