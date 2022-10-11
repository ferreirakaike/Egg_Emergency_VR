using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    public GameObject AudioManagerGameObject;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.Find("SoundManager").GetComponent<AudioManager>();
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

        if (other.gameObject.tag.Equals("Basket"))
        {
            _audioManager.PlayCollectSound();
            
            // TODO: ADD POINTS -kferreira
        }
        else if (other.gameObject.tag.Equals("InvisibleWall"))
        {
            _audioManager.PlayMissedSound();
        }

        Destroy(this.gameObject);
    }
}
