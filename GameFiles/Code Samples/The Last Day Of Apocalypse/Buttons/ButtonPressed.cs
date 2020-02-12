using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
	public SceneManage loader;
	public Button start;
	public StaticManager staticManager;
	public Button settings;
	public Button back;
	public GameObject menuCanvas;
	public GameObject settingsCanvas;

	public Button exit;

	// Use this for initialization
	void Start ()
	{
		staticManager = GameObject.Find("StaticManager").GetComponent<StaticManager>();
		start.onClick.AddListener(StartButton);
		settings.onClick.AddListener(SettingsButton);
		exit.onClick.AddListener(ExitButton);
		back.onClick.AddListener(BackButton);
	}
	
	

	void StartButton()
	{
		loader.LoadLevel("Level1");
		
		staticManager.firstStart = false;
	}

	void SettingsButton()
	{
		menuCanvas.SetActive(false);
		settingsCanvas.SetActive(true);
		
		staticManager.firstStart = false;
	}

	void BackButton()
	{
		menuCanvas.SetActive(true);
		settingsCanvas.SetActive(false);
		
	}

	void ExitButton()
	{
		Application.Quit();
	}
}
