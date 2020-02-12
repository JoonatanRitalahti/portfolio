using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SmgupdateText : MonoBehaviour {


	public float speed;
	public TextMeshProUGUI text;
	
	// Use this for initialization
	void Start ()
	{
		text = GetComponent<TextMeshProUGUI>();
		text.fontSize = 20;
	}
	
	// Update is called once per frame
	void Update()
	{
		
		if (text.fontSize <= 100){		
			text.fontSize = text.fontSize + speed * Time.deltaTime;
		}
		else
		{
			Destroy(gameObject);

		}
	}
}
