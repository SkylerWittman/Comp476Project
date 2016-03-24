using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NodeGenerator : MonoBehaviour {

    public Node node;
    public Vector3 levelSize;
    public float radius;
    public Vector3 tileSize;
    public float tileSize_Density;
    public int x, z;
    public List<Node> nodeList = new List<Node>();
    public GameObject[] players;
    // Use this for initialization
    void Start()
    {
        levelSize = new Vector3(x, 0, z);
        tileSize = new Vector3(levelSize.x / tileSize_Density, 0, levelSize.z / tileSize_Density);
        radius = tileSize.x / 2;
        Scan();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Scan()
    {
        for (int i = 0; i < tileSize_Density; i++)
        {
            for (int j = 0; j < tileSize_Density; j++)
            {
                Vector3 center = new Vector3(j * tileSize.x + ((-levelSize.x / 2) + (tileSize.x / 2)), 6, (-i * tileSize.z) + (levelSize.z / 2 - tileSize.z / 2));
                //layerMask parameter has to be in binary
                if (Physics.OverlapSphere(center, /*radius*/0.50f, 1000).Length == 0)
                {
                    Node tempNode;
                    Instantiate(node, center, new Quaternion());
                    nodeList.Add((Node)Instantiate(node, center, new Quaternion()));
                    node.number = (i) + (j * 10);
                }
            }
        }
        for (int i = 0; i < nodeList.Count; i++)
        {
            GenerateNeighbors(nodeList[i]);
        }
    }
    void GenerateNeighbors(Node node)
    {
        node.neighborList[(int)Node.NeighborNodes.UpLeft] = nodeList.Find(n => (n.transform.position - node.transform.position) == (new Vector3(-tileSize.x, 0, tileSize.z)));
        node.neighborList[(int)Node.NeighborNodes.Up] = nodeList.Find(n => (n.transform.position - node.transform.position) == (new Vector3(0, 0, tileSize.z)));
        node.neighborList[(int)Node.NeighborNodes.UpRight] = nodeList.Find(n => (n.transform.position - node.transform.position) == (new Vector3(tileSize.x, 0, tileSize.z)));
        node.neighborList[(int)Node.NeighborNodes.Right] = nodeList.Find(n => (n.transform.position - node.transform.position) == (new Vector3(tileSize.x, 0, 0)));
        node.neighborList[(int)Node.NeighborNodes.DownRight] = nodeList.Find(n => (n.transform.position - node.transform.position) == (new Vector3(tileSize.x, 0, -tileSize.z)));
        node.neighborList[(int)Node.NeighborNodes.Down] = nodeList.Find(n => (n.transform.position - node.transform.position) == (new Vector3(0, 0, -tileSize.z)));
        node.neighborList[(int)Node.NeighborNodes.DownLeft] = nodeList.Find(n => (n.transform.position - node.transform.position) == (new Vector3(-tileSize.x, 0, -tileSize.z)));
        node.neighborList[(int)Node.NeighborNodes.Left] = nodeList.Find(n => (n.transform.position - node.transform.position) == (new Vector3(-tileSize.x, 0, 0)));
    }
}
