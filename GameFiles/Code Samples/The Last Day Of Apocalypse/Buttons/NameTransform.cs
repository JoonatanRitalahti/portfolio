using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameTransform : MonoBehaviour {
	public Vector3 random;
	private float startx;
	private float starty;
	public float speed;
	public TextMeshProUGUI text;
	public StaticManager firstload;
	public bool play;
	public bool played;
	public AudioSource audioSource;
	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource>();
		startx = transform.position.x;
		text = GetComponent<TextMeshProUGUI>();
		starty = transform.position.y;
		firstload = GameObject.Find("StaticManager").GetComponent<StaticManager>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (firstload.firstStart == true)
		{
			if (text.fontSize >= 40)
			{
				text.fontSize = text.fontSize - speed * Time.deltaTime;
			}

			if (text.fontSize <= 40)
			{
				random = new Vector3(startx + Random.Range(-3, 3), starty + Random.Range(-3, 3), transform.position.z);
				transform.position = random;
				play = true;
			}
		}
		else
		{
			text.fontSize = 40;
			random = new Vector3(startx + Random.Range(-3, 3), starty + Random.Range(-3, 3), transform.position.z);
			transform.position = random;
		}

		if (play && !played)
		{
			audioSource.Play();
			played = true;
		}
	}
}
