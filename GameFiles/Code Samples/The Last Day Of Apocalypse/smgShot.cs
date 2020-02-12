using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smgShot : MonoBehaviour
{
	public GameObject smgShooterPoint;
	public bool smgplayss;
	public ParticleSystem smg;
	// Use this for initialization
	void Start ()
	{
		smgplayss = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void shoot()
	{
		//GameObject smgShoot = Instantiate(smgShooterPoint, transform.position, Quaternion.identity);
		//Destroy(smgShoot,1);
		if (smgplayss == false)
		{
			smg.Play();
		}

		smgplayss = true;

	}

	public void stopShoot()
	{
		smg.Stop();
		smgplayss = false;
	}
}
