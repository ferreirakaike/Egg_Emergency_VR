using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Change scorekeeping multiplier
// Record video
// Weekly report

public class CollectableBehavior : MonoBehaviour
{
    //public GameObject GreenParticleGameObject;
    private MeshRenderer _basket = null;
    private MeshRenderer _rim = null;
    private NetworkVariablesAndReferences networkVar;

    private AudioManager _audioManager;
	private GameplayManager _gameplayManager;
    private Material defaultBasketMaterial;
    private Material defaultRimMaterial;
    public Material successBasketMaterial;
    public Material failureBasketMaterial;
    public Material successRimMaterial;
    public Material failureRimMaterial;
    public float timeBeforeResetMaterial = 0.5f;
    private static float timePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
        _audioManager = GameObject.Find("SoundManager").GetComponent<AudioManager>();
		_gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();

        // view id might now be available at this time
        if (networkVar.basketIDs[0] > 0)
        {
            _basket = PhotonView.Find(networkVar.basketIDs[0]).transform.GetChild(0).GetComponent<MeshRenderer>();
            _rim = _basket.transform.GetChild(0).GetComponent<MeshRenderer>();
            defaultBasketMaterial = _basket.material;
            defaultRimMaterial = _rim.material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_basket)
        {
            if (_basket.material != defaultBasketMaterial)
            {
                timePassed += Time.deltaTime;
                if (timePassed > timeBeforeResetMaterial)
                {
                    _basket.material = defaultBasketMaterial;
                    _rim.material = defaultRimMaterial;
                }
            }
        }
        else
        {
            // view id might now be available at this time
            if (networkVar.basketIDs[0] > 0)
            {
                _basket = PhotonView.Find(networkVar.basketIDs[0]).transform.GetChild(0).GetComponent<MeshRenderer>();
                _rim = _basket.transform.GetChild(0).GetComponent<MeshRenderer>();
                defaultBasketMaterial = _basket.material;
                defaultRimMaterial = _rim.material;
            }
        }
        
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);
        if (other.gameObject.tag.Equals("InnerBasket") && gameObject.tag.Equals("Collectable"))
        {
            _audioManager.PlayCollectSound();
            _gameplayManager.IncreaseScore();
            // _greenLight.SetActive(true);
            _basket.material = successBasketMaterial;
            _rim.material = successRimMaterial;
            timePassed = 0;
        }
        /*else if (other.gameObject.tag.Equals("Basket"))
        {

        }*/
        else if (other.gameObject.tag.Equals("InnerBasket") && gameObject.tag.Equals("Deterrent"))
        {
            _basket.material = failureBasketMaterial;
            _rim.material = failureRimMaterial;
            timePassed = 0;
            _audioManager.PlayMissedSound();
            _gameplayManager.DecreaseScore();
        }
        else
        {
            if (gameObject.tag.Equals("Collectable"))
            {
                _audioManager.PlayMissedSound();
                // _redLight.SetActive(true);
                _gameplayManager.DecreaseScore();
                _basket.material = failureBasketMaterial;
                _rim.material = failureRimMaterial;
                timePassed = 0;
            }
        }

        Destroy(this.gameObject);
    }
}
