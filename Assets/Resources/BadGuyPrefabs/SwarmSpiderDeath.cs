using UnityEngine;
using System.Collections;

public class SwarmSpiderDeath : MonoBehaviour {

	bool LockA = false;
	private bool isDead = false;
	Animation anim;
	public AnimationClip die;
	private AudioSource audioSource;
	public AudioClip spiderDeath;
	private AudioClip hitSound;
	public float health;
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
		hitSound = Resources.Load("Sounds/GeneralSounds/Hit") as AudioClip;
	}

	// Update is called once per frame
	void Update()
	{

        if (LockA == true)
        {
            if (counter > 6)
            {
                health -= 10.0f;
                counter = 0;
            }
            counter++;
        }
        //Vector3 pos = transform.position;

        if (health < 0.0f && !isDead)
		{

			this.gameObject.tag = "SpiderDead";
			audioSource.PlayOneShot (spiderDeath, 3.0f);
			isDead = true;
			GetComponent<SwarmSpiderBehavior> ().SpiderDeath ();
			GetComponent<Collider> ().enabled = false;

			anim.Play(die.name);

            if (!triedSpawning)
            {
                powerupSpawner.trySpawn(transform.position);
                triedSpawning = true;
            }
			//Freeze all movement/rotations when NPC has been slain
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			GetComponent<SwarmSpiderBehavior> ().InformSwarmMembersOfDeath (this.gameObject);
            GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().addScore(spiderScore);
			GetComponent<SwarmSpiderBehavior> ().enabled = false;
			Destroy(this.gameObject, anim[die.name].length + 0.5f);

		}

	}
	void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
		case "RegularArrow":
            Destroy(other.gameObject);
			playHitSound();
			health -= other.GetComponent<Arrow>().damage;
			break;
		case "PoisonArrow":
			health -= other.GetComponent<Arrow>().damage;
            Destroy(other.gameObject);
			LockA = true;
			break;
		case "ExplosiveArrow":
			//Destroy(this.gameObject, anim[die.name].length + .5f);
           Destroy(other.gameObject);
           break;
        case "Gas":
            LockA = true;
           break;
        }
				
	}

	private void playHitSound()
	{
		audioSource.PlayOneShot(hitSound, 0.1f);
	}

}

