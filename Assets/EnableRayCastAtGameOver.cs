using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Class that enables raycast at game over
/// </summary>
public class EnableRayCastAtGameOver : MonoBehaviour
{
    private NetworkVariablesAndReferences networkVar;
    private bool toggledGameOver;
    // Start is called before the first frame update
    void Start()
    {
        networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
        toggledGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!toggledGameOver && networkVar.isGameOver)
        {
            toggledGameOver = true;
            GetComponent<XRDirectInteractor>().enabled = false;
            GetComponentInChildren<XRRayInteractor>().enabled = true;
        }
    }
}
