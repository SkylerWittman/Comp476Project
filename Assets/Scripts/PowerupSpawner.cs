using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour {

    private GameObject explosivePowerup;
    private GameObject poisonPowerup;
    private GameObject healthPowerup;

	void Start () {
        //RNG initialization
        Random.seed = (int)System.DateTime.Now.Ticks;
        
        //Powerup initialization
        explosivePowerup = Resources.Load("PowerupPrefabs/ExplosivePowerup") as GameObject;
        poisonPowerup = Resources.Load("PowerupPrefabs/PoisonPowerup") as GameObject;
        healthPowerup = Resources.Load("PowerupPrefabs/HealthPowerup") as GameObject;
	}

    public void trySpawn(Vector3 spawnPos)
    {
        StartCoroutine(spawnRandomPowerup(spawnPos));
    }

    private IEnumerator spawnRandomPowerup(Vector3 spawnPosition) 
    {
        yield return new WaitForSeconds(3.0f);
        Vector3 spawnPos = new Vector3(spawnPosition.x, spawnPosition.y + 8.0f, spawnPosition.z);
        int chance = Random.Range(0, 5);

        if (chance == 0 || chance == 1)
        {
            Instantiate(poisonPowerup, spawnPos, Quaternion.identity);
        }
        else if (chance == 2 || chance == 3)
        {
            Instantiate(healthPowerup, spawnPos, Quaternion.identity);
        }
        else
        {
            Instantiate(explosivePowerup, spawnPos, Quaternion.identity);
        }
    }
}
