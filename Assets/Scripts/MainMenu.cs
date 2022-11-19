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

/// <summary>
/// Enum that holds the difficulty of the game
/// </summary>
public enum Difficulty {
	Easy = 0,
	Medium = 1,
	Hard = 2
}


/// <summary>
/// Main menu class where user can select to create room, join room, set room difficulty, etc.
/// </summary>
public class MainMenu : MonoBehaviourPunCallbacks {
	/// <summary>
	/// This variable holds the currently set game difficulty
	/// </summary>
	public static Difficulty difficulty = Difficulty.Easy;
	private MainMenuAudioManager audioManager;

	/// <summary>
	/// Reference to the UICanvas gameobject
	/// </summary>
	public GameObject uiCanvas;

	/// <summary>
	/// Reference to the Menu title
	/// </summary>
	public GameObject gameLabel;

	/// <summary>
	/// Reference to the Create Room button
	/// </summary>
	public GameObject createButton;

	/// <summary>
	/// Reference to the Start game button
	/// </summary>
	public GameObject startButton;

	/// <summary>
	/// Reference to the Exit game button
	/// </summary>
	public GameObject exitButton;

	/// <summary>
	/// Reference to the HELP button
	/// </summary>
	public GameObject helpButton;

	/// <summary>
	/// Reference to the HELP panel
	/// </summary>
	public GameObject helpPanel;

	/// <summary>
	/// Reference to the Join room button
	/// </summary>
	public GameObject joinButton;

	/// <summary>
	/// Reference to the Notification text in the UI Menu
	/// </summary>
	public GameObject notificationText;

	/// <summary>
	/// Reference to the room number dropdown menu
	/// </summary>
	public GameObject dropdown;

	/// <summary>
	/// Reference to the difficulty dropdown menu
	/// </summary>
	public GameObject difficultyDropdown;

	/// <summary>
	/// Reference to the multiplater toggle
	/// </summary>
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
		difficulty = Difficulty.Easy;
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
				helpButton.SetActive(true);
				
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

	/// <summary>
	/// This method is used to display the help menu
	/// </summary>
	public void Help()
	{
		helpPanel.SetActive(!helpPanel.activeSelf);
		audioManager.PlayButtonClickSound();
	}
	
	/// <summary>
	/// This method puts the user into a room and start the game
	/// </summary>
	public void StartGame() {
		audioManager.PlayButtonClickSound();
		if (startButton.GetComponentInChildren<TextMeshProUGUI>().text == "Start")
		{
			TMP_Dropdown tmp_dropdown = dropdown.GetComponent<TMP_Dropdown>();
			int result;
			if (PhotonNetwork.InRoom)
			{
				StartCoroutine(SetNotification("You are already in a room.\nPlease hit \"Back\" and try again!", 0));
				notificationText.SetActive(true);
			}
			else if (Int32.TryParse(tmp_dropdown.options[tmp_dropdown.value].text, out result))
			{
				if (!networkManager.InitializeRoom(result))
				{
					Debug.Log("Failed to create room");
					StartCoroutine(SetNotification("Failed to create room due network error!", 0));
					notificationText.SetActive(true);
				}
				else
				{
					if (NetworkManager.isMultiplayer)
					{
						StartCoroutine(SetNotification("Waiting for second player to join ...", 0));
						notificationText.SetActive(true);
					}
				}
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
			}
		}
	}

	/// <summary>
	/// Override parent method. Notify player if the room creation failed
	/// </summary>
	/// <param name="returnCode"></param>
	/// <param name="message"></param>
	public override void OnCreateRoomFailed (short returnCode, string message)
	{
		base.OnCreateRoomFailed(returnCode, message);
		StartCoroutine(SetNotification("Failed to create room. Room already exist or network error.\nPlease try creating different a room or join a room", 0.5f));
		notificationText.SetActive(true);
	}

	/// <summary>
	/// Override parent method. Notify player if the room joining failed
	/// </summary>
	/// <param name="returnCode"></param>
	/// <param name="message"></param>
	public override void OnJoinRoomFailed (short returnCode, string message)
	{
		base.OnJoinRoomFailed(returnCode, message);
		StartCoroutine(SetNotification("", 0f));
		StartCoroutine(SetNotification("Failed to join room. Room is either full or does not exist.\nPlease create a room or join a different room", 0.5f));
		notificationText.SetActive(true);
	}

	/// <summary>
	/// Override parent method. This method stop the SetNotification coroutine in the current scene.
	/// This method will also start the game if the game mode is set to single player
	/// </summary>
	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		StopCoroutine(SetNotification());
		if (!NetworkManager.isMultiplayer)
		{
			PhotonNetwork.LoadLevel("KaiScene");
		}
	}

	IEnumerator SetNotification(string str = null, float delay = 1f)
	{
		if (str != null)
		{
			yield return new WaitForSeconds(delay);
			notificationText.GetComponent<TextMeshProUGUI>().text = str;
		}
  }

	/// <summary>
	/// This method is used to set the notice for when Multiplayer toggle is set
	/// </summary>
	/// <param name="setting"></param>
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

	/// <summary>
	/// This method displays the dropdown menus so that user can select a room number and set room difficulty before creating a room.
	/// </summary>
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

	/// <summary>
	/// This method displays a dropdown list of room numbers that user could potentially join
	/// </summary>
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
	
	/// <summary>
	/// This method is used to set the difficulty of the game.
	/// </summary>
	/// <param name="dropdown">This is the selected value from the difficulty dropdown menu.</param>
	public void SelectDifficulty(TMP_Dropdown dropdown) {
		audioManager.PlayButtonClickSound();
		MainMenu.difficulty = (Difficulty)dropdown.value;
	}
	
	/// <summary>
	/// This method is used to close the application wehen the Exit button is clicked.
	/// </summary>
	public void Exit() {
		if (exitButton.GetComponentInChildren<TextMeshProUGUI>().text == "Exit")
		{
			audioManager.PlayButtonClickSound();
			Application.Quit();
		}
		else if (exitButton.GetComponentInChildren<TextMeshProUGUI>().text == "Back")
		{
			if (PhotonNetwork.InRoom)
			{
				PhotonNetwork.LeaveRoom();
			}
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