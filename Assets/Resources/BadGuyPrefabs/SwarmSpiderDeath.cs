using UnityEngine;
using System.Collections;

public class SwarmSpiderDeath : MonoBehaviour {

	bool LockA = false;
	private bool isDead = false;
	Animation anim;
	public AnimationClip die;
	private AudioSource audioSource;
	public AudioClip spiderDeath;
	public float health;
	public float damage;
    public int spiderScore;
    int counter = 0;
    private bool triedSpawning = false;
    private PowerupSpawner powerupSpawner;
	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animation>();
		anim[die.name].layer = 2;
		anim[die.name].speed = .7f;
		audioSource = GetComponent<AudioSource> ();
        powerupSpawner = GameObject.FindGameObjectWithTag("PowerupSpawner").GetComponent<PowerupSpawner>();

	}

	// Update is called once per frame
	void Update()
	{

		if (LockA == true)
		{
			if (counter < 100)
			{
				health -= 50.0f;
				++counter;
			}
			else
			{
				LockA = false;
				counter = 0;
			}
		}
		//Vector3 pos = transform.position;

		if(health < 0.0f && !isDead)
		{

			this.gameObject.tag = "SpiderDead";
			isDead = true;
			GetComponent<SwarmSpiderBehavior> ().SpiderDeath ();
			//audioSource.PlayOneShot (spiderDeath, .5f);
			anim.Play(die.name);
			//GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); ;
			//transform.position = pos;
            if (!triedSpawning)
            {
                powerupSpawner.trySpawn(transform.position);
                triedSpawning = true;
            }
			//Freeze all movement/rotations when NPC has been slain
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			GetComponent<SwarmSpiderBehavior> ().InformSwarmMembersOfDeath (this.gameObject);
            GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().addScore(spiderScore);
			Destroy(this.gameObject, anim[die.name].length + 1.0f);

		}

	}
	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
		case "RegularArrow":
			health -= other.GetComponent<Arrow>().damage;
			break;
		case "PoisonArrow":
			health -= other.GetComponent<Arrow>().damage;
			LockA = true;
			break;
		case "ExplosiveArrow":
			Destroy(this.gameObject, anim[die.name].length + .5f);
			break;
        case "Gas":
            LockA = true;
           break;
        }


		Destroy(other.gameObject);
	}

}

