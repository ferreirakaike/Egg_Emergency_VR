using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterSeconds : MonoBehaviour
{
    // Disable this object after x seconds
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
