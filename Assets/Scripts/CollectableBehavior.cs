using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    //public GameObject GreenParticleGameObject;
    private GameObject _basket;
    private GameObject _redLight;
    private GameObject _greenLight;

    private AudioManager _audioManager;
	private GameplayManager _gameplayManager;

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.Find("SoundManager").GetComponent<AudioManager>();
		_gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        _basket = GameObject.Find("Basket");
        _redLight = _basket.transform.GetChild(2).gameObject;
        _greenLight = _basket.transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this.gameObject.transform.LookAt(player.transform);
        //this.gameObject.transform.position += transform.forward * 3.0f * Time.deltaTime;
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);
        if (other.gameObject.tag.Equals("InnerBasket") && gameObject.tag.Equals("Collectable"))
        {
            _audioManager.PlayCollectSound();
			_gameplayManager.IncreaseScore();
            _greenLight.SetActive(true);
        }
        else if (other.gameObject.tag.Equals("Basket"))
        {

        }
        else if(other.gameObject.tag.Equals("InnerBasket") && gameObject.tag.Equals("Deterrent"))
        {
            _audioManager.PlayMissedSound();
            _gameplayManager.DecreaseScore();
        }
        else
        {
            _audioManager.PlayMissedSound();
            _redLight.SetActive(true);
        }

        Destroy(this.gameObject);
    }
}
