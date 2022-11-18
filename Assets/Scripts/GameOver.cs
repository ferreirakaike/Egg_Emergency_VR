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
	
	private GameObject uiCanvas;
	private GameObject gameLabel;
	private GameObject finalScoreLabel;
	private TextMeshProUGUI scoreText;
	private GameObject mainMenuButton;
	
	public static bool moveCanvasToStart = false;
	private bool animateButtonsToStart = false;
	private bool fadeButtonIn = false;
	private int localPlayerIndex = 0;
	private int otherPlayerIndex = 1;
	private Vector3 userPosition;
	void Start() {
		audioManager = GameObject.Find("UISoundManager").GetComponent<MainMenuAudioManager>();
		uiCanvas = PhotonNetwork.Instantiate("GameOverPanel", new Vector3(userPosition.x, userPosition.y, 0.07f), Quaternion.identity);
		uiCanvas.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
		gameLabel = uiCanvas.transform.Find("Game Title").gameObject;
		finalScoreLabel = uiCanvas.transform.Find("Game Final Score").gameObject;
		scoreText = finalScoreLabel.GetComponent<TextMeshProUGUI>();
		mainMenuButton = uiCanvas.transform.Find("Main Menu Button").gameObject;
		if (PhotonNetwork.IsMasterClient)
		{
			localPlayerIndex = 0;
			otherPlayerIndex = 1;
		}
		else
		{
			localPlayerIndex = 1;
			otherPlayerIndex = 0;
		}
		StartCoroutine(GetUserPosition());
	}

	IEnumerator GetUserPosition()
	{
		yield return new WaitForSeconds(5f);
		userPosition = Camera.main.transform.position;
		uiCanvas.transform.position = new Vector3(userPosition.x, userPosition.y, 0.07f);
	}
	
	void Update() {
		if (GameOver.moveCanvasToStart) {
			uiCanvas.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			float finalZPosition = userPosition.z + 0.7f;
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
				if (NetworkManager.isMultiplayer)
				{
					if (GameplayManager.scores[localPlayerIndex] > GameplayManager.scores[otherPlayerIndex])
					{
						scoreText.text = "You Win!!!";
					}
					else if (GameplayManager.scores[localPlayerIndex] < GameplayManager.scores[otherPlayerIndex])
					{
						scoreText.text = "You Lose!!!";
					}
					else
					{
						scoreText.text = "Game Tied";
					}
				}
				else
				{
					scoreText.text = $"Final Score: {GameplayManager.scores[localPlayerIndex]}";
				}
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
		PhotonNetwork.LeaveRoom();
		SceneManager.LoadScene("MainMenu");
	}
}