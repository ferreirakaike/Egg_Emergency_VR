using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class adds a main camera reference to the event camera of the canvas
/// </summary>
public class AddMainCameraToCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
