using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to set a destroy timer for objects
/// </summary>
public class DestroyAfterBlankFrames : MonoBehaviour
{
    /// <summary>
    /// Default number of frames before the object is deleted
    /// </summary>
    public float Frames;
    private float _timer = 0.0f;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        _timer++;
        if (_timer > Frames)
        {
            Destroy(this.gameObject);
        }
    }
}
