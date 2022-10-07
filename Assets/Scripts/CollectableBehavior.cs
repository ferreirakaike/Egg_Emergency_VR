using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.gameObject.transform.LookAt(player.transform);
        this.gameObject.transform.position += transform.forward * 3.0f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);

        if (other.gameObject.name.Equals("CollectionTool"))
        {
            // TODO: ADD POINTS -kferreira
        }

        Destroy(this.gameObject);
    }
}
