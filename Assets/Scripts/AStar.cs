using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour {

    public Grid grid;
    private List<Node> path;

    void Start()
    {
        grid = GetComponent<Grid>();
        path = new List<Node>();
    }

    public List<Node> getPath(Vector3 startPos, Vector3 endPos)
    {
        FindPath(startPos, endPos);
        return path;
    }

    public void FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.NodeFromPoint(startPos);
        Node endNode = grid.NodeFromPoint(endPos);

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

            foreach (Node neighbour in grid.getNeighbours(currentNode))
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
