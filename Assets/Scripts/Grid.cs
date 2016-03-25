using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    private int gridSize;
    public Node[,] grid;
    private Node[,] reducedGrid;

    void Awake()
    {
        gridSize = 500;
        grid = new Node[gridSize, gridSize];
        initializeGrid();
        reducedGrid = getReducedGrid();
    }

    private void initializeGrid()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int z = 0; z < grid.GetLength(1); z++)
            {
                grid[x, z] = new Node(true, new Vector3(x * 2.0f, 0.0f, z * 2.0f), x, z);
            }
        }
    }

    public Node NodeFromPoint(Vector3 pos)
    {
        //float percentX = (pos.x * 2.0f + gridSize / 2) / gridSize;
        //float percentY = (pos.z * 2.0f + gridSize / 2) / gridSize;

        //percentX = Mathf.Clamp01(percentX);
        //percentY = Mathf.Clamp01(percentY);

        //int x = Mathf.RoundToInt((gridSize - 1) * percentX);
        //int y = Mathf.RoundToInt((gridSize - 1) * percentY);
        int x = Mathf.RoundToInt(pos.x / 20.0f);
        int z = Mathf.RoundToInt(pos.z / 20.0f);
        x = x > 49 ? 49 : x; x = x < 0 ? 0 : x;
        z = z > 49 ? 49 : z; z = z < 0 ? 0 : z;

        return reducedGrid[x, z];
    }

    public List<Node> getNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 && y != 0)
                {
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSize / 10 && checkY >= 0 && checkY < gridSize / 10)
                    {
                        neighbours.Add(reducedGrid[checkX, checkY]);
                    }
                }
            }
        }
        return neighbours;
    }

    public Node[,] getReducedGrid()
    {
        Node[,] reducedGrid = new Node[50, 50];

        for (int x = 0; x < grid.GetLength(0); x+=10)
        {
            for (int z = 0; z < grid.GetLength(1); z+=10)
            {
                Node tempNode = grid[x, z];
                tempNode.gridX = x / 10; tempNode.gridY = z / 10;
                reducedGrid[x / 10, z / 10] = tempNode;
            }
        }

        return reducedGrid;
    }

    //private LayerMask unwalkable;
    //private Vector2 gridSize;
    //private float nodeRadius;
    //public Node[,] grid;

    //private float nodeDiameter;
    //public int gridSizeX, gridSizeY;

    //void Start()
    //{
    //    unwalkable = LayerMask.GetMask("Unwalkable");
    //    gridSize = new Vector2(4300.0f, 3300.0f);
    //    nodeRadius = 60.0f;
    //    nodeDiameter = nodeRadius * 2.0f;
    //    gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
    //    gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
    //    initializeGrid();
    //}

    //void initializeGrid()
    //{
    //    grid = new Node[gridSizeX, gridSizeY];
    //    Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;


    //    for (int x = 0; x < gridSizeX; x++)
    //    {
    //        for (int y = 0; y < gridSizeY; y++)
    //        {
    //            Vector3 point = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
    //            point.y = 50.0f;
    //            bool canWalk = !(Physics.CheckSphere(point, nodeRadius, unwalkable));
    //            grid[x, y] = new Node(canWalk, point, x, y);
    //        }
    //    }
    //}

    //public Node NodeFromPoint(Vector3 pos)
    //{
    //    float percentX = (pos.x + gridSize.x / 2) / gridSize.x;
    //    float percentY = (pos.z + gridSize.y / 2) / gridSize.y;

    //    percentX = Mathf.Clamp01(percentX);
    //    percentY = Mathf.Clamp01(percentY);

    //    int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
    //    int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

    //    return grid[x, y];
    //}

    //public List<Node> getNeighbours(Node node)
    //{
    //    List<Node> neighbours = new List<Node>();

    //    for (int x = -1; x <= 1; x++)
    //    {
    //        for (int y = -1; y <= 1; y++)
    //        {
    //            if (x != 0 && y != 0)
    //            {
    //                int checkX = node.gridX + x;
    //                int checkY = node.gridY + y;

    //                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
    //                {
    //                    neighbours.Add(grid[checkX, checkY]);
    //                }
    //            }
    //        }
    //    }
    //    return neighbours;
    //}
}
