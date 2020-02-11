using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
	///<see cref="0=health20, 1=health50, 2=health100, 3=pistolammo12, 4=pistolammo36, 5=pistolammo108, 6=smgammo30, 7=smgammo90, 8=smgammo270, 9=smgammo810, 10=sprint30sec, 11=sprint180sec, 12=exp*3zomb, 13=exp*10 zomb"/>
	public int type;

	public GameStatus gameStatus;
	private GameObject particle;
	public GunController gunController;
	// Use this for initialization
	void Start ()
	{
		gameStatus = GameObject.Find("Manager").GetComponent<GameStatus>();
		gunController = GameObject.Find("soldier").GetComponent<GunController>();
		if (UnityEngine.QualitySettings.GetQualityLevel() < 4)
		{
			Destroy(GetComponentInChildren<ParticleSystem>().gameObject);
			GetComponentInChildren<MeshFilter>().gameObject.SetActive(true);
		}
		else
		{
			GetComponentInChildren<MeshFilter>().gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetType(int pickupType)
	{
		type = pickupType;
		
	}

	private void PickupType()
	{
		if (type == 0)
		{
			gameStatus.AddHealth(20);
		} else if (type == 1)
		{
			gameStatus.AddHealth(50);
		} else if (type == 2)
		{
			gameStatus.AddHealth(100);
		} else if (type == 3)
		{
			gunController.AddAmmo("pistol",12);
		} else if (type == 4)
		{
			gunController.AddAmmo("pistol",36);
		} else if (type == 5)
		{
			gunController.AddAmmo("pistol",108);
		} else if (type == 6)
		{
			gunController.AddAmmo("smg",30);
		} else if (type == 7)
		{
			gunController.AddAmmo("smg",90);
		} else if (type == 8)
		{
			gunController.AddAmmo("smg",270);
		} else if (type == 9)
		{
			gunController.AddAmmo("smg",810);
		} else if (type == 10)
		{
			GameStatus.sprintPicked = true;
			GameStatus.sprintCalculatorMax += 30;
		} else if (type == 11)
		{
			GameStatus.sprintPicked = true;
			GameStatus.sprintCalculatorMax += 180;
		} else if (type == 12)
		{
			gameStatus.AddPoints(Mathf.RoundToInt(gameStatus.zombieHealthMax/10*3));
		} else if (type == 13)
		{
			gameStatus.AddPoints(Mathf.RoundToInt(gameStatus.zombieHealthMax));
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			
			PickupType();
			Destroy(gameObject);
		}
	}
}
