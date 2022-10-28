using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GameplayManager : MonoBehaviour
{
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
	
    // Start is called before the first frame update
    void Start()
    {
			basket = GameObject.Find("/Basket").GetComponent<TwoHandGrabInteractable>();
			scoreText.text = $"{score}";
			
			switch(MainMenu.difficulty) {
				case Difficulty.Easy:
					health = 5;
					break;
				case Difficulty.Medium:
					heart1.SetActive(false);
					heart5.SetActive(false);
					health = 3;
					break;
				case Difficulty.Hard:
					heart1.SetActive(false);
					heart2.SetActive(false);
					heart4.SetActive(false);
					heart5.SetActive(false);
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
		scoreCanvas.SetActive(false);
		allHearts.SetActive(false);
		GameOver.moveCanvasToStart = true;
		GameplayManager.gameIsOver = true;
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
