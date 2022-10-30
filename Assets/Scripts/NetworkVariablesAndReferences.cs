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
    /// Hold the reference to game over state. Set by gameplay manager
    /// </summary>
    public bool isGameOver = false;

    private int roomCapacity;
    private bool gameStarted = false;
    private Gameplay gameplay;
    private GameplayManager gameplayManager;
    private GameObject shadowBasket1;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomCapacity = PhotonNetwork.CurrentRoom.PlayerCount;
        gameplay = FindObjectOfType<Gameplay>();
        gameplayManager = FindObjectOfType<GameplayManager>();
        shadowBasket1 = GameObject.Find("Shadow Basket");
        isGameOver = false;
    }

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
    /// Function used to call RPC to sync variaables across the network
    /// </summary>
    /// <param name="newData">New Serializable object of type T</param>
    /// <param name="oldData">Old Serializable object of type T</param>
    /// <typeparam name="T">Serializable data types like int, float, object[], string, etc</typeparam>
    public void UpdateVariables<T>(T newData,ref T oldData)
    {
        photonView.RPC("SyncUpdatedVaraibles", RpcTarget.AllBuffered, new T[]{newData, oldData});
        Debug.Log("Syncing Variable");
    }

    [PunRPC]
    private void StartGameplay()
    {
        gameplayManager.enabled = true;
        gameplay.enabled = true;
        shadowBasket1.SetActive(false);
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    [PunRPC]
    private void SyncUpdatedVaraibles<T>(T newData,ref T oldData)
    {
        oldData = newData;
    }

    /// <summary>
    /// Send and receive data with the network via Photon View.
    /// Put all variables that need to be synced regularly here.
    /// Send and receive order must match
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
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
            // IsChanged<int>((int)stream.ReceiveNext(), playerGrabbed);
            // IsChanged<int>((int)stream.ReceiveNext(), playerIDs[0]);
            // IsChanged<int>((int)stream.ReceiveNext(), playerIDs[1]);
            // IsChanged<int>((int)stream.ReceiveNext(), basketIDs[0]);
            // IsChanged<int>((int)stream.ReceiveNext(), basketIDs[1]);
            // IsChanged<bool>((bool)stream.ReceiveNext(), isGameOver);
        }
    }

    private void IsChanged<T>(T newData, T oldData)
    {
        if (Comparer<T>.Default.Compare(newData, oldData) != 0)
        {
            oldData = newData;
        }
    }
}
