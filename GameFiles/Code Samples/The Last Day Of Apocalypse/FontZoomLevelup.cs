using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontZoomLevelup : MonoBehaviour
{
	public float speed;
	public TextMeshProUGUI text;
	// Use this for initialization
	void Start ()
	{
		text = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
			if (text.fontSize <= 80 && text.enabled)
			{
				text.fontSize = text.fontSize + speed * Time.deltaTime;
				text.outlineWidth = text.outlineWidth + 0.05f * Time.deltaTime;
			}

			if (text.fontSize >= 80)
			{
				text.enabled = false;
				text.fontSize = 36;
				text.outlineWidth = 0.1f;

			}
		
		
			
			
	}
}
