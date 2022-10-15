using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
	public TextMeshProUGUI scoreText;
	public int score = 0;
	
    // Start is called before the first frame update
    void Start()
    {
		scoreText.text = $"{score}";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
	
	public void IncreaseScore()
	{
		score += 1;
		scoreText.text = $"{score}";
	}
}
