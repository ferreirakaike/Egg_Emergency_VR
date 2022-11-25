using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


/// <summary>
/// Class that holds everything and anything networked. This class keeps variables that others can reference.
/// This class also sync variable changes across the network.
/// </summary>
public class NetworkVariablesAndReferences : MonoBehaviourPunCallbacks, IPunObservable
{
    /// <summary>
    /// Keep reference of how many player have grabbed their basket. Start the game if the number
    /// of players garbbing is equals to the room capacity
    /// </summary>
    public int playerGrabbed = 0;

    /// <summary>
    /// Store the view ID of each network player prefab
    /// </summary>
    public int[] playerIDs = {-1, -1};

    /// <summary>
    /// Store the view ID of each network basket prefab
    /// </summary>
    public int[] basketIDs = {-1, -1};

    /// <summary>
    /// Store the view ID of each network shadow basket prefab
    /// </summary>
    public int[] shadowBasketIDs = {-1, -1};

    /// <summary>
    /// Store the view ID of each network gamescore/tombstone prefab
    /// </summary>
    public int[] tombstoneIDs = {-1, -1};

    /// <summary>
    /// Hold the reference to game over state. Set by gameplay manager
    /// </summary>
    public bool isGameOver = false;

    private int roomCapacity;
    private bool gameStarted = false;
    private Gameplay gameplay;
    private GameplayManager gameplayManager;
    private TextMeshProUGUI[] countDown;
    private int localPlayerIndex = 0;
    private int otherPlayerIndex = 1;

    void Awake()
    {
        // if it gets laggy, decrease these
        // default sendRate = 30 times/sec
        // default serializationrate = 10 times/sec
        PhotonNetwork.SerializationRate = 10;
        PhotonNetwork.SendRate = 45;
        gameStarted = false;
        isGameOver = false;
        Debug.Log("Setting photon send rate");
    }

    // Start is called before the first frame update
    void Start()
    {
        roomCapacity = PhotonNetwork.CurrentRoom.MaxPlayers;
        gameplay = FindObjectOfType<Gameplay>();
        gameplayManager = FindObjectOfType<GameplayManager>();
        if (gameplayManager == null)
        {
            gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        }
        isGameOver = false;
        
        if(PhotonNetwork.IsMasterClient && NetworkManager.isMultiplayer)
        {
            photonView.RPC("SyncIsMultiplayer", RpcTarget.AllBuffered, NetworkManager.isMultiplayer);
        }
        if (PhotonNetwork.IsMasterClient)
		{
			localPlayerIndex = 0;
			otherPlayerIndex = 1;
		}
		else
		{
			localPlayerIndex = 1;
			otherPlayerIndex = 0;
		}
        countDown = new TextMeshProUGUI[2];
        GameObject tombstone = PhotonView.Find(tombstoneIDs[localPlayerIndex]).gameObject;
        countDown[localPlayerIndex]  = tombstone.transform.Find("Canvas").Find("Count Down Value Label").GetComponent<TextMeshProUGUI>();
    }

    void Reset()
    {
        gameStarted = false;
        isGameOver = false;    
    }

