using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

    /*
     * Grid class is used for bad guy and tree spawning as well as pathfinding.
     * There are two types of grids:
     * grid is a 500 x 500 grid that contains information about every 2 units of the terrain. It is used strictly for bad guy and tree spawning.
     * reducedGrid is a 50 x 50 grid that contains information about every 20 units of the terrain. It is uses strictly for path finding.
     */

    private int gridSize;   
    public Node[,] grid; 
    private Node[,] reducedGrid; 

    public GameObject marker;

    void Awake()
    {
        gridSize = 500;
        grid = new Node[gridSize, gridSize];
        initializeGrid();
        reducedGrid = getReducedGrid();
    }

    /*
     * UNCOMMENT THE START IF YOU WANT TO SEE THE NODES GENERATED
     */

    void Start()
    {
        //RNG initialization
        Random.seed = (int)System.DateTime.Now.Ticks;

        //for (int i = 0; i < reducedGrid.GetLength(0); i++)
        //{
        //    for (int j = 0; j < reducedGrid.GetLength(1); j++)
        //    {
        //        Node tempNode = reducedGrid[i, j];
        //        GameObject m = Instantiate(marker, reducedGrid[i, j].position, Quaternion.identity) as GameObject;
        //        Renderer r = m.GetComponent<Renderer>();
        //        if (tempNode.walkable)
        //        {
        //            r.material.color = Color.green;
        //        }
        //        else
        //        {
        //            r.material.color = Color.red;
        //        }
        //    }
        //}
    }

    //Initializes grid, not reducedGrid
    private void initializeGrid()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int z = 0; z < grid.GetLength(1); z++)
            {
                //Just create the nodes, but doesn't have any data yet about the terrain
                grid[x, z] = new Node(true, new Vector3(x * 2.0f, 0.0f, z * 2.0f), x, z);
            }
        }
    }

    //Finds the closest node in reducedGrid from a given position 
    //Only usable with reducedGrid
    public Node NodeFromPoint(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x / 20.0f);
        int z = Mathf.RoundToInt(pos.z / 20.0f);
        x = Mathf.Clamp(x, 0, 49);
        z = Mathf.Clamp(z, 0, 49);

        return reducedGrid[x, z];
    }

    /*
     * Given a node, getNeighbours will find all other nodes in a 3 x 3 grid of the node
     * n n n
     * n N n
     * n n n
     * Where n's represent neighbours and N represents the node given
     * Only usable with reducedGrid
     */
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

    //Initialization for reducedGrid, not grid
    //Creates the reducedGrid, which is created from the data of every tenth node of the grid
    private Node[,] getReducedGrid()
    {
        int aTenth = gridSize / 10;
        Node[,] reducedGrid = new Node[aTenth, aTenth];

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

    public Node getRandomNodeCloseToPos(Vector3 pos)
    {
        Node node = NodeFromPoint(pos);
        int x = Random.Range(Mathf.Clamp(node.gridX - 5, 0, 49), Mathf.Clamp(node.gridX + 5, 0, 49));
        int y = Random.Range(Mathf.Clamp(node.gridY - 5, 0, 49), Mathf.Clamp(node.gridY + 5, 0, 49));
        return reducedGrid[x, y];
    }
}
