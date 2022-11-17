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
	public GameObject otherUiCanvas;
	public GameObject otherGameLabel;
	public GameObject otherFinalScoreLabel;
	public TextMeshProUGUI otherScoreText;
	public GameObject mainMenuButton;
	public GameObject otherMainMenuButton;
	
	public static bool moveCanvasToStart = false;
	private bool animateButtonsToStart = false;
	private bool fadeButtonIn = false;
	
	public static bool moveOtherCanvasToStart = false;
	private bool animateOtherButtonsToStart = false;
	private bool fadeOtherButtonIn = false;
	
	void Start() {
		audioManager = GameObject.Find("UISoundManager").GetComponent<MainMenuAudioManager>();
		uiCanvas.transform.position = new Vector3(uiCanvas.transform.position.x, uiCanvas.transform.position.y, 0.07f);
		uiCanvas.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
		otherUiCanvas.transform.position = new Vector3(otherUiCanvas.transform.position.x, otherUiCanvas.transform.position.y, 0.07f);
		otherUiCanvas.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	void Update() {
		// Player One
		if (GameOver.moveCanvasToStart) {
			uiCanvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			float finalZPosition = -10.17f;
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
				scoreText.text = $"Final Score: {GameplayManager.score}";
				finalScoreLabel.SetActive(true);
				animateButtonsToStart = false;
				
				if (PhotonNetwork.IsMasterClient) {
					mainMenuButton.SetActive(true);
					Color startColor = mainMenuButton.GetComponent<Image>().material.color;
					startColor.a = 0.0f;
					mainMenuButton.GetComponent<Image>().material.color = startColor;
					fadeButtonIn = true;
				}
			}
		} else if (fadeButtonIn) {
			Color finalColor = mainMenuButton.GetComponent<Image>().material.color;
			finalColor.a += 5.0f * Time.deltaTime;
			mainMenuButton.GetComponent<Image>().material.color = finalColor;
			if (finalColor.a >= 1.0f) {
				fadeButtonIn = false;
			}
		}
		
		// Player Two
		if (GameOver.moveOtherCanvasToStart) {
			otherUiCanvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			float finalZPosition = -10.17f;
			Vector3 newCanvasPosition = new Vector3(otherUiCanvas.transform.position.x, otherUiCanvas.transform.position.y, finalZPosition);
			otherUiCanvas.transform.position = Vector3.MoveTowards(otherUiCanvas.transform.position, newCanvasPosition, 10.0f * Time.deltaTime);
			if (otherUiCanvas.transform.position.z <= finalZPosition) {
				moveOtherCanvasToStart = false;
				animateOtherButtonsToStart = true;
			}
		} else if (animateOtherButtonsToStart) {
			float finalYPosition = -16.8f;//1.550f + (18.3f - 4.1f);
			Vector3 newGameLabelPosition = new Vector3(otherGameLabel.transform.localPosition.x, finalYPosition, otherGameLabel.transform.localPosition.z);
			otherGameLabel.transform.localPosition = Vector3.MoveTowards(otherGameLabel.transform.localPosition, newGameLabelPosition, 10.0f * Time.deltaTime);
			if (otherGameLabel.transform.localPosition.y >= finalYPosition) {
				otherScoreText.text = $"Final Score: {GameplayManager.otherScore}";
				otherFinalScoreLabel.SetActive(true);
				animateOtherButtonsToStart = false;
				
				if (NetworkManager.isMultiplayer && !PhotonNetwork.IsMasterClient) {
					otherMainMenuButton.SetActive(true);
					Color startColor = otherMainMenuButton.GetComponent<Image>().material.color;
					startColor.a = 0.0f;
					otherMainMenuButton.GetComponent<Image>().material.color = startColor;
					fadeOtherButtonIn = true;
				}
			}
		} else if (fadeOtherButtonIn) {
			Color finalColor = otherMainMenuButton.GetComponent<Image>().material.color;
			finalColor.a += 5.0f * Time.deltaTime;
			otherMainMenuButton.GetComponent<Image>().material.color = finalColor;
			if (finalColor.a >= 1.0f) {
				fadeOtherButtonIn = false;
			}
		}
	}
	
	public void OpenMainMenu() {
		audioManager.PlayButtonClickSound();
		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("MainMenu");
	}
}