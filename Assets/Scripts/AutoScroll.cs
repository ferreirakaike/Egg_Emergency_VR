using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AutoScroll : MonoBehaviour
{
    public float scrollDuration = 10;
    private ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.normalizedPosition = new Vector2(1,1);
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
