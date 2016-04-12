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
        int chance = Random.Range(0, 9);
        if (chance < 5)
        {
            StartCoroutine(spawnRandomPowerup(spawnPos));
        }
    }

    private IEnumerator spawnRandomPowerup(Vector3 spawnPosition) 
    {
        yield return new WaitForSeconds(3.0f);
        Vector3 spawnPos = new Vector3(spawnPosition.x, spawnPosition.y + 8.0f, spawnPosition.z);
        int chance = Random.Range(0, 5);

		GameObject newObj;
        if (chance == 0 || chance == 1)
        {
			newObj = Instantiate(poisonPowerup, spawnPos, Quaternion.identity) as GameObject;
        }
        else if (chance == 2 || chance == 3)
        {
			newObj = Instantiate(healthPowerup, spawnPos, Quaternion.identity) as GameObject;
        }
        else
        {
			newObj = Instantiate(explosivePowerup, spawnPos, Quaternion.identity) as GameObject;
        }

		yield return new WaitForSeconds (1.0f);
		newObj.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;

    }
}
