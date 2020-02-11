using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FPSGunController : MonoBehaviour
{
	public string currentGun;
	public GameObject pistol;
	public Animator gunAnimator;

	public GameObject ammoSpawn;
	public GameObject zombie;

	public GameObject bullet;
	// Use this for initialization
	void Start () {
		gunAnimator = gunAnimator.GetComponent<Animator>();
		gunAnimator.SetBool("GunIdle", false);
		

	}
	public void UpdateHand()
	{
		if (GameStatus.pistolInHand)
		{
			pistol.SetActive(true);
			currentGun = "pistol";
			gunAnimator.SetBool("GunIdle", true);
		}
		else
		{
			currentGun = null;
			pistol.SetActive(false);
			gunAnimator.SetBool("GunIdle", false);
		}
	}

	public void LowerHand()
	{
		GameStatus.pistolInHand = false;
		currentGun = null;
		pistol.SetActive(false);
		gunAnimator.SetBool("GunIdle", false);
	}
	
	// Update is called once per frame
	void Update () {
		
		UpdateHand();
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (!GameStatus.pistolInHand)
			{
				GameStatus.pistolInHand = true;
				pistol.SetActive(true);
				currentGun = "pistol";
				gunAnimator.SetBool("GunIdle", true);
			}
			else
			{
				GameStatus.pistolInHand = false;
				currentGun = null;
				pistol.SetActive(false);
				gunAnimator.SetBool("GunIdle",false);
			}
		}

		if (Input.GetMouseButtonDown(0) && currentGun == "pistol")
		{
			
			Debug.Log("BangBang");
			GameObject ammoInstance = Instantiate(bullet, ammoSpawn.transform.position, Quaternion.identity);
			Destroy(ammoInstance,2);
			ammoInstance.transform.localRotation = Quaternion.AngleAxis(90,transform.right);
			ammoInstance.GetComponent<Rigidbody>().AddForce(ammoSpawn.transform.forward * 100, ForceMode.Impulse);
		}
	}
}
