using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class gradually increases the height of the parent object.
/// </summary>
public class FlyUp : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 vec = new Vector3(0.0f, 1.0f, 0.0f);
        this.gameObject.transform.position += vec * 3.0f * Time.deltaTime;
    }
}
