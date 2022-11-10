using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// This class automatically scrolls through a scrollview in a determined amount of time.
/// </summary>
public class AutoScroll : MonoBehaviour
{
    /// <summary>
    /// The amount of time needed for the scrollview to be completely scrolled through.
    /// </summary>
    public float scrollDuration = 10;
    private ScrollRect scrollRect;

    /// <summary>
    /// Hold the reference text that should be displayed on the scrollview
    /// </summary>
    public static string textToScrollThrough = "";
    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.normalizedPosition = new Vector2(1,1);
        if (textToScrollThrough != "")
        {
            scrollRect.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = textToScrollThrough;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
    }

    private void Scroll()
    {
        if (scrollRect.normalizedPosition.y > 0.002)
        {
            Vector2 newScrollPosition = new Vector2(1, Mathf.MoveTowards(scrollRect.normalizedPosition.y, 0, Time.deltaTime * (1/scrollDuration)));
            scrollRect.normalizedPosition = newScrollPosition;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
