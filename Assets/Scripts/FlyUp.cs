using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 vec = new Vector3(0.0f, 1.0f, 0.0f);
        this.gameObject.transform.position += vec * 3.0f * Time.deltaTime;
    }
}
