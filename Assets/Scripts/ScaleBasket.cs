using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBasket : MonoBehaviour
{
    public bool grabbedWithBothHands = false;
    public GameObject controller1;
    public GameObject controller2;
    public float minScale = 20f;
    public float maxScale = 44f;

    private float oldScaleMultiplier;
    private float constantZScale;
    // Start is called before the first frame update
    void Start()
    {
        grabbedWithBothHands = false;
        oldScaleMultiplier = this.transform.localScale.x;
        constantZScale = this.transform.localScale.z;
        if (!controller1)
        {
            Debug.LogError("Failed to get controller 1");
        }
        if (!controller2)
        {
            Debug.LogError("Failed to get controller 2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controller1 && controller2 && grabbedWithBothHands)
        {
            float distance = (oldScaleMultiplier + 10) * Vector3.Distance(controller1.transform.position, controller2.transform.position);
            Debug.Log("Old distance " + distance.ToString());
            if (distance < minScale)
            {
                distance = minScale;
            }
            else if (distance > maxScale)
            {
                distance = maxScale;
            }
            this.transform.position = Vector3.Lerp(controller1.transform.position, controller2.transform.position, 0.5f);
            this.transform.localScale = new Vector3(distance, distance, constantZScale);
        }
        else
        {
            this.transform.localScale = new Vector3(oldScaleMultiplier, oldScaleMultiplier, constantZScale);
        }
    }
}