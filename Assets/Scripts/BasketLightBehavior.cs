using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketLightBehavior : MonoBehaviour
{
    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer++;

        if (_timer > 20)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _timer = 0;
    }
}
