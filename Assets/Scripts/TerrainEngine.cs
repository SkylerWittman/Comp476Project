using UnityEngine;
using System.Collections;

public class TerrainEngine : MonoBehaviour
{

    //Terrain vars
    public static Terrain terrain;
    private const float terrainSize = 2000.0f;
    private const float terrainHeight = 1500.0f;
    public static bool[,] terrainGrid = new bool[1000, 1000]; //Each index will indicate a 2x2 grid on the terrain, and will evaluate to true if occupied
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

    void Start()
    {
        //Grid initialization
        for (int x = 0; x < terrainGrid.GetLength(0); x++)
        {
            for (int z = 0; z < terrainGrid.GetLength(1); z++)
            {
                terrainGrid[x, z] = false;
            }
        }

        //Tree initialization
        trees[0] = Resources.Load("TreePrefabs/BroadLeafTree") as GameObject;
        trees[1] = Resources.Load("TreePrefabs/ConiferTree") as GameObject;

        for (int i = 0; i < numOfTreePrototypes; i++)
        {
            treePrototypes[i] = new TreePrototype();
            treePrototypes[i].prefab = trees[i];
        }

        //****************************//

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

        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = 5;
        terrainData.alphamapResolution = 3;
        terrainData.size = new Vector3(2000.0f, 1500.0f, 2000.0f);
        terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
        terrain.transform.parent = gameObject.transform;
        terrain.transform.position = new Vector3(0.0f, -75.0f, 0.0f);
        terrain.terrainData.splatPrototypes = splatPrototypes;
        terrain.terrainData.treePrototypes = treePrototypes;
        float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
            {
                float x = Random.Range(0.0f, 0.3f);
                float z = Random.Range(0.0f, 0.3f);
                heights[i, k] = Mathf.PerlinNoise(x, z) / 10.0f;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);

        TreeInstance[] tr = new TreeInstance[numOfTrees];
        for (int i = 0; i < numOfTrees; i++)
        {
            //Get a random starting x and z value
            float x = Random.value;
            float z = Random.value;

            //While the starting x and z values within the grid are occupied and/or under the water level, get new values
            while (terrainGrid[(int)(x*1000.0f), (int)(z*1000.0f)] == true || terrain.SampleHeight(new Vector3(x * 2000.0f, 0.0f, z * 2000.0f)) <= waterLevel)
            {
                x = Random.value;
                z = Random.value;
            }

            float height = terrain.terrainData.GetInterpolatedHeight(x, z);

            tr[i].position = new Vector3(x, (height / 1550.0f), z);
            terrainGrid[(int)(x*1000.0f), (int)(z*1000.0f)] = true;

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
            tr[i].widthScale = Random.Range(8f, 9f);
            tr[i].heightScale = Random.Range(7f, 9f);
        }

        terrain.terrainData.treeInstances = tr;
        terrain.treeDistance = 400.0f;
        terrain.treeBillboardDistance = 200.0f;
        terrain.treeCrossFadeLength = 20.0f;
        terrain.treeMaximumFullLODCount = 20;

        //This appears to be some sort of hack to get the colliders of the trees to work
        terrain.GetComponent<TerrainCollider>().enabled = false;
        terrain.GetComponent<TerrainCollider>().enabled = true;
    }
}