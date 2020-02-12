using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashGun : MonoBehaviour
{

	public Light flash;

	private bool flashing = false;
	// Use this for initialization
	void Start ()
	{
		flash = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		int i = 1;
		if (flashing)
		{
			i++;
		}
		else
		{
			i--;
		}
		if (i == 100)
		{
			flash.range = 0;
			flashing = false;
		}
		else if (i == 0)
		{
			
			flash.range = 3.5f;
			flashing = true;
		}

		
	}
}
