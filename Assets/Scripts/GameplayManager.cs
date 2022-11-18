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
	
	private GameObject heart1;
	private GameObject heart2;
	private GameObject heart3;
	private GameObject heart4;
	private GameObject heart5;
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
		heart1 = hearts.Find("Heart 1").gameObject;
		heart2 = hearts.Find("Heart 2").gameObject;
		heart3 = hearts.Find("Heart 3").gameObject;
		heart4 = hearts.Find("Heart 4").gameObject;
		heart5 = hearts.Find("Heart 5").gameObject;
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
			scoreText[otherPlayerIndex] = PhotonView.Find(networkVar.tombstoneIDs[otherPlayerIndex]).transform.Find("Canvas").Find("Score Value Label").GetComponent<TextMeshProUGUI>();
			deterrentCount[otherPlayerIndex] = PhotonView.Find(networkVar.tombstoneIDs[otherPlayerIndex]).transform.Find("Deterrent_Bomb").GetChild(0).Find("Deterrent Count").GetComponent<TextMeshProUGUI>();
			scoreText[otherPlayerIndex].text = $"{scores[otherPlayerIndex]}";
			deterrentCount[otherPlayerIndex].text = $"{deterrentsAvailable[otherPlayerIndex]}";
		}
		
		switch(Gameplay.menuDifficulty) {
			case Difficulty.Easy:
				heart1.SetActive(true);
				heart2.SetActive(true);
				heart3.SetActive(true);
				heart4.SetActive(true);
				heart5.SetActive(true);
				health = 5;
				break;
			case Difficulty.Medium:
				heart2.SetActive(true);
				heart3.SetActive(true);
				heart4.SetActive(true);
				health = 3;
				break;
			case Difficulty.Hard:
				heart3.SetActive(true);
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
		Instantiate(minusSign, new Vector3(0.03999999f, 0.45f, -8.3f), Quaternion.identity);
		switch(Gameplay.menuDifficulty) {
			case Difficulty.Easy:
				if (health == 5) {
					heart5.SetActive(false);
				} else if (health == 4) {
					heart4.SetActive(false);
				} else if (health == 3) {
					heart3.SetActive(false);
				} else if (health == 2) {
					heart2.SetActive(false);
				} else if (health == 1) {
					heart1.SetActive(false);
					networkVar.UpdateIsGameOver(true);
				}
				break;
			case Difficulty.Medium:
				if (health == 3) {
					heart4.SetActive(false);
				} else if (health == 2) {
					heart3.SetActive(false);
				} else if (health == 1) {
					heart2.SetActive(false);
					networkVar.UpdateIsGameOver(true);
				}
				break;
			case Difficulty.Hard:
				if (health > 0) {
					heart3.SetActive(false);
					networkVar.UpdateIsGameOver(true);
				}
			break;
		}
		health--;
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
