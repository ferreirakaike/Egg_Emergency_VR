using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBasket : MonoBehaviour
{
    public bool grabbedWithBothHands = false;
    public GameObject controller1;
    public GameObject controller2;
    public float minScale = 10f;
    public float maxScale = 50f;

    private float oldScaleMultiplier;
    private float constantZScale;
    private float angleAtController;
    private float basketAngle;
    private float initialDistanceToController;
    private float initialDistanceBetweenControllers;
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
        Vector3 controllerToBasketVector = controller1.transform.position - this.transform.position;
        Vector3 basketToController2Vector = this.transform.position - controller2.transform.position;
        Vector3 controller2ToController1Vector = controller2.transform.position - controller1.transform.position;
        basketAngle = Vector2.Angle(new Vector2(controllerToBasketVector.x, controllerToBasketVector.z), new Vector2(basketToController2Vector.x, basketToController2Vector.z));
        angleAtController = Vector2.Angle(new Vector2(controller2ToController1Vector.x, controller2ToController1Vector.z), new Vector2(controllerToBasketVector.x, controllerToBasketVector.z));
        initialDistanceToController = Vector2.Distance(new Vector2(controller1.transform.position.x, controller1.transform.position.z), new Vector2(this.transform.position.x, this.transform.position.z));
        initialDistanceBetweenControllers = Vector2.Distance(new Vector2(controller1.transform.position.x,controller1.transform.position.z), new Vector2(controller2.transform.position.x, controller2.transform.position.z));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controller1 && controller2 && grabbedWithBothHands)
        {
            // find distance between controllers
            float controllerDistance = Vector2.Distance(new Vector2(controller1.transform.position.x, controller1.transform.position.z), new Vector2(controller2.transform.position.x, controller2.transform.position.z));
            float newDistanceControllerBasket = controllerDistance / Mathf.Cos(angleAtController);
            float newScale = (newDistanceControllerBasket / initialDistanceToController) * oldScaleMultiplier;

            // float newScale = 38 * controllerDistance;
            Debug.Log("New Scale " + newScale.ToString());
            if (newScale < minScale)
            {
                newScale = minScale;
            }
            else if (newScale > maxScale)
            {
                newScale = maxScale;
            }
            this.transform.position = Vector3.Lerp(controller1.transform.position, controller2.transform.position, 0.5f);
            this.transform.localScale = new Vector3(newScale, newScale, constantZScale);
        }
        // else
        // {
        //     this.transform.localScale = new Vector3(oldScaleMultiplier, oldScaleMultiplier, constantZScale);
        // }
    }
}