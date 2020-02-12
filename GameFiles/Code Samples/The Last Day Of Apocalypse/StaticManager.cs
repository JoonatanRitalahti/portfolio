using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticManager : MonoBehaviour {
	public static string currentLevel;
	public bool firstStart = true;
	public StaticManager status;

	public AudioSource audioSource;
	// Use this for initialization
	void Start () {
		
	}
	void Awake()
	{
		if (status == null)
		{
			DontDestroyOnLoad(gameObject);
			status = this;
		}
		else if (status != null)
		{
			Destroy(gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void PauseMusic()
	{
		audioSource.Pause();
	}

	public void PlayMusic()
	{
		audioSource.UnPause();
	}
}
