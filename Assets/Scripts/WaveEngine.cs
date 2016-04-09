using UnityEngine;
using System.Collections;

public class WaveEngine : MonoBehaviour {

    //Terrain vars
    private Terrain terrain;
    private float terrainSize;
    private Grid terrainGrid;
    private float waterLevel;

    //Bad guy vars
    private const int numOfSpiders = 1;
    private const int numOfGoblins = 1;
    private const int numOfDinos = 3;
    private GameObject[] spiders;
    private GameObject[] goblins;
    private GameObject[] dinos;

    void Start()
    {
        //RNG initialization
        Random.seed = (int)System.DateTime.Now.Ticks;
        
        //Terrain initialization
        terrain = TerrainEngine.terrain;
        terrainSize = TerrainEngine.terrainSize;
        terrainGrid = TerrainEngine.terrainGrid;
        waterLevel = TerrainEngine.waterLevel;
        
        //Bad guy initialization
        spiders = new GameObject[numOfSpiders];
        goblins = new GameObject[numOfGoblins];
        dinos = new GameObject[numOfDinos];
        spiders[0] = Resources.Load("BadGuyPrefabs/Spider") as GameObject;
        goblins[0] = Resources.Load("BadGuyPrefabs/GoblinRed") as GameObject;
        dinos[0] = Resources.Load("BadGuyPrefabs/DinoGreen") as GameObject;
        dinos[1] = Resources.Load("BadGuyPrefabs/DinoGrey") as GameObject;
        dinos[2] = Resources.Load("BadGuyPrefabs/DinoRed") as GameObject;
    }

    public void spawnBadGuys(Wave wave)
    {
        int amount = wave.badGuyCount;
        for (int i = 0; i < amount; i++)
        {
            float[] XZ = getXZ();

            Vector3 spawnPosition = new Vector3(XZ[0], 10.0f, XZ[1]);

            int typeOfEnemy = Random.Range(0, 3);
            switch (typeOfEnemy)
            {
                case 0:
                    int goblinColor = Random.Range(0, numOfGoblins);
                    Instantiate(goblins[goblinColor], spawnPosition, Quaternion.identity);
                    break;
                case 1:
                    int dinoColor = Random.Range(0, numOfDinos);
                    GameObject dinoInstance = Instantiate(dinos[dinoColor], spawnPosition, Quaternion.identity) as GameObject;
                    float randSize = Random.Range(1.5f, 6.5f);
                    dinoInstance.transform.localScale = new Vector3(randSize, randSize, randSize);
                    dinoInstance.GetComponent<DinoBehavior>().maxSpeed = 11.0f - randSize;
                    dinoInstance.GetComponent<DinoBehavior>().maxRunSpeed = 16.0f - randSize;
                    dinoInstance.GetComponent<NPCDetail>().health = 50.0f * randSize;
                    dinoInstance.GetComponent<NPCDetail>().damage = 20.0f * randSize;
                    break;
                case 2:
                    int spiderType = Random.Range(0, numOfSpiders);
                    Instantiate(spiders[spiderType], spawnPosition, Quaternion.identity);
                    break;
                default:
                    Debug.Log("???");
                    break;
            }
        }
    }

    //Checks random x and z coordinates within the environment to see if they are valid spawning coordinates
    //Once it has found a valid spawning coordinate, it returns an array XZ of size 2
    //XZ[0] is the x coordinate
    //XZ[1] is the z coordinate
    public float[] getXZ()
    {
        float[] XZ = new float[2];
        float x = Random.Range(100.0f, terrainSize - 100.0f);
        float z = Random.Range(100.0f, terrainSize - 100.0f);
        Vector3 samplePosition = new Vector3(x, 0.0f, z);

        //Check whether the position in the grid is occupied by a tree, or under the water level or there is a bad guy/player within a radius of 1 of samplePosition
        while (terrainGrid.NodeFromPoint(samplePosition).walkable == false || terrain.SampleHeight(samplePosition) <= waterLevel || checkAroundPosition(samplePosition))
        {
            x = Random.Range(100.0f, terrainSize - 100.0f);
            z = Random.Range(100.0f, terrainSize - 100.0f);
            samplePosition.x = x;
            samplePosition.z = z;
        }

        XZ[0] = x;
        XZ[1] = z;

        return XZ;
    }

    //Checks to see if the position is close to a bad guy or player
    private bool checkAroundPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 40.0f);
        bool found = false;
        
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag.Equals("Player") || colliders[i].gameObject.tag.Equals("BadGuy") || colliders[i].gameObject.tag.Equals("Tree"))
            {
                found = true;
                break;
            }
        }
        
        return found;
    }
}
