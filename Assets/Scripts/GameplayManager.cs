using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class GameplayManager : MonoBehaviourPunCallbacks
{
	private MainMenuAudioManager audioManager;
	private AudioManager nonMainMenuAudioManager;

	private TextMeshProUGUI[] scoreText;
	private TextMeshProUGUI[] deterrentCount;
	public int health = 0;
	public static bool gameIsOver = false;
	
	private GameObject[] heart1;
	private GameObject[] heart2;
	private GameObject[] heart3;
	private GameObject[] heart4;
	private GameObject[] heart5;
	public GameObject minusSign;
	
	private GameObject scoreCanvas;
	private GameObject allHearts;
	private GameObject tombstone;
	private GameObject graveUpright;
	private GameObject graveDown;
	private GameObject deterrentCanvas;
	
	private TwoHandGrabInteractable basket;
	private NetworkVariablesAndReferences networkVar;
	public static int[] scores = {0, 0};
	private int localPlayerIndex = -1;
	private int otherPlayerIndex = -1;

	private int gameStreak;
	public int[] deterrentsAvailable = {0, 0};
	public int streakToDeterrent;
	private bool firstTimeStreak = true;
	/// <summary>
	/// Override the on enable method of MonoBehaviourPunCallbacks.
	/// Instantiates necessary variables
	/// </summary>
	public override void OnEnable()
	{
		base.OnEnable();
		audioManager = GameObject.Find("UISoundManager").GetComponent<MainMenuAudioManager>();
		nonMainMenuAudioManager = GameObject.Find("SoundManager").GetComponent<AudioManager>();
		networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
		scoreText = new TextMeshProUGUI[2];
		deterrentCount = new TextMeshProUGUI[2];
		deterrentsAvailable = new int[2];
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
		basket = PhotonView.Find(networkVar.basketIDs[localPlayerIndex]).GetComponent<TwoHandGrabInteractable>();
		tombstone = PhotonView.Find(networkVar.tombstoneIDs[localPlayerIndex]).gameObject;
		graveUpright = tombstone.transform.Find("Game Gravestone Upright").gameObject;
		graveDown = tombstone.transform.Find("Game Gravestone Down").gameObject;
		Transform hearts = tombstone.transform.Find("Hearts");
		heart1 = new GameObject[2];
		heart2 = new GameObject[2];
		heart3 = new GameObject[2];
		heart4 = new GameObject[2];
		heart5 = new GameObject[2];
		heart1[localPlayerIndex] = hearts.Find("Heart 1").gameObject;
		heart2[localPlayerIndex] = hearts.Find("Heart 2").gameObject;
		heart3[localPlayerIndex] = hearts.Find("Heart 3").gameObject;
		heart4[localPlayerIndex] = hearts.Find("Heart 4").gameObject;
		heart5[localPlayerIndex] = hearts.Find("Heart 5").gameObject;
		allHearts = hearts.gameObject;
		scoreCanvas = tombstone.transform.Find("Canvas").gameObject;
		scoreText[localPlayerIndex]  = scoreCanvas.transform.Find("Score Value Label").GetComponent<TextMeshProUGUI>();
		deterrentCanvas = tombstone.transform.Find("Deterrent_Bomb").GetChild(0).gameObject;
		deterrentCount[localPlayerIndex] = deterrentCanvas.transform.Find("Deterrent Count").GetComponent<TextMeshProUGUI>();
		scores[localPlayerIndex] = 0;
		gameIsOver = false;
		scoreText[localPlayerIndex].text = $"{scores[localPlayerIndex]}";
		gameStreak = 0;
		deterrentsAvailable[localPlayerIndex] = 0;
		firstTimeStreak = true;
		deterrentCount[localPlayerIndex].text = $"{deterrentsAvailable[localPlayerIndex]}";

		if (NetworkManager.isMultiplayer)
		{
			GameObject otherTombstone = PhotonView.Find(networkVar.tombstoneIDs[otherPlayerIndex]).gameObject;
			scoreText[otherPlayerIndex] = otherTombstone.transform.Find("Canvas").Find("Score Value Label").GetComponent<TextMeshProUGUI>();
			deterrentCount[otherPlayerIndex] = otherTombstone.transform.Find("Deterrent_Bomb").GetChild(0).Find("Deterrent Count").GetComponent<TextMeshProUGUI>();
			scoreText[otherPlayerIndex].text = $"{scores[otherPlayerIndex]}";
			deterrentCount[otherPlayerIndex].text = $"{deterrentsAvailable[otherPlayerIndex]}";
			Transform otherHearts = otherTombstone.transform.Find("Hearts");
			heart1[otherPlayerIndex] = otherHearts.Find("Heart 1").gameObject;
			heart2[otherPlayerIndex] = otherHearts.Find("Heart 2").gameObject;
			heart3[otherPlayerIndex] = otherHearts.Find("Heart 3").gameObject;
			heart4[otherPlayerIndex] = otherHearts.Find("Heart 4").gameObject;
			heart5[otherPlayerIndex] = otherHearts.Find("Heart 5").gameObject;
		}
		
		switch(Gameplay.menuDifficulty) {
			case Difficulty.Easy:
				photonView.RPC("SyncHearts", RpcTarget.AllBuffered, 5, localPlayerIndex);
				health = 5;
				break;
			case Difficulty.Medium:
				photonView.RPC("SyncHearts", RpcTarget.AllBuffered, 3, localPlayerIndex);
				health = 3;
				break;
			case Difficulty.Hard:
				photonView.RPC("SyncHearts", RpcTarget.AllBuffered, 1, localPlayerIndex);
				health = 1;
				break;
		}

		if (streakToDeterrent == 0)
		{
			streakToDeterrent = 10;
		}
	}

	public void IncreaseScore()
	{
		 if (GameplayManager.gameIsOver) {
			 return;
		 }
		float percentage = basket.transform.localScale.x / basket.maxScale;
		int scoreIncrease = (int)(1f + (5f * (1f - percentage)));
		scores[localPlayerIndex] += scoreIncrease;
		photonView.RPC("SyncScore", RpcTarget.AllBuffered, scores[localPlayerIndex], localPlayerIndex);
		gameStreak++;
		if (NetworkManager.isMultiplayer && (gameStreak % streakToDeterrent == 0))
		{
			IncreaseDeterrentsAvailable();
		}
	}

	private void IncreaseDeterrentsAvailable()
	{
		deterrentsAvailable[localPlayerIndex]++;
		if (firstTimeStreak)
		{
			deterrentCanvas.transform.Find("Deterrent Notification").gameObject.SetActive(true);
			firstTimeStreak = false;
		}
		UpdateDeterrentCountText();
	}

	public void UpdateDeterrentCountText()
	{
		photonView.RPC("SyncScore", RpcTarget.AllBuffered, deterrentCount[localPlayerIndex], localPlayerIndex);
	}

	[PunRPC]
	private void SyncHearts(int heartCount, int playerIndex)
	{
		switch(heartCount)
		{
			case 0: 
				heart1[playerIndex].SetActive(false);
				heart2[playerIndex].SetActive(false);
				heart3[playerIndex].SetActive(false);
				heart4[playerIndex].SetActive(false);
				heart5[playerIndex].SetActive(false);
				break;
			case 1:
				heart1[playerIndex].SetActive(false);
				heart2[playerIndex].SetActive(false);
				heart3[playerIndex].SetActive(true);
				heart4[playerIndex].SetActive(false);
				heart5[playerIndex].SetActive(false);
				break;
			case 2:
				heart1[playerIndex].SetActive(false);
				heart2[playerIndex].SetActive(true);
				heart3[playerIndex].SetActive(true);
				heart4[playerIndex].SetActive(false);
				heart5[playerIndex].SetActive(false);
				break;
			case 3:
				heart1[playerIndex].SetActive(false);
				heart2[playerIndex].SetActive(true);
				heart3[playerIndex].SetActive(true);
				heart4[playerIndex].SetActive(true);
				heart5[playerIndex].SetActive(false);
				break;
			case 4:
				heart1[playerIndex].SetActive(true);
				heart2[playerIndex].SetActive(true);
				heart3[playerIndex].SetActive(true);
				heart4[playerIndex].SetActive(true);
				heart5[playerIndex].SetActive(false);
				break;
			case 5:
				heart1[playerIndex].SetActive(true);
				heart2[playerIndex].SetActive(true);
				heart3[playerIndex].SetActive(true);
				heart4[playerIndex].SetActive(true);
				heart5[playerIndex].SetActive(true);
				break;
		}
	}

	[PunRPC]
	private void SyncDeterrentCount(int newData, int playerIndex)
	{
		deterrentsAvailable[playerIndex] = newData;
		deterrentCount[playerIndex].text = $"{deterrentsAvailable[playerIndex]}";
	}

	[PunRPC]
	private void SyncScore(int newScore, int playerIndex)
	{
		scores[playerIndex] = newScore;
		scoreText[playerIndex].text = $"{scores[playerIndex]}";
	}

	public void DecreaseScore()
	{
		if (GameplayManager.gameIsOver) {
			return;
		}
		if (localPlayerIndex == 0)
		{
			Instantiate(minusSign, new Vector3(0.03999999f, 0.45f, -8.3f), Quaternion.identity);
		}
		else if (localPlayerIndex == 1)
		{
			Instantiate(minusSign, new Vector3(5f, 0.45f, -8.3f), Quaternion.identity);
		}
		
		health--;
		if (health == 0)
		{
			networkVar.UpdateIsGameOver(true);
		}
		photonView.RPC("SyncHearts", RpcTarget.AllBuffered, health, localPlayerIndex);
		gameStreak = 0;
	}
	
	public void gameOver()
	{
		nonMainMenuAudioManager.StopBackgroundMusic();
		audioManager.PlayGameOverSound();
		scoreCanvas.SetActive(false);
		allHearts.SetActive(false);
		GameOver.moveCanvasToStart = true;
		GameplayManager.gameIsOver = true;
		graveUpright.SetActive(false);
		graveDown.SetActive(true);
	}
}
