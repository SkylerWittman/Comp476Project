using UnityEngine;
using System.Collections;

public class Node {

    /*
     * Nodes are used both for spawning and pathfinding
     * They contain data represents:
     * walkable, whether the position on the terrain is occupied or underwater
     * position, the position in the game they are located
     * gridX, gridY, the indices of the node in their respective grid (Nodes are used both in grid and reducedGrid)
     * g, h, f, the values of the heuristic computed in AStar
     * parent, used to retrace a path when pathfinding
     */

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

}
