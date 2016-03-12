using UnityEngine;
using System.Collections;

public class TerrainTest : MonoBehaviour {

    //Terrain vars
	private Terrain terrain;
    private const int numOfTerrainTextures = 1;
	private Texture2D[] terrainTextures = new Texture2D[numOfTerrainTextures];
	//Trees vars
    private const int numOfTrees = 2;
	private GameObject[] trees = new GameObject[numOfTrees];
	private TreePrototype[] treePrototypes = new TreePrototype[numOfTrees];

	void Awake() {
		//Terrain initialization
        terrainTextures[0] = Resources.Load ("TerrainTextures/GrassHillAlbedo") as Texture2D;
		
        //trees [0] = Resources.Load ("TreePrefabs/BroadLeafTree") as GameObject;
        //trees [1] = Resources.Load ("TreePrefabs/ConiferTree") as GameObject;
        //for (int i=0; i<numOfTrees; i++) {
        //    treePrototypes[i] = new TreePrototype();
        //    treePrototypes[i].prefab = trees[i];
        //}
		TerrainData terrainData = new TerrainData();
		terrainData.size = new Vector3(2000, 1500, 2000);
		terrain = Terrain.CreateTerrainGameObject (terrainData).GetComponent<Terrain>();
		float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
		
		for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
		{
			for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
			{
				int chance = Random.Range(0, 10);
				if(chance == 1) {
					float ranW = Random.Range(20.0f, 30.0f);
					float ranH = Random.Range(7.0f, 10.0f);
					heights[i, k] = Mathf.PerlinNoise(((float)i / (float)terrain.terrainData.heightmapWidth) * ranW, ((float)k / (float)terrain.terrainData.heightmapHeight) * ranH)/10.0f;
				}
				else {
					float ranW = Random.Range(5.0f, 10.0f);
					float ranH = Random.Range(0.0f, 3.0f);
					heights[i, k] = Mathf.PerlinNoise(((float)i / (float)terrain.terrainData.heightmapWidth) * ranW, ((float)k / (float)terrain.terrainData.heightmapHeight) * ranH)/10.0f;
				}
			}
		}
		terrain.terrainData.SetHeights(0, 0, heights);
		SplatPrototype[] splats = new SplatPrototype[1];
		splats [0] = new SplatPrototype ();
		splats [0].texture = terrainTextures[0];
		splats [0].tileSize = new Vector2 (50, 50);
		terrain.terrainData.splatPrototypes = splats;
        //terrain.terrainData.treePrototypes = treePrototypes;
		for (int i=0; i<(int)terrain.terrainData.heightmapWidth; i+=10) {
			for (int j=0; j<(int)terrain.terrainData.heightmapHeight; j+=10) {
				int chance = Random.Range (1, 3);
				if(chance == 1) {

				}
			}
		}
		terrain.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
	}
}