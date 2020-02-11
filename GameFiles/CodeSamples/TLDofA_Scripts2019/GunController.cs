using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
	public GameStatus status;
	public string currentGun;
	public GameObject pistol;
	public GameObject smgGun;
	public Animator gunAnimator;

	public GameObject ammoSpawn;
	public GameObject zombie;
	public Light muzzleFlash;
	public GameObject bullet;
	public static bool autoReload;
	public int pistolbullets;
	public int pistolmagazines;
	public SimpleHealthBar ammoBar;
	public int smgbullets;
	float smgCounter = 0;
	public int smgmagazines;
	public AudioSource audioSource;
	public AudioClip SMGsound;
	public smgShot SmgShot;
	public AudioClip GunFire;
	public AudioClip reload;
	public int quality;
	public float x = 1;
	public float xEnd = 1.2f;
	public bool shot;
	public Text ammoText;
	// Use this for initialization
	void Start () {
		gunAnimator = gunAnimator.GetComponent<Animator>();
		muzzleFlash.GetComponent<Light>();
		UpdateHand();
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = 0.1f;
		quality = UnityEngine.QualitySettings.GetQualityLevel();
	}

	public void UpdateHand()
	{
		if (GameStatus.pistolInHand)
		{
			GameStatus.pistolInHand = true;
			GameStatus.smgInHand = false;
			pistol.SetActive(true);
			smgGun.SetActive(false);
			currentGun = "pistol";
			gunAnimator.SetBool("GunIdle", true);
			gunAnimator.SetBool("SmgIdle",false);
			ammoBar.UpdateBar( pistolbullets, 12 );
		} else if (GameStatus.smgInHand)
		{
			GameStatus.pistolInHand = true;
			GameStatus.smgInHand = false;
			pistol.SetActive(true);
			smgGun.SetActive(false);
			currentGun = "pistol";
			gunAnimator.SetBool("GunIdle", true);
			gunAnimator.SetBool("SmgIdle",false);
			ammoBar.UpdateBar( smgbullets, 30 );
		}
		else
		{
			gunAnimator.SetBool("SmgIdle",false);
			gunAnimator.SetBool("GunIdle",false);
			GameStatus.smgInHand = false;
			GameStatus.pistolInHand = false;
			smgGun.SetActive(false);
			pistol.SetActive(false);
			currentGun = null;
		}
	}

	public void AddAmmo(string gun, int count)
	{
		if (gun == "pistol")
		{
			pistolmagazines += count;
			ammoBar.UpdateBar( pistolbullets, 12 );
			if (pistolbullets < 10)
			{

				ammoText.text = "Ammo/Mags:  " + pistolbullets + " / " + pistolmagazines;
			}
			else
			{
				ammoText.text = "Ammo/Mags: " + pistolbullets + " / " + pistolmagazines;
			}
		} else if (gun == "smg")
		{
			smgmagazines += count;
			ammoBar.UpdateBar( smgbullets, 30 );
			if (smgbullets < 10)
			{

				ammoText.text = "Ammo/Mags:  " + smgbullets + " / " + smgmagazines;
			}
			else
			{
				ammoText.text = "Ammo/Mags: " + smgbullets + " / " + smgmagazines;
			}
		}
	}

	public void ChangeToPistol()
	{
		GameStatus.pistolInHand = true;
		GameStatus.smgInHand = false;
		pistol.SetActive(true);
		smgGun.SetActive(false);
		currentGun = "pistol";
		gunAnimator.SetBool("GunIdle", true);
		gunAnimator.SetBool("SmgIdle",false);
		ammoBar.UpdateBar( pistolbullets, 12 );
		if (pistolbullets < 10)
		{

			ammoText.text = "Ammo/Mags:  " + pistolbullets + " / " + pistolmagazines;
		}
		else
		{
			ammoText.text = "Ammo/Mags: " + pistolbullets + " / " + pistolmagazines;
		}
	}

	public void ChangeToSmg()
	{
		if (GameStatus.smgPicked)
		{
			gunAnimator.SetBool("SmgIdle", true);
			gunAnimator.SetBool("GunIdle", false);
			GameStatus.smgInHand = true;
			GameStatus.pistolInHand = false;
			pistol.SetActive(false);
			currentGun = "smg";
			smgGun.SetActive(true);
			
			ammoBar.UpdateBar(smgbullets, 30);
			if (smgbullets < 10)
			{

				ammoText.text = "Ammo/Mags:  " + smgbullets + " / " + smgmagazines;
			}
			else
			{
				ammoText.text = "Ammo/Mags: " + smgbullets + " / " + smgmagazines;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		
		x += x * Time.deltaTime;
		if (x >= xEnd)
		{
			shot = false;
			muzzleFlash.enabled = false;
			x = 1;
		}

		
		if (GameStatus.firstPerson == false && !GameStatus.paused)
		{
			if ((Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)  && GameStatus.pistolInHand && GameStatus.smgPicked)
			{
			ChangeToSmg();	
			} else if ((Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f) &&
			           GameStatus.smgInHand)
			{
				ChangeToPistol();
			} else if ((Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f) &&
			           currentGun == null)
			{
				ChangeToPistol();
			}
			
				if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				if (GameStatus.smgInHand)
				{
					GameStatus.pistolInHand = true;
					GameStatus.smgInHand = false;
					pistol.SetActive(true);
					smgGun.SetActive(false);
					currentGun = "pistol";
					gunAnimator.SetBool("GunIdle", true);
					gunAnimator.SetBool("SmgIdle",false);
					ammoBar.UpdateBar( pistolbullets, 12 );
					if (pistolbullets < 10)
					{

						ammoText.text = "Ammo/Mags:  " + pistolbullets + " / " + pistolmagazines;
					}
					else
					{
						ammoText.text = "Ammo/Mags: " + pistolbullets + " / " + pistolmagazines;
					}
				}
				else
				if (!GameStatus.pistolInHand)
				{
					GameStatus.pistolInHand = true;
					GameStatus.smgInHand = false;
					pistol.SetActive(true);
					smgGun.SetActive(false);
					currentGun = "pistol";
					gunAnimator.SetBool("GunIdle", true);
					ammoBar.UpdateBar( pistolbullets, 12 );
					if (pistolbullets < 10)
					{

						ammoText.text = "Ammo/Mags:  " + pistolbullets + " / " + pistolmagazines;
					}
					else
					{
						ammoText.text = "Ammo/Mags: " + pistolbullets + " / " + pistolmagazines;
					}
				}
			 else
				{
					gunAnimator.SetBool("SmgIdle",false);
					gunAnimator.SetBool("GunIdle",false);
					GameStatus.smgInHand = false;
					GameStatus.pistolInHand = false;
					smgGun.SetActive(false);
					pistol.SetActive(false);
					currentGun = null;
				}
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				if (GameStatus.pistolInHand && GameStatus.smgPicked)
				{
					GameStatus.pistolInHand = false;
					GameStatus.smgInHand = true;
					pistol.SetActive(false);
					smgGun.SetActive(true);
					currentGun = "smg";
					gunAnimator.SetBool("GunIdle", false);
					gunAnimator.SetBool("SmgIdle",true);
					ammoBar.UpdateBar( smgbullets, 30 );
					if (smgbullets < 10)
					{

						ammoText.text = "Ammo/Mags:  " + smgbullets + " / " + smgmagazines;
					}
					else
					{
						ammoText.text = "Ammo/Mags: " + smgbullets + " / " + smgmagazines;
					}
				} else
				if (!GameStatus.smgInHand && GameStatus.smgPicked)
				{
					GameStatus.smgInHand = true;
					GameStatus.pistolInHand = false;
					pistol.SetActive(false);
					currentGun = "smg";
					smgGun.SetActive(true);
					gunAnimator.SetBool("SmgIdle",true);
					ammoBar.UpdateBar( smgbullets, 30 );
					if (smgbullets < 10)
					{

						ammoText.text = "Ammo/Mags:  " + smgbullets + " / " + smgmagazines;
					}
					else
					{
						ammoText.text = "Ammo/Mags: " + smgbullets + " / " + smgmagazines;
					}
				}
				 else
				{
					gunAnimator.SetBool("SmgIdle",false);
					gunAnimator.SetBool("GunIdle",false);
					GameStatus.smgInHand = false;
					GameStatus.pistolInHand = false;
					smgGun.SetActive(false);
					pistol.SetActive(false);
					currentGun = null;
				}
			}

			
			if (Input.GetMouseButtonDown(0) && currentGun == "pistol" && pistolbullets > 0)
			{
				audioSource.PlayOneShot(GunFire);
				shot = true;
				x = 1;
				if (quality > 1)
				{
					muzzleFlash.enabled = true;
				}

				Debug.Log("BangBang");
				GameObject ammoInstance = Instantiate(bullet, ammoSpawn.transform.position, Quaternion.identity);
				Destroy(ammoInstance, 2);
				ammoInstance.transform.localRotation = Quaternion.AngleAxis(90, transform.right);
				ammoInstance.GetComponent<Rigidbody>().AddForce(ammoSpawn.transform.forward * 100, ForceMode.Impulse);
				pistolbullets--;
				ammoBar.UpdateBar( pistolbullets, 12 );
				
				if (pistolbullets < 10)
				{

					ammoText.text = "Ammo/Mags:  " + pistolbullets + " / " + pistolmagazines;
				}
				else
				{
					ammoText.text = "Ammo/Mags: " + pistolbullets + " / " + pistolmagazines;
				}
	
			} else if (Input.GetMouseButton(0) && currentGun == "smg" && smgbullets > 0 && smgCounter >= 0.05)
			{
				audioSource.PlayOneShot(SMGsound);
				shot = true;
				x = 1;
				if (quality > 1)
				{
					muzzleFlash.enabled = true;
				}

				Debug.Log("SMGBang");
				GameObject ammoInstance = Instantiate(bullet, ammoSpawn.transform.position, Quaternion.identity);
				Destroy(ammoInstance, 2);
				SmgShot.shoot();
				ammoInstance.transform.localRotation = Quaternion.AngleAxis(90, transform.right);
				ammoInstance.GetComponent<Rigidbody>().AddForce(ammoSpawn.transform.forward * 100, ForceMode.Impulse);
				smgbullets--;
				
				ammoBar.UpdateBar( smgbullets, 30 );
				if (smgbullets < 10)
				{

					ammoText.text = "Ammo/Mags:  " + smgbullets + " / " + smgmagazines;
				}
				else
				{
					ammoText.text = "Ammo/Mags: " + smgbullets + " / " + smgmagazines;
				}
				smgCounter = 0;
			}
			else
			{
				SmgShot.stopShoot();
			}

			smgCounter += Time.deltaTime;
			
			if ((Input.GetKeyDown(KeyCode.R) && currentGun == "pistol"|| autoReload && currentGun == "pistol" && pistolbullets == 0)&& pistolmagazines > 0)
			{
				if (pistolbullets < 12)
				{
					audioSource.PlayOneShot(reload);
				}
				gunAnimator.SetBool("Reload",true);
				
				gunAnimator.SetBool("GunIdle", true);
				if (pistolmagazines - 12 + pistolbullets >= 0)
				{
					pistolmagazines -= (12-pistolbullets);
					pistolbullets = 12;
				}
				else if (pistolmagazines - 12 + pistolbullets < 0)
				{
					
					pistolbullets += pistolmagazines;
					pistolmagazines -= pistolmagazines;
				}
				
				ammoBar.UpdateBar( pistolbullets, 12 );
				if (pistolbullets < 10)
				{

					ammoText.text = "Ammo/Mags:  " + pistolbullets + " / " + pistolmagazines;
				}
				else
				{
					ammoText.text = "Ammo/Mags: " + pistolbullets + " / " + pistolmagazines;
				}
				
			} else if ((Input.GetKeyDown(KeyCode.R) && currentGun == "smg" ||
			            autoReload && currentGun == "smg" && smgbullets == 0) && smgmagazines > 0)
			{
				if (smgbullets < 30)
				{
					audioSource.PlayOneShot(reload);
				}
				gunAnimator.SetBool("Reload", true);
				gunAnimator.SetBool("SmgIdle", true);
				if (smgmagazines - 30 + smgbullets >= 0)
				{
					smgmagazines -= 30 - smgbullets;
					smgbullets = 30;
				}
				else if (smgmagazines - 30 + smgbullets < 0)
				{
					smgbullets += smgmagazines;
					smgmagazines -= smgmagazines;
				}


				ammoBar.UpdateBar(smgbullets, 30);
				if (smgbullets < 10)
				{

					ammoText.text = "Ammo/Mags:  " + smgbullets + " / " + smgmagazines;
				}
				else
				{
					ammoText.text = "Ammo/Mags: " + smgbullets + " / " + smgmagazines;
				}
			}
		}
	}
}
