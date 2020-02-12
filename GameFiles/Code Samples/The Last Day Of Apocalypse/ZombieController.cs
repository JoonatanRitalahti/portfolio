using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine.AI;
using UnityEngine;
using Random = System.Random;

public class ZombieController : MonoBehaviour
{
	public GameStatus gameStatus;
	public GameObject blood;
	public float health;
	public Transform player;
	[HideInInspector]
	public Transform chaseTarget;
	[HideInInspector]
	public NavMeshAgent navMeshAgent;
	public double z = 1.5;
	public double zEnd = 1.5;
	public double x = 10;
	public double xEnd = 10;
	private bool sounds;
	private int origHealth;
	public AudioClip ZombieHitClip;
	public Transform plane;
	public GameObject smgDrop;
	public float damageToPlayer;
	public TextMesh healthText;
	Quaternion rotation;
	public AudioSource audioSource;
	private ParticleSystem bloodSplat;
	public GameObject pickupDrop;
	///<see cref="0=health20, 1=health50, 2=health100, 3=pistolammo12, 4=pistolammo36, 5=pistolammo108, 6=smgammo30, 7=smgammo90, 8=smgammo270, 9=smgammo810, 10=sprint30sec, 11=sprint180sec, 12=exp*3zomb, 13=exp*10 zomb"/>
	public int dropPickupType;

	public string lastDropped;
	// Use this for initialization
	void Start ()
	{
		bloodSplat = GetComponentInChildren<ParticleSystem>();
		plane = GameObject.Find("TerrainMain").GetComponent<Transform>();
		gameStatus = GameObject.Find("Manager").GetComponent<GameStatus>();
		health = gameStatus.zombieHealthMax;
		player = GameObject.Find("soldier").GetComponent<Transform>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		healthText.text ="Health: " + Mathf.Round(health);
		origHealth = Mathf.RoundToInt(health);
		audioSource = GetComponent<AudioSource>();
		if (navMeshAgent.speed >= 5)
		{
			sounds = true;
		}
	}
	
	void Awake()
	{
		healthText = GetComponentInChildren<TextMesh>();
		rotation = healthText.transform.rotation;
		xEnd = RandomPickup();
		x = xEnd;
	}
	void LateUpdate()
	{
		healthText.transform.rotation = rotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		Chase();
		

		if (sounds)
		{
			x += x * Time.deltaTime;
			if (x >= xEnd)
			{
				playGroal();
				x = 0;
			}
		}

		if (GameStatus.paused)
		{
			audioSource.Pause();
		} else if (GameStatus.unPaused)
		{
			audioSource.UnPause();
		}

	}

	void Shot(int damage)
	{
		
		bloodSplat.Play();
		Transform transform = GetComponentInChildren<Transform>();
		Quaternion rotation = Quaternion.LookRotation(gameObject.transform.forward, gameObject.transform.up);
		transform.rotation = rotation;
		
		
		health -= damage;
		CheckHealth();
	}

	void CheckHealth()
	{
		
		healthText.text ="Health: " + Mathf.Round(health);
		if (health <= 0)
		{
			
			Vector3 asd = new Vector3(transform.position.x,plane.position.y+0.05f,transform.position.z);
			GameObject bloodInstance = Instantiate(blood, asd, Quaternion.identity);
			gameStatus.AddKills();
			gameStatus.AddPoints(origHealth/10);
			if (GameStatus.smgDropCounterFinish && !GameStatus.smgDropped)
			{
				DropSmg();
			}

			if (gameStatus.dropPickup)
			{
				DropPickup(RandomPickup());
				gameStatus.dropPickup = false;
			}
			Destroy(bloodInstance,20);
			Destroy(gameObject);
		}
	}

	public void playGroal()
	{
		if (navMeshAgent.speed >= 4 && !audioSource.isPlaying)
		{
			
			audioSource.Play();
		}
	}

	void DropSmg()
	{
		Vector3 asd = new Vector3(transform.position.x,-4.15f,transform.position.z);
		GameObject smgDropper = Instantiate(smgDrop, asd, Quaternion.identity);
		GameStatus.smgDropped = true;
	}

	void DropPickup(int type)
	{
		Vector3 asd = new Vector3(transform.position.x,-4.15f,transform.position.z);
		GameObject pickupDropped = Instantiate(pickupDrop, asd, Quaternion.identity);
		Destroy(pickupDropped, 15);
		dropPickupType = type;
		ChangeLastDropped(dropPickupType);
		pickupDropped.GetComponentInChildren<TextMesh>().text = lastDropped;
		pickupDropped.GetComponent<PickUpScript>().SetType(dropPickupType);
	}
	void Chase()
	{
		
		navMeshAgent.destination = player.position;
		navMeshAgent.isStopped = false; 
	}

	public int RandomPickup()
	{
		int random = UnityEngine.Random.Range(0, 14);
		return random;
	}

	public void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Bullet"))
		{
			Shot(GameStatus.ammoDamage);
			Destroy(other.gameObject);
			
		}

		if (other.gameObject.CompareTag("Player"))
		{
			gameStatus.RemoveHealth(damageToPlayer);
			audioSource.PlayOneShot(ZombieHitClip);
		}
	}

	public void ChangeLastDropped(int type)
	{
		if (type == 0)
		{
			lastDropped = "Health + 20";
		}if (type == 1)
		{
			lastDropped = "Health + 50";
		}if (type == 2)
		{
			lastDropped = "Health + 100";
		}if (type == 3)
		{
			lastDropped = "Pistol Ammo + 12";
		}if (type == 4)
		{
			lastDropped = "Pistol Ammo + 36";
		}if (type == 5)
		{
			lastDropped = "Pistol Ammo + 108";
		}if (type == 6)
		{
			lastDropped = "SMG Ammo + 30";
		}if (type ==7)
		{
			lastDropped = "SMG Ammo + 90";
		}if (type == 8)
		{
			lastDropped = "SMG Ammo + 270";
		}if (type == 9)
		{
			lastDropped = "SMG Ammo + 810";
		}if (type == 10)
		{
			lastDropped = "Sprint Unlimited for: 30 sec";
		}if (type == 11)
		{
			lastDropped = "Sprint Unlimited for: 180 sec";
		}if (type ==12)
		{
			lastDropped = "Exp + 3*zombies";
		}if (type == 13)
		{
			lastDropped = "Exp + 10*zombies";
		}
		
	}
}
