using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmgPickup : MonoBehaviour
{
	public SmgupdateText SMGtext;
	
	// Use this for initialization
	void Start () {
		SMGtext = GameObject.Find("SMGtext").GetComponent<SmgupdateText>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (SMGtext != null)
			{
				SMGtext.GetComponent<SmgupdateText>().enabled = true;
			}

			GameStatus.smgPicked = true;
				
			Destroy(gameObject);
		}
	}
}
