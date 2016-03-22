using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {


	public float gCost;
	public float hCost;
	private Node parent;
	private Vector3 position;
	private bool isTarget;
	private bool isStart;
	public List<Node> visibleNodes; 
	public string roomName;

	public Node(Vector3 pos){ //creates a node with a position equal to the node gameObjects that are used to create the nodes, default the gcost to inf and all other costs to 0,
		position = pos;
		gCost = Mathf.Infinity;
		hCost = 0.0f;
		visibleNodes = new List<Node> ();
	}

	public float fCost {
		get {return gCost + hCost;}
	}

	public Vector3 getPosition(){
		return position;
	}

	public void SetParent(Node parentNode){

		this.parent = parentNode;
	}

	public Node getParent(){
		return parent;
	}


}