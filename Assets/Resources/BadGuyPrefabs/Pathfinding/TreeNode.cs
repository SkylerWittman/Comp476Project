using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeNode {


	public float gCost;
	public float hCost;
	private TreeNode parent;
	private Vector3 position;
	private bool isTarget;
	private bool isStart;
	private List<TreeNode> neighbourNodes; 


	public TreeNode(Vector3 pos){ //creates a node with a position equal to the node gameObjects that are used to create the nodes, default the gcost to inf and all other costs to 0,
		position = pos;
		gCost = 0.0f;
		hCost = 0.0f;
		neighbourNodes = new List<TreeNode> ();
	}

	public float fCost {
		get {return gCost + hCost;}
	}

	public Vector3 getPosition(){
		return position;
	}

	public void AddToNeighbourNodes(TreeNode node){
		neighbourNodes.Add (node);
	}

	public List<TreeNode> GetNeighbourNodes(){
		return neighbourNodes;
	}


	public void SetParent(TreeNode parentNode){

		this.parent = parentNode;
	}

	public TreeNode getParent(){
		return parent;
	}


}