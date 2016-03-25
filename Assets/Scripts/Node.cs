using UnityEngine;
using System.Collections;

public class Node {

    public bool walkable;
    public Vector3 position;
    public int gridX, gridY;

    public int g, h;
    public Node parent;

    public Node(bool w, Vector3 pos, int x, int y)
    {
        walkable = w;
        position = pos;
        gridX = x;
        gridY = y;
    }

    public int f { get { return g + h; } }

    public Quaternion rotation
    {
        get
        {
            Quaternion rotation = Quaternion.LookRotation(position);
            return rotation;
        }
    }
}
