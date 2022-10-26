using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
	public TextMeshProUGUI scoreText;
	public int score = 0;
	
	private TwoHandGrabInteractable basket;
	
    // Start is called before the first frame update
    void Start()
    {
		basket = GameObject.Find("/Basket").GetComponent<TwoHandGrabInteractable>();
		scoreText.text = $"{score}";
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
		score -= 1;
		scoreText.text = $"{score}";
	}
}
