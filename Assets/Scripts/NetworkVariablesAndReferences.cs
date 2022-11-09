using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
    /// Hold the reference to game over state. Set by gameplay manager
    /// </summary>
    public bool isGameOver = false;

    private int roomCapacity;
    private bool gameStarted = false;
    private Gameplay gameplay;
    private GameplayManager gameplayManager;

    // Start is called before the first frame update
    void Start()
    {
        roomCapacity = PhotonNetwork.CurrentRoom.PlayerCount;
        gameplay = FindObjectOfType<Gameplay>();
        gameplayManager = FindObjectOfType<GameplayManager>();
        isGameOver = false;
    }

    // public override void OnJoinedRoom()
    // {
    //     base.OnJoinedRoom();
    //     roomCapacity = PhotonNetwork.CurrentRoom.PlayerCount;
    //     gameplay = FindObjectOfType<Gameplay>();
    //     gameplayManager = FindObjectOfType<GameplayManager>();
    //     isGameOver = false;
    // }

    void Reset()
    {
        gameStarted = false;    
    }

    // Update is called once per frame
    void Update()
    {
        // update rpc only if needed. Don't polute the data stream
        if (!gameStarted && roomCapacity > 0 && (roomCapacity == playerGrabbed))
        {
            gameStarted = true;
            photonView.RPC("StartGameplay", RpcTarget.AllBuffered);
            Debug.Log("Starting game");
        }
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
        PhotonNetwork.CurrentRoom.IsOpen = false;
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
    private void SyncIsGameOver(bool newData)
    {
        isGameOver = newData;
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
