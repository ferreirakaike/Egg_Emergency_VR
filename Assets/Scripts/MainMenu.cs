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
	private MainMenuAudioManager audioManager;
	public GameObject uiCanvas;
	public GameObject gameLabel;
	public GameObject startButton;
	public GameObject exitButton;
	public GameObject difficultySelection;
	
	private bool moveCanvasToStart = false;
	private bool animateButtonsToStart = false;
	private bool fadeButtonIn = false;
	
	void Start() {
		audioManager = GameObject.Find("SoundManager").GetComponent<MainMenuAudioManager>();
		uiCanvas.transform.position = new Vector3(uiCanvas.transform.position.x, uiCanvas.transform.position.y, 0.07f);
		moveCanvasToStart = true;
	}
	
	void Update() {
		if (moveCanvasToStart) {
			float finalZPosition = -9.0f;
			Vector3 newCanvasPosition = new Vector3(uiCanvas.transform.position.x, uiCanvas.transform.position.y, finalZPosition);
			uiCanvas.transform.position = Vector3.MoveTowards(uiCanvas.transform.position, newCanvasPosition, 10.0f * Time.deltaTime);
			if (uiCanvas.transform.position.z <= finalZPosition) {
				moveCanvasToStart = false;
				animateButtonsToStart = true;
			}
		} else if (animateButtonsToStart) {
			float finalYPosition = 18.3f;//1.550f + (18.3f - 4.1f);
			Vector3 newGameLabelPosition = new Vector3(gameLabel.transform.localPosition.x, finalYPosition, gameLabel.transform.localPosition.z);
			gameLabel.transform.localPosition = Vector3.MoveTowards(gameLabel.transform.localPosition, newGameLabelPosition, 10.0f * Time.deltaTime);
			if (gameLabel.transform.localPosition.y >= finalYPosition) {
				startButton.SetActive(true);
				exitButton.SetActive(true);
				difficultySelection.SetActive(true);
				
				Color startColor = startButton.GetComponent<Image>().material.color;
				startColor.a = 0.0f;
				startButton.GetComponent<Image>().material.color = startColor;
				animateButtonsToStart = false;
				fadeButtonIn = true;
			}
		} else if (fadeButtonIn) {
			Color finalColor = startButton.GetComponent<Image>().material.color;
			finalColor.a += 5.0f * Time.deltaTime;
			startButton.GetComponent<Image>().material.color = finalColor;
			if (finalColor.a >= 1.0f) {
				fadeButtonIn = false;
			}
		}
	}
	
	public void StartGame() {
		audioManager.PlayButtonClickSound();
		SceneManager.LoadScene("KaiScene");
	}
	
	public void SelectDifficulty(TMP_Dropdown dropdown) {
		audioManager.PlayButtonClickSound();
		MainMenu.difficulty = (Difficulty)dropdown.value;
	}
	
	public void Exit() {
		audioManager.PlayButtonClickSound();
		Application.Quit();
	}
	
	private IEnumerator Lerp_MeshRenderer_Color(MeshRenderer target_MeshRender, float 
	 lerpDuration, Color startLerp, Color targetLerp)
	 {
	     float lerpStart_Time = Time.time;
	     float lerpProgress;
	     bool lerping = true;
	     while (lerping)
	     {
	         yield return new WaitForEndOfFrame();
	         lerpProgress = Time.time - lerpStart_Time;
	         if (target_MeshRender != null)
	         {
	             target_MeshRender.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
	         }
	         else
	         {
	             lerping = false;
	         }
         
         
	         if (lerpProgress >= lerpDuration)
	         {
	             lerping = false;
	         }
	     }
	     yield break;
	 }
}