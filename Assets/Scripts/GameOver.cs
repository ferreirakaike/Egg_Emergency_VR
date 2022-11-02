using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;


public class GameOver : MonoBehaviour {
	private MainMenuAudioManager audioManager;
	
	public GameObject uiCanvas;
	public GameObject gameLabel;
	public GameObject finalScoreLabel;
	public TextMeshProUGUI scoreText;
	public GameObject mainMenuButton;
	
	public static bool moveCanvasToStart = false;
	private bool animateButtonsToStart = false;
	private bool fadeButtonIn = false;
	
	void Start() {
		audioManager = GameObject.Find("UISoundManager").GetComponent<MainMenuAudioManager>();
		uiCanvas.transform.position = new Vector3(uiCanvas.transform.position.x, uiCanvas.transform.position.y, 0.07f);
		uiCanvas.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	void Update() {
		if (GameOver.moveCanvasToStart) {
			uiCanvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			float finalZPosition = -10.4f;
			Vector3 newCanvasPosition = new Vector3(uiCanvas.transform.position.x, uiCanvas.transform.position.y, finalZPosition);
			uiCanvas.transform.position = Vector3.MoveTowards(uiCanvas.transform.position, newCanvasPosition, 10.0f * Time.deltaTime);
			if (uiCanvas.transform.position.z <= finalZPosition) {
				moveCanvasToStart = false;
				animateButtonsToStart = true;
			}
		} else if (animateButtonsToStart) {
			float finalYPosition = -16.8f;//1.550f + (18.3f - 4.1f);
			Vector3 newGameLabelPosition = new Vector3(gameLabel.transform.localPosition.x, finalYPosition, gameLabel.transform.localPosition.z);
			gameLabel.transform.localPosition = Vector3.MoveTowards(gameLabel.transform.localPosition, newGameLabelPosition, 10.0f * Time.deltaTime);
			if (gameLabel.transform.localPosition.y >= finalYPosition) {
				mainMenuButton.SetActive(true);
				scoreText.text = $"Final Score: {GameplayManager.score}";
				finalScoreLabel.SetActive(true);
				
				Color startColor = mainMenuButton.GetComponent<Image>().material.color;
				startColor.a = 0.0f;
				mainMenuButton.GetComponent<Image>().material.color = startColor;
				animateButtonsToStart = false;
				fadeButtonIn = true;
			}
		} else if (fadeButtonIn) {
			Color finalColor = mainMenuButton.GetComponent<Image>().material.color;
			finalColor.a += 5.0f * Time.deltaTime;
			mainMenuButton.GetComponent<Image>().material.color = finalColor;
			if (finalColor.a >= 1.0f) {
				fadeButtonIn = false;
			}
		}
	}
	
	public void OpenMainMenu() {
		audioManager.PlayButtonClickSound();
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("MainMenu");
	}
}