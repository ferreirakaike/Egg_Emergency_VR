using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// This class handles the storing and fetching of leaderboard scores for local user.
/// </summary>
public class LeaderboardHandler : MonoBehaviour
{
  private static List<int> leaderboardScores = null;
  private GameObject leaderboard;
  // Awake is called before the first frame update
  void Awake()
  {
    StartCoroutine(FetchAndSortScores());
  }

  IEnumerator FetchAndSortScores()
  {
    yield return new WaitForSeconds(0);
    if (leaderboardScores == null)
    {
      leaderboardScores = new List<int>();
    }
    if (leaderboardScores.Count == 0)
    {
      for (int i = 0; i < 5; i++)
      {
      leaderboardScores.Add(PlayerPrefs.GetInt("Score" + i.ToString(), 0));
      }
      leaderboardScores.Sort();
      leaderboardScores.Reverse();
    }
    
    leaderboard = transform.Find("Leaderboard").gameObject;
    string text = "";
    for (int i = 0; i < 5; i++)
    {
      text += (i + 1).ToString() + ".\t" + leaderboardScores[i].ToString();
      if (i != 4)
      {
        text += "\n\n";
      }
    }
    leaderboard.GetComponent<TextMeshProUGUI>().text = text;
  }

  /// <summary>
  /// Static method that check if a new score is within the top 5 highest scores. If it is, then add it to the top 5 scores
  /// </summary>
  /// <param name="newScore">This is the new score that the user supply to the method at gameover in Single Player mode</param>
  public static void UpdateLeaderboardScores(int newScore)
  {
    leaderboardScores.Add(newScore);
    leaderboardScores.Sort();
    leaderboardScores.Reverse();
    if (leaderboardScores.Count > 0)
    {
      leaderboardScores.RemoveAt(leaderboardScores.Count - 1);
    }
    for (int i = 0; i < leaderboardScores.Count; i++)
    {
      PlayerPrefs.SetInt("Score" + i.ToString(), leaderboardScores[i]);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
