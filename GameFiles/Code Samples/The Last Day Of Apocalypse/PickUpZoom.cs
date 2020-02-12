using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpZoom : MonoBehaviour
{
	public float speed;
	public TextMesh text;
	public int move;
	public bool notEmpty;
	// Use this for initialization
	void Start ()
	{
		
		//text = GetComponent<TextMesh>();
		move = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
			if (notEmpty && move == 1)
			{
				text.fontSize = text.fontSize + Mathf.RoundToInt(speed * Time.deltaTime);
				
				if (text.fontSize >= 500)
				{
					move = 2;
				}
			}

			if (notEmpty && move == 2)
			{
				text.fontSize = text.fontSize - Mathf.RoundToInt(speed * Time.deltaTime);
				move = 2;
				if (text.fontSize <= 450)
				{
					move = 1;
				}
			}

		if (!notEmpty)
		{
			if (move != 1 || text.fontSize != 450)
			{
				
				text.fontSize = 450;
			}
		}

		notEmpty = !string.IsNullOrEmpty(text.text);
	}
	
}
