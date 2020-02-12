using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpper : MonoBehaviour
{
	public int startLevel;
	public int level;

	public int experience;
	public TextMeshProUGUI exptext;
	public int neededForNextLevel;
	public int neededstart;
	public int leftForNextLevel;
	public TextMeshProUGUI levelUp;
	public SimpleHealthBar healthBar;
	// Use this for initialization
	void Start ()
	{
		level = startLevel;
		neededForNextLevel = neededstart;
		leftForNextLevel = neededstart;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddExp(int count)
	{
		GameStatus.fullXp += count;
		experience += count;
		//LEVEL UP
		while (experience >= neededForNextLevel)
		{
			if (experience >= neededForNextLevel)
			{
				level++;
				levelUp.text = "LEVEL UP\nLevel: " + level;
				levelUp.enabled = true;
				GameStatus.ammoDamage += 5;
				experience = 0 + experience - neededForNextLevel;
				
				neededForNextLevel += level * 100;
				leftForNextLevel = neededForNextLevel - experience;
				

			}
		}
		if (experience < neededForNextLevel)
		{
			leftForNextLevel = neededForNextLevel - experience;
		}
		healthBar.UpdateBar( experience, neededForNextLevel );
		exptext.text = "Level: " + level + " Exp: " + experience + " Next Level: " + leftForNextLevel;
	}
}
