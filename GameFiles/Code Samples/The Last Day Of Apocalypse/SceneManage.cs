using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneManage : MonoBehaviour
{
	public VideoPlayer videoPlayer;
	public string levelToLoad;
	private float x = 0;

	private float xx = 10;
	// Use this for initialization
	void Start () {
		/*
		if (!String.IsNullOrEmpty(levelToLoad))
		{
			SceneManager.LoadScene(levelToLoad);
		}
		*/
		
	}
	
	// Update is called once per frame
	void Update()
	{
		x += Time.deltaTime;
		
		if (videoPlayer != null)
		{
			if (videoPlayer.frame >= (long) videoPlayer.frameCount && x>=xx)
			{
				if (!String.IsNullOrEmpty(levelToLoad))
				{
					SceneManager.LoadScene(levelToLoad);
				}
			}
		}
	}

	public void LoadLevel(string name)
	{
		SceneManager.LoadScene(name);
	}

	
}
