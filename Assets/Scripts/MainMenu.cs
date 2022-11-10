using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public enum Difficulty {
	Easy = 0,
	Medium = 1,
	Hard = 2
}

public class MainMenu : MonoBehaviourPunCallbacks {
	public static Difficulty difficulty = Difficulty.Easy;
	private MainMenuAudioManager audioManager;
	public GameObject uiCanvas;
	public GameObject gameLabel;
	public GameObject createButton;
	public GameObject startButton;
	public GameObject exitButton;
	public GameObject joinButton;
	public GameObject notificationText;
	public GameObject dropdown;
	public GameObject difficultyDropdown;
	public GameObject multiplayerToggle;
	private GameObject overrideButton;
	private NetworkManager networkManager;
	
	private bool moveCanvasToStart = false;
	private bool animateButtonsToStart = false;
	private bool fadeButtonIn = false;
	
	void Start() {
		audioManager = GameObject.Find("SoundManager").GetComponent<MainMenuAudioManager>();
		uiCanvas.transform.position = new Vector3(uiCanvas.transform.position.x, uiCanvas.transform.position.y, 0.07f);
		moveCanvasToStart = true;
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		overrideButton = transform.Find("OverrideDropdownButton").gameObject;
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
				createButton.SetActive(true);
				joinButton.SetActive(true);
				exitButton.SetActive(true);
				
				Color createColor = createButton.GetComponent<Image>().material.color;
				createColor.a = 0.0f;
				createButton.GetComponent<Image>().material.color = createColor;
				Color joinColor = joinButton.GetComponent<Image>().material.color;
				joinColor.a = 0.0f;
				joinButton.GetComponent<Image>().material.color = joinColor;
				animateButtonsToStart = false;
				fadeButtonIn = true;
			}
		} else if (fadeButtonIn) {
			Color finalColor = createButton.GetComponent<Image>().material.color;
			finalColor.a += 5.0f * Time.deltaTime;
			createButton.GetComponent<Image>().material.color = finalColor;
			if (finalColor.a >= 1.0f) {
				fadeButtonIn = false;
			}
			finalColor = joinButton.GetComponent<Image>().material.color;
			finalColor.a += 5.0f * Time.deltaTime;
			joinButton.GetComponent<Image>().material.color = finalColor;
			if (finalColor.a >= 1.0f) {
				fadeButtonIn = false;
			}
		}
	}
	
	public void StartGame() {
		audioManager.PlayButtonClickSound();
		if (startButton.GetComponentInChildren<TextMeshProUGUI>().text == "Start")
		{
			TMP_Dropdown tmp_dropdown = dropdown.GetComponent<TMP_Dropdown>();
			int result;
			if (Int32.TryParse(tmp_dropdown.options[tmp_dropdown.value].text, out result))
			{
				if (!networkManager.InitializeRoom(result))
				{
					Debug.Log("Failed to create room");
				}
				notificationText.GetComponent<TextMeshProUGUI>().text = "";
				StartCoroutine(SetNotification("Failed to create room. Room already exist or network error.\nPlease try creating different a room or join a room"));
				notificationText.SetActive(true);
			}
		}
		else if (startButton.GetComponentInChildren<TextMeshProUGUI>().text == "Join")
		{
			TMP_Dropdown tmp_dropdown = dropdown.GetComponent<TMP_Dropdown>();
			int result;
			if (Int32.TryParse(tmp_dropdown.options[tmp_dropdown.value].text, out result))
			{
				if (!networkManager.JoinRoom(result))
				{
					Debug.Log("Failed to join room");
				}
				notificationText.GetComponent<TextMeshProUGUI>().text = "";
				StartCoroutine(SetNotification("Failed to join room. Room is either full or does not exist.\nPlease create a room or join a different room"));
				notificationText.SetActive(true);
			}
		}
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		StopCoroutine(SetNotification());
	}

	IEnumerator SetNotification(string str = null)
	{
		if (str != null)
		{
			yield return new WaitForSeconds(1);
			notificationText.GetComponent<TextMeshProUGUI>().text = str;
		}
  }

	public void SetMultiplayerNotification(Toggle setting)
  {
    if(setting.isOn)
		{
			notificationText.GetComponent<TextMeshProUGUI>().text = "NOTICE: Game only starts when both players have grabbed their basket in Multiplayer Mode";
			notificationText.SetActive(true);
		}
		else
		{
			notificationText.GetComponent<TextMeshProUGUI>().text = "";
			notificationText.SetActive(false);
		}
  }

	public void OnCreateRoomButtonClicked()
	{
		audioManager.PlayButtonClickSound();
		startButton.SetActive(true);
		createButton.SetActive(false);
		joinButton.SetActive(false);
		TMP_Dropdown tmp_dropdown = dropdown.GetComponent<TMP_Dropdown>();
		tmp_dropdown.ClearOptions();
		List<TMP_Dropdown.OptionData> dropdown_options = new List<TMP_Dropdown.OptionData>();
		foreach (int room in networkManager.availableRoom)
		{
				dropdown_options.Add(new TMP_Dropdown.OptionData() {text = room.ToString()});
		}
		
		tmp_dropdown.AddOptions(dropdown_options);
		dropdown.SetActive(true);
		multiplayerToggle.SetActive(true);
		difficultyDropdown.SetActive(true);
		exitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
		startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
		if(NetworkManager.isMultiplayer)
		{
			notificationText.GetComponent<TextMeshProUGUI>().text = "NOTICE: Game only starts when both players have grabbed their basket in Multiplayer Mode";
			notificationText.SetActive(true);
		}
	}

	public void OnJoinRoomButtonClicked()
	{
		audioManager.PlayButtonClickSound();
		startButton.SetActive(true);
		createButton.SetActive(false);
		joinButton.SetActive(false);
		TMP_Dropdown tmp_dropdown = dropdown.GetComponent<TMP_Dropdown>();
		tmp_dropdown.ClearOptions();
		List<TMP_Dropdown.OptionData> dropdown_options = new List<TMP_Dropdown.OptionData>();
		foreach (int room in networkManager.availableRoom)
		{
				dropdown_options.Add(new TMP_Dropdown.OptionData() {text = room.ToString()});
		}
		
		tmp_dropdown.AddOptions(dropdown_options);
		dropdown.SetActive(true);
		exitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
		startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Join";
	}
	
	public void SelectDifficulty(TMP_Dropdown dropdown) {
		audioManager.PlayButtonClickSound();
		MainMenu.difficulty = (Difficulty)dropdown.value;
	}
	
	public void Exit() {
		if (exitButton.GetComponentInChildren<TextMeshProUGUI>().text == "Exit")
		{
			audioManager.PlayButtonClickSound();
			Application.Quit();
		}
		else if (exitButton.GetComponentInChildren<TextMeshProUGUI>().text == "Back")
		{
			audioManager.PlayButtonClickSound();
			joinButton.SetActive(true);
			createButton.SetActive(true);
			dropdown.SetActive(false);
			overrideButton.SetActive(false);
			multiplayerToggle.SetActive(false);
			difficultyDropdown.SetActive(false);
			startButton.SetActive(false);
			notificationText.SetActive(false);
			exitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Exit";
		}
		
	}
}