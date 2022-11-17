using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Change scorekeeping multiplier
// Record video
// Weekly report

public class CollectableBehavior : MonoBehaviourPunCallbacks
{
    //public GameObject GreenParticleGameObject;
    private MeshRenderer _basket = null;
    private MeshRenderer _rim = null;
    private NetworkVariablesAndReferences networkVar;

    private AudioManager _audioManager;
	private GameplayManager _gameplayManager;
    public Material defaultBasketMaterial;
    public Material defaultRimMaterial;
    public Material successBasketMaterial;
    public Material failureBasketMaterial;
    public Material successRimMaterial;
    public Material failureRimMaterial;
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
        if (!collided)
        {
            collided = true;
            if (other.gameObject.tag.Equals("InnerBasket") && gameObject.tag.Equals("Collectable"))
            {
                _basket.material = successBasketMaterial;
                _rim.material = successRimMaterial;
				
				if (playerIndex == 0 && PhotonNetwork.IsMasterClient && gameObject.transform.position.x <= 3.0f)
                {
                    timePassed = 0;
                    _audioManager.PlayCollectSound();
					int amount = _gameplayManager.calculateIncreaseScore();
					//_gameplayManager.IncreasePlayerOneScore(amount);
					networkVar.UpdateIncreaseScore(amount, 0);
                }
                else if (playerIndex == 1 && !PhotonNetwork.IsMasterClient && gameObject.transform.position.x >= 3.0f)
                {
                    timePassed = 0;
                    _audioManager.PlayCollectSound();
					int amount = _gameplayManager.calculateIncreaseScore();
					//_gameplayManager.IncreasePlayerTwoScore(amount);
					networkVar.UpdateIncreaseScore(amount, 1);
                }
            }
            else if (other.gameObject.tag.Equals("InnerBasket") && gameObject.tag.Equals("Deterrent"))
            {
                GameObject explo = PhotonNetwork.Instantiate("ExplosionEffect", gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
                explo.SetActive(true);
                _basket.material = failureBasketMaterial;
                _rim.material = failureRimMaterial;
				
				if (playerIndex == 0 && PhotonNetwork.IsMasterClient && gameObject.transform.position.x <= 3.0f)
                {
                    _audioManager.PlayBombSound();
					//_gameplayManager.DecreasePlayerOneScore();
					networkVar.UpdateDecreaseScore(0);
                }
                else if (playerIndex == 1 && !PhotonNetwork.IsMasterClient && gameObject.transform.position.x >= 3.0f)
                {
                    _audioManager.PlayBombSound();
					//_gameplayManager.DecreasePlayerTwoScore();
					networkVar.UpdateDecreaseScore(1);
                }
            }
            else
            {
                if (gameObject.tag.Equals("Collectable"))
                {
                    _basket.material = failureBasketMaterial;
                    _rim.material = failureRimMaterial;
					
					if (playerIndex == 0 && PhotonNetwork.IsMasterClient && gameObject.transform.position.x <= 3.0f)
                    {
                        timePassed = 0;
                        _audioManager.PlayMissedSound();
						//_gameplayManager.DecreasePlayerOneScore();
						networkVar.UpdateDecreaseScore(0);
                    }
                    else if (playerIndex == 1 && !PhotonNetwork.IsMasterClient && gameObject.transform.position.x >= 3.0f)
                    {
                        timePassed = 0;
                        _audioManager.PlayMissedSound();
						//_gameplayManager.DecreasePlayerTwoScore();
						networkVar.UpdateDecreaseScore(1);
                    }
                }
            }
            // Master / Client destroy their own object
            if (playerIndex == 1 && !PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
            else if (playerIndex == 0 && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Override parent method. This method destroys the collectable that corresponds to the leaving player
    /// </summary>
    public override void OnPlayerLeftRoom(Player player)
    {
        base.OnPlayerLeftRoom(player);
        if(!player.IsMasterClient && playerIndex == 1)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

}
