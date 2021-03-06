﻿using UnityEngine;
using System.Collections;

public class TerrainEngine : MonoBehaviour
{
    //Terrain vars
    public static Terrain terrain;
    public const float terrainSize = 1000.0f;
    public float terrainHeight = 1500.0f;
    public const int gridSize = (int)(terrainSize / 2);
    public static Grid terrainGrid; //This grid is referring to grid in the Grid class (the 500 x 500 grid used for spawning)
    public static float waterLevel = 75.0f;  

    //Tree vars
    private const int numOfTrees = 200;
    private const int numOfTreePrototypes = 2;
    private GameObject[] trees = new GameObject[numOfTreePrototypes];
    private TreePrototype[] treePrototypes = new TreePrototype[numOfTreePrototypes];

    //Splat vars
    private const int numOfSplats = 2;
    private SplatPrototype[] splatPrototypes = new SplatPrototype[numOfSplats];
    private Texture2D[] terrainTextures = new Texture2D[numOfSplats];
    private const float terrainTile0 = 25.0f;
    private const float terrainTile1 = 30.0f;

    void Awake()
    {
        terrainGrid = GetComponent<Grid>();
    }

    void Start()
    {

        //RNG initialization
        Random.seed = (int)System.DateTime.Now.Ticks;
        
        //Tree initialization
        trees[0] = Resources.Load("TreePrefabs/BroadLeafTree") as GameObject;
        trees[1] = Resources.Load("TreePrefabs/ConiferTree") as GameObject;


        for (int i = 0; i < numOfTreePrototypes; i++)
        {
            treePrototypes[i] = new TreePrototype();
            treePrototypes[i].prefab = trees[i];
			treePrototypes [i].prefab.tag = "Tree";
        }

        //Splat initialization
        terrainTextures[0] = Resources.Load("TerrainTextures/GrassHillAlbedo") as Texture2D;
        terrainTextures[1] = Resources.Load("TerrainTextures/MudRockyAlbedoSpecular") as Texture2D;

        Vector2[] splatTileSize = new Vector2[numOfSplats] { 
            new Vector2(terrainTile0, terrainTile0), new Vector2(terrainTile1, terrainTile1) 
        };

        for (int i = 0; i < numOfSplats; i++)
        {
            splatPrototypes[i] = new SplatPrototype();
        }

        for (int i = 0; i < numOfSplats; i++)
        {
            splatPrototypes[i].texture = terrainTextures[i];
            splatPrototypes[i].tileOffset = Vector2.zero;
            splatPrototypes[i].tileSize = splatTileSize[i];
        }

        //Terrain initialization
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = 5;
        terrainData.alphamapResolution = 3;
        terrainData.size = new Vector3(terrainSize, terrainHeight, terrainSize);
        terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
        terrain.transform.parent = gameObject.transform;
        terrain.transform.position = new Vector3(0.0f, -waterLevel+2.0f, 0.0f);
        terrain.terrainData.splatPrototypes = splatPrototypes;
        terrain.terrainData.treePrototypes = treePrototypes;
        float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
            {
                float x = Random.Range(0.0f, 0.1f);
                float z = Random.Range(0.0f, 0.1f);
                heights[i, k] = Mathf.PerlinNoise(x, z) / 10.0f;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);

        //Tree instances initialization
        TreeInstance[] tr = new TreeInstance[numOfTrees];
        for (int i = 0; i < numOfTrees; i++)
        {
            //Get a random starting x and z value
            float x = Random.value;
            float z = Random.value;

            //While the starting x and z values within the grid are occupied and/or under the water level, get new values
            while (terrainGrid.grid[(int)(x*gridSize), (int)(z*gridSize)].walkable == false || terrain.SampleHeight(new Vector3(x * terrainSize, 0.0f, z * terrainSize)) < waterLevel)
            {
                x = Random.value;
                z = Random.value;
            }

            //Get the height of the terrain at the location, and place the tree there
            float height = terrain.terrainData.GetInterpolatedHeight(x, z);

            tr[i].position = new Vector3(x, (height / 1550.0f), z);

            /*
             * We'll mark on the grid where the position of the tree is as unwalkable. Additionally, the surrounding positions will be marked as unwalkable as well.
             * t t t
             * t T t
             * t t t
             * Where t's are the current tree's neighbouring positions and T is the position of the current tree
             */
            for (int X = (int)(x*gridSize) - 5; X <= (int)(x*gridSize) + 5; X++)
            {
                for (int Z = (int)(z*gridSize) - 5; Z <= (int)(z*gridSize) + 5; Z++)
                {
                    terrainGrid.grid[Mathf.Clamp(X, 0, gridSize-1), Mathf.Clamp(Z, 0, gridSize-1)].walkable = false;
                }
            }
            
            //Broad leafs trees have a probability of 2/3 of occuring
            //Conifer trees have a probability of 1/3 of occuring
            int chance = Random.Range(0, 3);
            if (chance == 0)
            {
                tr[i].prototypeIndex = 1;
            }
            else
            {
                tr[i].prototypeIndex = 0;
            }

            //These paramaters are used to vary the width and height of the tree
            tr[i].widthScale = Random.Range(3f, 4f);
            tr[i].heightScale = Random.Range(4f, 6f);
        }

        //Tell the terrain about the instances of trees we've created
        terrain.terrainData.treeInstances = tr;
        terrain.treeDistance = 400.0f;
        terrain.treeBillboardDistance = 200.0f;
        terrain.treeCrossFadeLength = 20.0f;
        terrain.treeMaximumFullLODCount = 20;

        //Setup positions in the terrain grid where the terrain is below water level
        for (int x = 0; x < terrainGrid.grid.GetLength(0); x++)
        {
            for (int z = 0; z < terrainGrid.grid.GetLength(1); z++)
            {
                if (terrainGrid.grid[x, z].walkable)
                {
                    terrainGrid.grid[x, z].walkable = terrain.SampleHeight(new Vector3(x*2.0f, 0.0f, z*2.0f)) >= waterLevel-2;
                }
                terrainGrid.grid[x, z].position.y = terrain.SampleHeight(new Vector3(x * 2.0f, 0.0f, z * 2.0f)) - waterLevel;
            }
        }

        //This appears to be some sort of hack to get the colliders of the trees to work
        terrain.GetComponent<TerrainCollider>().enabled = false;
        terrain.GetComponent<TerrainCollider>().enabled = true;
    }
}