using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;

public enum Difficulty {
	Easy = 0,
	Medium = 1,
	Hard = 2
}

public class MainMenu : MonoBehaviour {
	public static Difficulty difficulty = Difficulty.Easy;
	
	public void StartGame() {
		SceneManager.LoadScene("SampleScene");
	}
	
	public void SelectDifficulty(TMP_Dropdown dropdown) {
		MainMenu.difficulty = (Difficulty)dropdown.value;
	}
	
	public void Exit() {
		Application.Quit();
	}
}