    // Update is called once per frame
    void Update()
    {
        // update rpc only if needed. Don't polute the data stream
        if (!gameStarted)
        {
            roomCapacity = PhotonNetwork.CurrentRoom.MaxPlayers;
            if(roomCapacity > 0 && (roomCapacity == playerGrabbed))
            {
                gameStarted = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    StartCoroutine(StartCountDown());
                }
            }
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && tombstoneIDs[otherPlayerIndex] != -1 && !countDown[otherPlayerIndex])
		{
			GameObject otherTombstone = PhotonView.Find(tombstoneIDs[otherPlayerIndex]).gameObject;
			countDown[otherPlayerIndex]  = otherTombstone.transform.Find("Canvas").Find("Count Down Value Label").GetComponent<TextMeshProUGUI>();
		}
    }

    [PunRPC]
	private void SyncCountDown(bool toggleDisable, int number, int playerIndex)
	{
        countDown[playerIndex].gameObject.SetActive(!toggleDisable);
		countDown[playerIndex].text = $"{number}";
	}


    IEnumerator StartCountDown()
    {
        int count = 3;
        while (count >= 0)
        {
            photonView.RPC("SyncCountDown", RpcTarget.All, false, count, 0);
            if (roomCapacity == 2)
            {
                photonView.RPC("SyncCountDown", RpcTarget.All, false, count, 1);
            }
            count--;
            yield return new WaitForSeconds(1);
        }
        photonView.RPC("SyncCountDown", RpcTarget.All, true, count, 0);
        if (roomCapacity == 2)
        {
            photonView.RPC("SyncCountDown", RpcTarget.All, true, count, 1);
        }
        photonView.RPC("StartGameplay", RpcTarget.All);
        Debug.Log("Starting game");
    }

    /// <summary>
    /// Update the number of players who have grabbed their basket.
    /// Use as a counter to know when to start game.
    /// Game is started when the number of player grabbed is the same as room capacity.
    /// </summary>
    public void UpdatePlayerGrabbed()
    {
        photonView.RPC("SyncPlayerGrabbed", RpcTarget.AllBuffered);
        Debug.Log("Syncing # Player Grabbed");
    }

    /// <summary>
    /// Update the player photonview ID
    /// </summary>
    /// <param name="newData">Photonview ID of the player</param>
    /// <param name="playerIndex">Player index. 0 is Master, 1 is client</param>
    public void UpdatePlayerIDs(int newData, int playerIndex)
    {
        photonView.RPC("SyncPlayerIDs", RpcTarget.AllBuffered, newData, playerIndex);
        Debug.Log("Syncing Player IDs");
    }

    /// <summary>
    /// Update the basket photonview ID
    /// </summary>
    /// <param name="newData">Photonview ID of the basket</param>
    /// <param name="playerIndex">Player index. 0 is Master, 1 is client</param>
    public void UpdateBasketIDs(int newData, int playerIndex)
    {
        photonView.RPC("SyncBasketIDs", RpcTarget.AllBuffered, newData, playerIndex);
        Debug.Log("Syncing Basket IDs");
    }

    /// <summary>
    /// Update the shadow basket photonview ID
    /// </summary>
    /// <param name="newData">Photonview ID of the basket</param>
    /// <param name="playerIndex">Player index. 0 is Master, 1 is client</param>
    public void UpdateShadowBasketIDs(int newData, int playerIndex)
    {
        photonView.RPC("SyncShadowBasketIDs", RpcTarget.AllBuffered, newData, playerIndex);
        Debug.Log("Syncing Basket IDs");
    }

    /// <summary>
    /// Update the tombstone photonview ID
    /// </summary>
    /// <param name="newData">Photonview ID of the tombstone</param>
    /// <param name="playerIndex">Player index. 0 is Master, 1 is client</param>
    public void UpdateTombstoneIDs(int newData, int playerIndex)
    {
        photonView.RPC("SyncTombstoneIDs", RpcTarget.AllBuffered, newData, playerIndex);
        Debug.Log("Syncing Tombstone IDs");
    }

    /// <summary>
    /// Update the game over boolean
    /// </summary>
    /// <param name="newData">Game over state</param>
    public void UpdateIsGameOver(bool newData)
    {
        photonView.RPC("SyncIsGameOver", RpcTarget.AllBuffered, newData);
        Debug.Log("Syncing isGameOver");
    }

    [PunRPC]
    private void StartGameplay()
    {
        gameplayManager.enabled = true;
        gameplay.enabled = true;
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    [PunRPC]
    private void SyncPlayerIDs(int newData, int playerIndex)
    {
        playerIDs[playerIndex] = newData;
    }

    [PunRPC]
    private void SyncBasketIDs(int newData, int playerIndex)
    {
        basketIDs[playerIndex] = newData;
    }

    [PunRPC]
    private void SyncShadowBasketIDs(int newData, int playerIndex)
    {
        shadowBasketIDs[playerIndex] = newData;
    }

    [PunRPC]
    private void SyncTombstoneIDs(int newData, int playerIndex)
    {
        tombstoneIDs[playerIndex] = newData;
    }

    [PunRPC]
    private void SyncIsGameOver(bool newData)
    {
        isGameOver = newData;
        if (isGameOver)
        {
            gameplayManager.gameOver();
        }
    }

    [PunRPC]
    private void SyncIsMultiplayer(bool newData)
    {
        NetworkManager.isMultiplayer = newData;
    }

    [PunRPC]
    private void SyncPlayerGrabbed()
    {
        playerGrabbed++;
    }

    /// <summary>
    /// Send and receive data with the network via Photon View.
    /// Put all variables that need to be synced regularly here.
    /// Send and receive order must match
    /// </summary>
    /// <param name="stream">In/Out datastream</param>
    /// <param name="info">General message information like sent and received time</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // stream.SendNext(playerGrabbed);
            // stream.SendNext(playerIDs[0]);
            // stream.SendNext(playerIDs[1]);
            // stream.SendNext(basketIDs[0]);
            // stream.SendNext(basketIDs[1]);
            // stream.SendNext(isGameOver);
        }
        else if (stream.IsReading)
        {
            // IsChanged<int>((int)stream.ReceiveNext(),ref playerGrabbed);
            // IsChanged<int>((int)stream.ReceiveNext(),ref playerIDs[0]);
            // IsChanged<int>((int)stream.ReceiveNext(),ref playerIDs[1]);
            // IsChanged<int>((int)stream.ReceiveNext(),ref basketIDs[0]);
            // IsChanged<int>((int)stream.ReceiveNext(),ref basketIDs[1]);
            // IsChanged<bool>((bool)stream.ReceiveNext(),ref isGameOver);
        }
    }

    private void IsChanged<T>(T newData, ref T oldData)
    {
        if (Comparer<T>.Default.Compare(newData, oldData) != 0)
        {
            oldData = newData;
        }
    }
}
