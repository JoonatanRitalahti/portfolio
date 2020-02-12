using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStatus : MonoBehaviour
{



	public static GameStatus status;

	public static bool firstPerson;
	public static bool pistolInHand;
	public static bool smgInHand;
	public static bool smgPicked;
	public static bool smgDropCounterFinish;
	public static bool smgDropped;
	
	public static bool paused;
	public static bool unPaused;
	
	public static float deadInfo1;
	public static float deadInfo2;
	public static float deadInfo3;
	public static float deadInfo4;
	
	public static float sprintCalculatorMax;
	public static float sprintCalc;
	public static bool sprintConst;
	public static bool sprintPicked;
	public PauseMenu pauseMenu;
	public float enemySpeed;
	public float enemyAnimatorSpeed;
	public LevelUpper levelUpper;
	public SimpleHealthBar healthBar;
	public static int smgDropCounter = 0;
	public int smgDropKills;
	public float experience;
	public float health;
	public float previousHealth;
	public float maxHealth;
	public GameObject Camera;
	public GameObject fpsCamera;
	public float gameHardness = 1;
	public float hardnessSpeed;
	public Text hardness;
	public bool dropPickup;
	public bool randomSelected;
	private int dropCalculator;
	private int rand;
	public PostProcessingProfile postProcess;

	public static float fullXp;
	//0 no, 1  yes, 2 did previously
	private int hardnessUpdate = 0;
	public int kills;
	public int zombiesInScene;
	public Text zombieCounter;
	public Text uiKills;
	public Terrain terrain;
	public float zombieHealthMax;
	public float zombieHealthOriginal = 20;
	public GameObject zombiePrefab;
	public GameObject ground;
	public float zombieMaxSpeed;
	public GameObject player;
	public static int ammoDamage = 10;
	private float calc;
	public float points;
	public Text uiPoints;

	float Offset = 10.0f;
	float AboveGround = 1.0f;
	bool TerrainOnly = true;

// Use this for initialization
	void Awake()
	{
		/*
		 if (status == null)
		{
			DontDestroyOnLoad(gameObject);
			status = this;
		}
		else if (status != null)
		{
			Destroy(gameObject);
		}
		*/
	}

	void Start()
	{
		
		sprintCalculatorMax = 0;
		zombiesInScene = 0;
		calc = 0;
		AddEnemies(20);
		if (UnityEngine.QualitySettings.GetQualityLevel() < 4)
		{
			postProcess.antialiasing.enabled = false;
			postProcess.fog.enabled = false;
			postProcess.depthOfField.enabled = false;
			postProcess.motionBlur.enabled = false;

		}
		else
		{
			
			postProcess.antialiasing.enabled = true;
			postProcess.fog.enabled = true;
			postProcess.depthOfField.enabled = true;
			postProcess.motionBlur.enabled = true;
			
		}

	}

	private float RandomSpeed()
	{
		float randm;
		if (zombieMaxSpeed * gameHardness + 3 < 12)
		{
			 randm = UnityEngine.Random.Range(2f, zombieMaxSpeed * gameHardness + 3);
		}
		else
		{
			 randm = UnityEngine.Random.Range(4f, 12f);
		}

		return randm;
	}

	private void LateUpdate()
	{
		if (unPaused)
		{
			unPaused = false;
		}

		
	}

	private void Update()
	{
		if (zombiesInScene <= 5)
		{
			zombieHealthMax = gameHardness * zombieHealthOriginal;
			AddEnemies();
		}
		if (sprintPicked)
		{
			sprintConst = true;
			sprintCalc += Time.deltaTime;
			if (sprintCalc >= sprintCalculatorMax)
			{
				sprintCalc = 0;
				sprintCalculatorMax = 0;
				sprintPicked = false;
				sprintConst = false;
			}
		}

		/*
		if (Input.GetKey(KeyCode.KeypadPlus))
		{
			AddHardness(0.2f);
		} else if (Input.GetKey(KeyCode.KeypadMinus) && gameHardness >= 1)
		{
			AddHardness(-0.2f);
		}
		*/
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}

		if (firstPerson == true)
		{
			fpsCamera.SetActive(true);
			Camera.SetActive(false);
		}
		else
		{
		
			fpsCamera.SetActive(false);
			Camera.SetActive(true);

		}
		
		gameHardness += gameHardness * hardnessSpeed * Time.deltaTime;
		hardness.text = "Hardness: " + gameHardness ;

		/*
		if (Input.GetKey(KeyCode.F))
		{
			AddEnemies();
		}
		*/

		if (calc >= 10 && zombiesInScene < 75)
		{
			zombieHealthMax = gameHardness * zombieHealthOriginal;
			AddEnemies();
			calc = 0;
			
		}
		

		calc += gameHardness * Time.deltaTime;
		if (kills % 20 == 0 && hardnessUpdate == 0 && kills != 0)
		{
			hardnessUpdate = 1;
		}

		if (hardnessUpdate == 1)
		{
			AddHardness(1);
			hardnessUpdate = 2;
		}

	}

	public void PauseGame()
	{
		if (paused)
		{
			paused = false;
			pauseMenu.TogglePause();
			unPaused = true;
		} else if (!paused)
		{
			paused = true;
			pauseMenu.TogglePause();
		}
	}
	public void AddPickup()
	{
		if (!dropPickup && !randomSelected)
		{
			dropCalculator = 0;
			rand = UnityEngine.Random.Range(2, 6);
			randomSelected = true;
		}

		if (randomSelected && !dropPickup)
		{
			dropCalculator++;
		}

		if (dropCalculator >= rand)
		{
			randomSelected = false;
			dropPickup = true;
			
			dropCalculator = 0;
		}
	}
	public void AddKills(int count = 1)
	{
		AddPickup();
		kills += count;
		uiKills.text = "Kills: " + kills;
		hardnessUpdate = 0;
		zombiesInScene -= count;
		zombieCounter.text = "Zombies In Scene: " + zombiesInScene + "\nAmmo Damage: " + ammoDamage;
		if (smgDropCounterFinish || smgDropped) return;
		smgDropCounter += 1;
		if (smgDropCounter >= smgDropKills)
		{
			smgDropCounterFinish = true;
			smgDropCounter = 0;
		}
	}
	
	

	public void ResetKills()
	{
		kills = 0;
		uiKills.text = "Kills: " + kills;


	}

	public void RemoveKills(int count = 1)
	{
		kills -= count;
		uiKills.text = "Kills: " + kills;
	}

	public void AddPoints(int count = 1)
	{
		if (count * gameHardness < 200)
		{
			points += count * gameHardness;
		}
		else
		{
			points += Random.Range(200,250);
		}
		
		levelUpper.AddExp(Mathf.RoundToInt(count * gameHardness));
		uiPoints.text = "Money: " + Mathf.Round(points) + "$";
	}

	public void ResetPoints()
	{
		points = 0;
		uiPoints.text = "Money: " + Mathf.Round(points) + "$";
	}

	public void RemovePoints(int count = 1)
	{
		points -= count;
		uiPoints.text = "Money: " + Mathf.Round(points) + "$";
	}

	public void RemoveHealth(float amount = 1)
	{		
		deadInfo1 = points;
		deadInfo2 = kills;
		deadInfo3 = fullXp;
		deadInfo4 = levelUpper.level;
		
		
		health -= amount;
		UpdateHealth();
		
		
	}

	public void UpdateHealth()
	{
		healthBar.UpdateBar(health, maxHealth);
		if (health <= 0)
		{
			SceneManager.LoadScene("Dead");
		}
	}
	public void AddHealth(float amount = 1)
	{
		if (health + amount <= 100)
		{
			health += amount;
		}
		else
		{
			health = 100;
			
		}

		healthBar.UpdateBar(health, maxHealth);
	}

	public void AddEnemies(int count = 1,float enemyHealth = 1)
	{


		int counter = 0;
		while (true)
		{
			if (counter >= count)
			{
				break;
			}

			Vector3 position = RandomPositionOnTerrain();
			GameObject enemy = Instantiate(zombiePrefab, position, Quaternion.identity);
			enemySpeed = RandomSpeed();
			enemyAnimatorSpeed = enemySpeed / 1.5f;
			enemy.GetComponent<Animator>().speed = enemyAnimatorSpeed;
			enemy.GetComponent<NavMeshAgent>().speed = enemySpeed;
			zombiesInScene++;
			zombieCounter.text = "Zombies In Scene: " + zombiesInScene + "\nAmmo Damage: " + ammoDamage;
			counter++;
		}
	}


	// Use this for initialization
	public Vector3 RandomPositionOnTerrain()
	{

		Vector3 newPosition;
		while (true)
		{
			newPosition = new Vector3(Random.Range(0, 500), 0, Random.Range(0, 500));
			if(Vector3.Distance(newPosition, player.transform.position) > 35 && Vector3.Distance(newPosition, player.transform.position) < 80)
			{
				//ei kaupunkiin
				if ((newPosition.x < 275 || newPosition.x > 340) && (newPosition.z < 133 || newPosition.z > 188))
				{
					break;
				}
			}
		}

	
			return newPosition;
	}


	public void AddHardness(float amount)
	{
		gameHardness += amount;
	}
}