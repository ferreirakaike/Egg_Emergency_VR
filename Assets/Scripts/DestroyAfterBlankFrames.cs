using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterBlankFrames : MonoBehaviour
{
    public float Frames;
    private float _timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
