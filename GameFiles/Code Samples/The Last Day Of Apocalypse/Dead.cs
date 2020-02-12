using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

	
public class Dead : MonoBehaviour {
	
	
	public SceneManage loader;
	public Button start;
	public Button menu;
	public Button exit;
	public TextMeshProUGUI levels;

	public GameStatus gameStatus;
	// Use this for initialization
	void Start ()
	{
		
		if (GameObject.Find("Manager") != null)
		{
			gameStatus = GameObject.Find("Manager").GetComponent<GameStatus>();
		}

		levels.text = "You got: \n" + Mathf.Round(GameStatus.deadInfo1) + "$\n" + GameStatus.deadInfo2 +
		              " kills\n" + GameStatus.deadInfo3 + " experience\nAnd you achieved " + GameStatus.deadInfo4 +
		              " levels!";
		GameStatus.pistolInHand = false;
		GameStatus.ammoDamage = 10;
		GameStatus.smgDropped = false;
		GameStatus.smgDropCounterFinish = false;
		GameStatus.smgInHand = false;
		GameStatus.smgDropCounter = 0;
		GameStatus.smgPicked = false;
		GunController.autoReload = false;
		GameStatus.fullXp = 0;
		if (gameStatus != null)
		{
			Destroy(gameStatus.gameObject);
		}

		start.onClick.AddListener(ContinueButton);
		exit.onClick.AddListener(ExitButton);
		menu.onClick.AddListener(MenuButton);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ContinueButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
	}

	void ExitButton()
	{
		Application.Quit();
	}

	void MenuButton()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
	}
}
