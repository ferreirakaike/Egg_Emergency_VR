using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
	public TextMeshProUGUI scoreText;
	public int score = 0;
	public int health = 0;
	
	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;
	public GameObject heart4;
	public GameObject heart5;
	public GameObject minusSign;
	
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
	
	public void IncreaseScore()
	{
		float percentage = basket.transform.localScale.x / basket.maxScale;
		int scoreIncrease = (int)(1f + (5f * (1f - percentage)));
		score += scoreIncrease;
		scoreText.text = $"{score}";
	}

 	 public void DecreaseScore()
	 {
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
		
	}
}
