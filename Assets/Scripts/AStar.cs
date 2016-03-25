using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

    /*
     * AStar is used strictly for pathfinding and uses the reducedGrid from the Grid class only
     * It is a utility class that provides data to the user about which path to follow to get to their goal
     * It contains data about:
     * reducedGrid, a Grid object which it uses to compute its path
     * path, the computed path
     */

    private Grid reducedGrid;
    private List<Node> path;

    void Start()
    {
        reducedGrid = GetComponent<Grid>();
        path = new List<Node>();
    }

    //Returns a list to the user representing the path of nodes to get from start position to end position
    public List<Node> getPath(Vector3 startPos, Vector3 endPos)
    {
        FindPath(startPos, endPos);
        return path;
    }

    //The actual A* algorithm
    private void FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = reducedGrid.NodeFromPoint(startPos);
        Node endNode = reducedGrid.NodeFromPoint(endPos);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].f < currentNode.f || openList[i].f == currentNode.f && openList[i].h < currentNode.h)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                path = tracePath(startNode, endNode);
                return;
            }

            foreach (Node neighbour in reducedGrid.getNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                int newMoveCost = currentNode.g + getDistance(currentNode, neighbour);

                if (newMoveCost < neighbour.g || !openList.Contains(neighbour))
                {
                    neighbour.g = newMoveCost;
                    neighbour.h = getDistance(neighbour, endNode);

                    neighbour.parent = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
    }

    //Traces path from end node to start node
    private List<Node> tracePath(Node s, Node e)
    {
        List<Node> path = new List<Node>();
        Node currentNode = e;

        while (currentNode != s)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        return path;
    }

    //Used in evaluation of the heuristic
    private int getDistance(Node node1, Node node2)
    {
        int distX = Mathf.Abs(node1.gridX - node2.gridX);
        int distY = Mathf.Abs(node1.gridY - node2.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
