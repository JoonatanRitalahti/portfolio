using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public TextMeshProUGUI moneyText;
	public Canvas uiCanvas;
	public SceneManage loader;
	public Button start;
	public StaticManager staticManager;
	public Button ReloadItem;
	public Button menu;
	public Text neededforReload;
	public Button exit;
	public Toggle debugToggle;
	public Text zombiezInScene;
	public Text fps;
	public Text hardness;
	public bool debugIsToggledOn;
	public TextMeshProUGUI keys;
	public GameStatus gameStatus;
	float counter = 1;
	public int avgFrameRate;
	public Text display_Text;
	
	// Use this for initialization
	void Start () {
		if (GameObject.Find("StaticManager") != null)
		{
			staticManager = GameObject.Find("StaticManager").GetComponent<StaticManager>();
		}

		start.onClick.AddListener(ContinueButton);
		exit.onClick.AddListener(ExitButton);
		menu.onClick.AddListener(MenuButton);
		ReloadItem.onClick.AddListener(ReloadBuy);
		
		debugToggle.onValueChanged.AddListener(DebugToggled);
		keys.text = "Button Layout \nMove: W A S D\nSprint: Left Shift\nJump: Space\nReload: R\nShoot: Left Mouse Button\nChange Weapons: Scroll Wheel\nPistol: 1\nSmg(When found): 2";
		
		debugIsToggledOn = false;
		zombiezInScene.enabled = false;
			
		fps.enabled = false;
		hardness.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		float current = 0;
		
		if (fps.enabled)
			
		{
		current = (int)(1f / Time.unscaledDeltaTime);
		avgFrameRate = (int)current;

			if (counter >= 1.2)
			{
				fps.text = "FPS: " + avgFrameRate;
				counter = 1;
			}

			counter += Time.deltaTime;
		}
	}

	public void TogglePause()
	{
		if (GameStatus.paused)
		{
			uiCanvas.gameObject.SetActive(false);
			Time.timeScale = 0f;
			foreach (Transform child in transform)
			{
				//child is your child transform
				child.gameObject.SetActive(true);
			}

			moneyText.text = "You Have: " + Mathf.RoundToInt(gameStatus.points) + "$";
			if (GunController.autoReload == true)
			{
				neededforReload.text = "Bought";
			}
			else
			{
				neededforReload.text = "5000$";
			}
		} else if (!GameStatus.paused)
		{
			Time.timeScale = 1f;
			
			uiCanvas.gameObject.SetActive(true);
			foreach (Transform child in transform)
			{
				//child is your child transform
				child.gameObject.SetActive(false);
			}
		}
	}

	public void ContinueButton()
	{
		gameStatus.PauseGame();
	}

	public void MenuButton()
	{
		if (GameObject.Find("StaticManager") != null)
		{
			staticManager.firstStart = false;
		}

		loader.LoadLevel("Main Menu");
		
	}

	public void ExitButton()
	{
		Application.Quit();
	}

	public void ReloadBuy()
	{
		if (Mathf.RoundToInt(gameStatus.points) >= 5000 && !GunController.autoReload)
		{
			GunController.autoReload = true;
			gameStatus.RemovePoints(5000);
			moneyText.text = "You Have: " + Mathf.RoundToInt(gameStatus.points) + "$";
			neededforReload.text = "Bought";
		}
		else if(!GunController.autoReload)
		{
			neededforReload.text = "5000$ " + "You don't have enough money";
		}
	}

	public void DebugToggled(bool toggleStatus)
	{
		if (toggleStatus)
		{
			debugIsToggledOn = true;
			zombiezInScene.enabled = true;
			
			fps.enabled = true;
			hardness.enabled = true;
		} else if (toggleStatus == false)
		{
			debugIsToggledOn = false;
			zombiezInScene.enabled = false;
			
			fps.enabled = false;
			hardness.enabled = false;
			
		}
	}
	
	
}
