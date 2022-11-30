using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to rotate the heart at every update
/// </summary>
public class Rotate : MonoBehaviour
{
    /// <summary>
    /// User-defined rotation speed
    /// </summary>
    public float rotateSpeed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
