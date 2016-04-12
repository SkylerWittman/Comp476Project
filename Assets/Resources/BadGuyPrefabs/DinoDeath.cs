using UnityEngine;
using System.Collections;

public class DinoDeath : MonoBehaviour {
	bool LockA = false;
	Animation anim;
	public AnimationClip die;
	public AnimationClip hit;
	public GameObject dinoCollider;
	public float health;
	public float damage;
	int counter = 0;
	public int deathScore = 10;
	private bool addedScore = false;

	private AudioSource audioSource;

	private AudioClip hitSound;

	private bool triedSpawning = false;
	private PowerupSpawner powerupSpawner;

	public bool isAlive;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animation>();
		anim[die.name].layer = 2;
		anim[die.name].speed = .7f;
		anim[hit.name].layer = 3;

		hitSound = Resources.Load("Sounds/GeneralSounds/Hit") as AudioClip;

		powerupSpawner = GameObject.FindGameObjectWithTag("PowerupSpawner").GetComponent<PowerupSpawner>();
		isAlive = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (LockA == true)
		{
			if (counter > 5)
			{
				health -= 10.0f;
				counter = 0;
			}
			counter++; 
		}
		Vector3 pos = transform.position;

		if(health <= 0.0f && isAlive)
		{
			isAlive = false;
			anim.Play(die.name);
			transform.position = pos;
			//Freeze all movement/rotations when NPC has been slain
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			dinoCollider.GetComponent<Collider> ().enabled = false;



			if (!triedSpawning)
			{
				powerupSpawner.trySpawn(pos);
				triedSpawning = true;
			}

			if (!addedScore)
			{
				GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().addScore(deathScore);
				addedScore = true;
			}


			Destroy(this.gameObject, anim[die.name].length+.5f);
		}

	}
	void OnTriggerEnter(Collider other)
	{
		DinoBehavior dinoBehave = GetComponent<DinoBehavior>();
		if (dinoBehave != null)
		{
			dinoBehave.gotHit();
		}


		switch (other.gameObject.tag)
		{
		case "RegularArrow":
			health -= other.GetComponent<Arrow> ().damage;
			anim.CrossFade(hit.name, 0.5f);
			playHitSound();
			Destroy(other.gameObject);
			break;
		case "PoisonArrow":
			health -= other.GetComponent<Arrow>().damage;
			anim.CrossFade(hit.name, 0.5f);
			LockA = true;
			break;
		case "ExplosiveArrow":
			anim.CrossFade(hit.name, 0.5f);
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