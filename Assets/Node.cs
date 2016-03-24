using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

    public enum NeighborNodes { UpLeft = 0, Up = 1, UpRight = 2, Right = 3, DownRight = 4, Down = 5, DownLeft = 6, Left = 7 };
    public Node[] neighborList = new Node[8];
    public bool visible = false;
    public int number;
    public float g = 0;
    public float h = 0;
    public float t = 999;
    public Node prevNode;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
}
