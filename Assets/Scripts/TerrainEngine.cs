using UnityEngine;
using System.Collections;

public class TerrainEngine : MonoBehaviour {

    //Terrain vars
    private Terrain terrain;
    
    //Tree vars
    private const int numOfTreePrototypes = 2;
    private const int numOfTrees = 200;
    private GameObject[] trees = new GameObject[numOfTreePrototypes];
    private TreePrototype[] treePrototypes = new TreePrototype[numOfTreePrototypes];
    
    //Splat vars
    private const int numOfSplats = 2;
    private Texture2D[] terrainTextures = new Texture2D[numOfSplats];
    private SplatPrototype[] splatPrototypes = new SplatPrototype[numOfSplats];
    private float terrainTile0 = 25.0f;
    private float terrainTile1 = 30.0f;

    void Start()
    {
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
            //splatPrototypes[i].texture.Apply(true);
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

        //terrain.terrainData.RefreshPrototypes();

        //float[, ,] splatMap = new float[terrain.terrainData.alphamapResolution, terrain.terrainData.alphamapResolution, 2];

        //for (int x = 0; x < terrain.terrainData.alphamapHeight; x++)
        //{
        //    for (int z = 0; z < terrain.terrainData.alphamapWidth; z++)
        //    {
        //        float normalizedX = (float)x / (terrain.terrainData.alphamapWidth - 1);
        //        float normalizedZ = (float)z / (terrain.terrainData.alphamapHeight - 1);

        //        float steepness = terrain.terrainData.GetSteepness(normalizedX, normalizedZ);
        //        float steepnessNormalized = Mathf.Clamp(steepness/1.5f, 0.0f, 1.0f);

        //        splatMap[z, x, 0] = 1.0f - steepnessNormalized;
        //        splatMap[z, x, 1] = steepnessNormalized;
        //    }
        //}

        //terrainData.SetAlphamaps(0, 0, splatMap);

        TreeInstance[] tr = new TreeInstance[numOfTrees];
        for (int i = 0; i < numOfTrees; i++) {
            float x = Random.value;
            float y = Random.value;

            float angle = terrain.terrainData.GetSteepness(x, y);
            float height = terrain.terrainData.GetInterpolatedHeight(x, y);

            tr[i].position = new Vector3(x, (height / 1500.0f), y);
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

    //    TerrainData terrainData = new TerrainData();
    //    terrainData.size = new Vector3(2000, 1500, 2000);
    //    terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
    //    float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

    //    for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
    //    {
    //        for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
    //        {
    //            float ranW = Random.Range(5.0f, 10.0f);
    //            float ranH = Random.Range(0.0f, 3.0f);
    //            heights[i, k] = Mathf.PerlinNoise(((float)i / (float)terrain.terrainData.heightmapWidth) * ranW, ((float)k / (float)terrain.terrainData.heightmapHeight) * ranH) / 10.0f;
                
    //        }
    //    }
      
    //    terrain.terrainData.SetHeights(0, 0, heights);
    //    SplatPrototype[] splats = new SplatPrototype[1];
    //    splats[0] = new SplatPrototype();
    //    splats[0].texture = terrainTextures[0];
    //    splats[0].tileSize = new Vector2(50, 50);
    //    terrain.terrainData.splatPrototypes = splats;
    //    //terrain.terrainData.treePrototypes = treePrototypes;
    //    for (int i = 0; i < (int)terrain.terrainData.heightmapWidth; i += 10)
    //    {
    //        for (int j = 0; j < (int)terrain.terrainData.heightmapHeight; j += 10)
    //        {
    //            int chance = Random.Range(1, 3);
    //            if (chance == 1)
    //            {

    //            }
    //        }
    //    }
    //    terrain.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
    }
}