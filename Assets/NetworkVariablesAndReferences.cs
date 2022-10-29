using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkVariablesAndReferences : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Keep reference of how many player have grabbed their basket. Start the game if the number
    /// of players garbbing is equals to the room capacity
    /// </summary>
    public int playerGrabbed = 0;

    /// <summary>
    /// Store the view ID of each network player prefab
    /// </summary>
    public int[] playerIDs = new int[2];

    /// <summary>
    /// Store the view ID of each network basket prefab
    /// </summary>
    public int[] basketIDs = new int[2];

    private int roomCapacity;
    private bool gameStarted = false;
    private Gameplay gameplay;
    private GameplayManager gameplayManager;
    private GameObject shadowBasket1;

    // Start is called before the first frame update
    void Start()
    {
        playerIDs[0] = -1;
        playerIDs[1] = -1;
        basketIDs[0] = -1;
        basketIDs[1] = -1;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomCapacity = PhotonNetwork.CurrentRoom.PlayerCount;
        gameplay = FindObjectOfType<Gameplay>();
        gameplayManager = FindObjectOfType<GameplayManager>();
        shadowBasket1 = GameObject.Find("Shadow Basket");
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

    [PunRPC]
    private void StartGameplay()
    {
        gameplayManager.enabled = true;
        gameplay.enabled = true;
        shadowBasket1.SetActive(false);
    }
}
