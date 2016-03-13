using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{

    //Terrain vars
    private Terrain terrain;
    private bool[,] terrainGrid;
    private float waterLevel;

    //Player vars
    private GameObject player;

    //Enemy vars
    private const int numOfZombies = 1;
    private const int numOfGoblins = 3;
    private const int numOfDinos = 3;
    private GameObject[] zombies;
    private GameObject[] goblins;
    private GameObject[] dinos;

    void Start()
    {
        //Terrain initialization
        terrain = TerrainEngine.terrain;
        terrainGrid = TerrainEngine.terrainGrid;
        waterLevel = TerrainEngine.waterLevel;

        //Player initialization
        player = Resources.Load("PlayerPrefabs/Player") as GameObject;
        spawnPlayer();

        //Enemy initialization
        zombies = new GameObject[numOfZombies];
        goblins = new GameObject[numOfGoblins];
        dinos = new GameObject[numOfDinos];
        zombies[0] = Resources.Load("BadGuyPrefabs/ZombieGrey") as GameObject;
        goblins[0] = Resources.Load("BadGuyPrefabs/GoblinGreen") as GameObject;
        goblins[1] = Resources.Load("BadGuyPrefabs/GoblinBlue") as GameObject;
        goblins[2] = Resources.Load("BadGuyPrefabs/GoblinRed") as GameObject;
        dinos[0] = Resources.Load("BadGuyPrefabs/DinoGreen") as GameObject;
        dinos[1] = Resources.Load("BadGuyPrefabs/DinoGrey") as GameObject;
        dinos[2] = Resources.Load("BadGuyPrefabs/DinoRed") as GameObject;
        spawnEnemies(500);
    }

    private void spawnPlayer()
    {
        float x = Random.Range(0.0f, 2000.0f);
        float z = Random.Range(0.0f, 2000.0f);

        while (terrainGrid[(int)(x / 2.0f), (int)(z / 2.0f)] == true || terrain.SampleHeight(new Vector3(x, 0.0f, z)) <= waterLevel)
        {
            x = Random.Range(0.0f, 2000.0f);
            z = Random.Range(0.0f, 2000.0f);
        }

        Vector3 playerStartPosition = new Vector3(x, 10.0f, z);
        Instantiate(player, playerStartPosition, Quaternion.identity);
    }

    private void spawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float x = Random.Range(0.0f, 2000.0f);
            float z = Random.Range(0.0f, 2000.0f);

            while (terrainGrid[(int)(x / 2.0f), (int)(z / 2.0f)] == true || terrain.SampleHeight(new Vector3(x, 0.0f, z)) <= waterLevel)
            {
                x = Random.Range(0.0f, 2000.0f);
                z = Random.Range(0.0f, 2000.0f);
            }
            
            Vector3 spawnPosition = new Vector3(x, 10.0f, z);

            int typeOfEnemy = Random.Range(0, 3);
            switch (typeOfEnemy)
            {
                case 0:
                    int zombieColor = Random.Range(0, numOfZombies - 1);
                    Instantiate(zombies[zombieColor], spawnPosition, Quaternion.identity);
                    break;
                case 1:
                    int goblinColor = Random.Range(0, numOfGoblins);
                    Instantiate(goblins[goblinColor], spawnPosition, Quaternion.identity);
                    break;
                case 2:
                    int dinoColor = Random.Range(0, numOfDinos);
                    Instantiate(dinos[dinoColor], spawnPosition, Quaternion.identity);
                    break;
                default:
                    Debug.Log("???");
                    break;
            }
        }
    }
}
