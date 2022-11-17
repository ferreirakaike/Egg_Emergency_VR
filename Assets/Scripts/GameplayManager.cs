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

	public static bool gameIsOver = false;
	private bool foundOtherBasket = false;
	
	// Player One
	public TextMeshProUGUI scoreText;
	public static int score = 0;
	public int health = 0;
	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;
	public GameObject heart4;
	public GameObject heart5;
	public GameObject minusSign;
	public GameObject scoreCanvas;
	public GameObject allHearts;
	public GameObject graveUpright;
	public GameObject graveDown;
	public int playerOneStreak = 0;
	
	// Player Two
	public GameObject otherGameScore;
	public TextMeshProUGUI otherScoreText;
	public static int otherScore = 0;
	public int otherHealth = 0;
	public GameObject otherHeart1;
	public GameObject otherHeart2;
	public GameObject otherHeart3;
	public GameObject otherHeart4;
	public GameObject otherHeart5;
	public GameObject otherMinusSign;
	public GameObject otherScoreCanvas;
	public GameObject otherAllHearts;
	public GameObject otherGraveUpright;
	public GameObject otherGraveDown;
	public int playerTwoStreak = 0;
	
	private TwoHandGrabInteractable basket;
	private TwoHandGrabInteractable otherBasket;
	private NetworkVariablesAndReferences networkVar;
	
	/// <summary>
	/// Override the on enable method of MonoBehaviourPunCallbacks.
	/// Instantiates necessary variables
	/// </summary>
	public override void OnEnable()
	{
		base.OnEnable();
		StartTheGame();
	}
	
    // Start is called before the first frame update
    void Start()
    {
		StartTheGame();
    }
	
	void StartTheGame() {
		if (!NetworkManager.isMultiplayer) {
			otherGameScore.SetActive(false);
		}
		audioManager = GameObject.Find("UISoundManager").GetComponent<MainMenuAudioManager>();
		nonMainMenuAudioManager = GameObject.Find("SoundManager").GetComponent<AudioManager>();
		networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
		int playerOneId = PhotonNetwork.IsMasterClient ? 0 : 1;
		int playerTwoId = PhotonNetwork.IsMasterClient ? 1 : 0;
		basket = PhotonView.Find(networkVar.basketIDs[playerOneId]).GetComponent<TwoHandGrabInteractable>();
		if (NetworkManager.isMultiplayer && networkVar.basketIDs[playerTwoId] != -1) {
			otherBasket = PhotonView.Find(networkVar.basketIDs[playerTwoId]).GetComponent<TwoHandGrabInteractable>();
			foundOtherBasket = true;
			Debug.Log("FOUND");
		}
		else if (!NetworkManager.isMultiplayer) {
			foundOtherBasket = true;
		}
		score = 0;
		otherScore = 0;
		gameIsOver = false;
		scoreText.text = $"{GameplayManager.score}";
		otherScoreText.text = $"{GameplayManager.otherScore}";
		
		switch(MainMenu.difficulty) {
			case Difficulty.Easy:
				// Player One
				heart1.SetActive(true);
				heart2.SetActive(true);
				heart3.SetActive(true);
				heart4.SetActive(true);
				heart5.SetActive(true);
				health = 5;
				// Player Two
				otherHeart1.SetActive(true);
				otherHeart2.SetActive(true);
				otherHeart3.SetActive(true);
				otherHeart4.SetActive(true);
				otherHeart5.SetActive(true);
				otherHealth = 5;
				break;
			case Difficulty.Medium:
				// Player One
				heart2.SetActive(true);
				heart3.SetActive(true);
				heart4.SetActive(true);
				health = 3;
				// Player Two
				otherHeart2.SetActive(true);
				otherHeart3.SetActive(true);
				otherHeart4.SetActive(true);
				otherHealth = 3;
				break;
			case Difficulty.Hard:
				// Player One
				heart3.SetActive(true);
				health = 1;
				// Player Two
				otherHeart3.SetActive(true);
				otherHealth = 1;
				break;
		}
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		int playerTwoId = PhotonNetwork.IsMasterClient ? 1 : 0;
        if (!foundOtherBasket && networkVar.basketIDs[playerTwoId] != -1) {
			otherBasket = PhotonView.Find(networkVar.basketIDs[playerTwoId]).GetComponent<TwoHandGrabInteractable>();
			foundOtherBasket = true;
			Debug.Log("FOUND");
        }
    }
	
	public int calculateIncreaseScore() {
		if (GameplayManager.gameIsOver) {
			return 0;
		}
		// Player One
		if (PhotonNetwork.IsMasterClient) {
			float percentage = basket.transform.localScale.x / basket.maxScale;
			return (int)(1f + (5f * (1f - percentage)));
		}
		// Player Two
		else {
			float percentage = otherBasket.transform.localScale.x / otherBasket.maxScale;
			return (int)(1f + (5f * (1f - percentage)));
		}
	}
	
	public void IncreasePlayerOneScore(int scoreIncrease) {
		if (GameplayManager.gameIsOver) {
			return;
	 	}
		GameplayManager.score += scoreIncrease;
		scoreText.text = $"{GameplayManager.score}";
		playerOneStreak++;
		if (playerOneStreak >= 5) {
			// TODO: -- Show Player One Send Deterrent UI
		}
	}
	
	public void IncreasePlayerTwoScore(int scoreIncrease) {
		if (GameplayManager.gameIsOver) {
			return;
	 	}
		GameplayManager.otherScore += scoreIncrease;
		otherScoreText.text = $"{GameplayManager.otherScore}";
		playerTwoStreak++;
		if (playerTwoStreak >= 5) {
			// TODO: -- Show Player Two Send Deterrent UI
		}
	}
	
	public void DecreasePlayerOneScore() {
		 if (GameplayManager.gameIsOver) {
			 return;
		 }
		 playerOneStreak = 0;
		 // TODO: -- Hide Player One Send Deterrent UI
		 
		 Instantiate(minusSign, new Vector3(0.03999999f, 0.45f, -8.3f), Quaternion.identity);
		 switch(MainMenu.difficulty) {
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
					health--;
					gameOver();
				}
				break;
			case Difficulty.Medium:
				if (health == 3) {
					heart4.SetActive(false);
				} else if (health == 2) {
					heart3.SetActive(false);
				} else if (health == 1) {
					heart2.SetActive(false);
					health--;
					gameOver();
				}
				break;
			case Difficulty.Hard:
				if (health > 0) {
					heart3.SetActive(false);
					health--;
					gameOver();
				}
				break;
		}
		health--;
	}
	
	public void DecreasePlayerTwoScore() {
		 if (GameplayManager.gameIsOver) {
			 return;
		 }
		 playerOneStreak = 0;
		 // TODO: -- Hide Player Two Send Deterrent UI
		 
		 Instantiate(minusSign, new Vector3(0.03999999f, 0.45f, -8.3f), Quaternion.identity);
		 switch(MainMenu.difficulty) {
			case Difficulty.Easy:
				if (otherHealth == 5) {
					otherHeart5.SetActive(false);
				} else if (otherHealth == 4) {
					otherHeart4.SetActive(false);
				} else if (otherHealth == 3) {
					otherHeart3.SetActive(false);
				} else if (otherHealth == 2) {
					otherHeart2.SetActive(false);
				} else if (otherHealth == 1) {
					otherHeart1.SetActive(false);
					otherHealth--;
					gameOver();
				}
				break;
			case Difficulty.Medium:
				if (otherHealth == 3) {
					otherHeart4.SetActive(false);
				} else if (otherHealth == 2) {
					otherHeart3.SetActive(false);
				} else if (otherHealth == 1) {
					otherHeart2.SetActive(false);
					otherHealth--;
					gameOver();
				}
				break;
			case Difficulty.Hard:
				if (otherHealth > 0) {
					otherHeart3.SetActive(false);
					otherHealth--;
					gameOver();
				}
				break;
		}
		otherHealth--;
	}
	
	
	public void gameOver()
	{
		if (!GameplayManager.gameIsOver) {
			GameplayManager.gameIsOver = true;
			nonMainMenuAudioManager.StopBackgroundMusic();
			audioManager.PlayGameOverSound();
			scoreCanvas.SetActive(false);
			otherScoreCanvas.SetActive(false);
			allHearts.SetActive(false);
			otherAllHearts.SetActive(false);
			GameOver.moveCanvasToStart = true;
			if (NetworkManager.isMultiplayer) {
				GameOver.moveOtherCanvasToStart = true;
			}
			if (health <= 0) {
				graveUpright.SetActive(false);
				graveDown.SetActive(true);
			}
			if (otherHealth <= 0) {
				otherGraveUpright.SetActive(false);
				otherGraveDown.SetActive(true);
			}
			networkVar.UpdateIsGameOver(true);
		}
	}
}
