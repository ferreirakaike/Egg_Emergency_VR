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
	
	public TextMeshProUGUI scoreText;
	public static int score = 0;
	public int health = 0;
	public static bool gameIsOver = false;
	public List<GameObject> rayInteractors;
	
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
	
	private TwoHandGrabInteractable basket;
	private NetworkVariablesAndReferences networkVar;
	
	/// <summary>
	/// Override the on enable method of MonoBehaviourPunCallbacks.
	/// Instantiates necessary variables
	/// </summary>
	public override void OnEnable()
	{
		base.OnEnable();
		networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
		basket = PhotonView.Find(networkVar.basketIDs[0]).GetComponent<TwoHandGrabInteractable>();
		score = 0;
		gameIsOver = false;
		scoreText.text = $"{score}";
		
		switch(MainMenu.difficulty) {
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
		foreach(var rayInteractor in rayInteractors)
		{
			rayInteractor.SetActive(false);
		}
	}
	
    // Start is called before the first frame update
    void Start()
    {
			audioManager = GameObject.Find("UISoundManager").GetComponent<MainMenuAudioManager>();
			networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
			basket = PhotonView.Find(networkVar.basketIDs[0]).GetComponent<TwoHandGrabInteractable>();
			score = 0;
			gameIsOver = false;
			scoreText.text = $"{score}";
			
			switch(MainMenu.difficulty) {
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
			foreach(var rayInteractor in rayInteractors)
			{
				rayInteractor.SetActive(false);
			}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
	
	public void IncreaseScore()
	{
		 if (GameplayManager.gameIsOver) {
			 return;
		 }
		float percentage = basket.transform.localScale.x / basket.maxScale;
		int scoreIncrease = (int)(1f + (5f * (1f - percentage)));
		GameplayManager.score += scoreIncrease;
		scoreText.text = $"{GameplayManager.score}";
	}

 	 public void DecreaseScore()
	 {
		 if (GameplayManager.gameIsOver) {
			 return;
		 }
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
					gameOver();
				}
				break;
			case Difficulty.Hard:
				if (health > 0) {
					heart3.SetActive(false);
					gameOver();
				}
				break;
		}
		health--;
	}
	
	public void gameOver()
	{
		audioManager.PlayGameOverSound();
		scoreCanvas.SetActive(false);
		allHearts.SetActive(false);
		GameOver.moveCanvasToStart = true;
		GameplayManager.gameIsOver = true;
		networkVar.UpdateIsGameOver(true);
		graveUpright.SetActive(false);
		graveDown.SetActive(true);

		foreach(var rayInteractor in rayInteractors)
		{
			rayInteractor.SetActive(true);
			rayInteractor.GetComponentInParent<XRDirectInteractor>().enabled = false;
			// disbale hand prefab
			rayInteractor.transform.parent.GetChild(0).gameObject.SetActive(false);
		}
	}
}
