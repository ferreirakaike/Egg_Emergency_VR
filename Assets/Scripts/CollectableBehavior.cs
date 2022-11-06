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
    private static MeshRenderer _basket = null;
    private static MeshRenderer _rim = null;
    private NetworkVariablesAndReferences networkVar;

    private AudioManager _audioManager;
	private GameplayManager _gameplayManager;
    public Material defaultBasketMaterial;
    public Material defaultRimMaterial;
    public Material successBasketMaterial;
    public Material failureBasketMaterial;
    public Material successRimMaterial;
    public Material failureRimMaterial;
    public ParticleSystem explosion;
    public float timeBeforeResetMaterial = 0.3f;
    public int playerIndex = 0;
    private static float timePassed = 0;

    // keep track of whether the object has collided with the basket
    // this is to prevent collision with outter colliders after colliding with inner colliders
    private bool collided = false;


    // Start is called before the first frame update
    void Start()
    {
        networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
        _audioManager = GameObject.Find("SoundManager").GetComponent<AudioManager>();
		_gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();

        // view id might now be available at this time
        // set by gameplay with some logic
        if (networkVar.basketIDs[playerIndex] > 0)
        {
            _basket = PhotonView.Find(networkVar.basketIDs[playerIndex]).transform.GetChild(0).GetComponent<MeshRenderer>();
            _rim = _basket.transform.GetChild(0).GetComponent<MeshRenderer>();
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
            if (networkVar.basketIDs[playerIndex] > 0)
            {
                _basket = PhotonView.Find(networkVar.basketIDs[playerIndex]).transform.GetChild(0).GetComponent<MeshRenderer>();
                _rim = _basket.transform.GetChild(0).GetComponent<MeshRenderer>();
            }
        }
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag.Equals("Deterrent"))
        {
            Debug.Log("Collided with " + other.gameObject.tag);
        }
        if (!collided)
        {
            collided = true;
            if (other.gameObject.tag.Equals("InnerBasket") && gameObject.tag.Equals("Collectable"))
            {
                _audioManager.PlayCollectSound();
                _gameplayManager.IncreaseScore();
                _basket.material = successBasketMaterial;
                _rim.material = successRimMaterial;
                timePassed = 0;
            }
            else if (other.gameObject.tag.Equals("InnerBasket") && gameObject.tag.Equals("Deterrent"))
            {
                GameObject explo = Instantiate(explosion.gameObject, gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
                explo.SetActive(true);
                _basket.material = failureBasketMaterial;
                _rim.material = failureRimMaterial;
                _audioManager.PlayMissedSound();
                _gameplayManager.DecreaseScore();
            }
            else
            {
                if (gameObject.tag.Equals("Collectable"))
                {
                    _audioManager.PlayMissedSound();
                    _gameplayManager.DecreaseScore();
                    _basket.material = failureBasketMaterial;
                    _rim.material = failureRimMaterial;
                    timePassed = 0;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
