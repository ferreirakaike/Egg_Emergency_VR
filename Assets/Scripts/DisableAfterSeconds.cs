using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class disables an object after it has been active for a set amount of time
/// </summary>
public class DisableAfterSeconds : MonoBehaviour
{
    /// <summary>
    /// User-defined value that disables the current object after x seconds.
    /// </summary>
    public float disableThisObjectAfter = 10f;
    private float timePassed = 0;
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > disableThisObjectAfter)
        {
            gameObject.SetActive(false);
        }
    }

}
