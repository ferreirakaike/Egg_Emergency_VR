using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;
using Photon.Realtime;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that spawns networked players after the transition to the main scene
/// </summary>
public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
  private GameObject spawnedPlayerPrefab;
  private GameObject spawnedBasketPrefab;
  private GameObject spawnedShadowBasketPrefab;

  /// <summary>
  /// Array of locations where players can spawn
  /// </summary>
  public Transform[] playerSpawnLocations;

  /// <summary>
  /// Array of locations where baskets can spawn
  /// </summary>
  public Transform[] basketSpawnLocations; 
  private NetworkVariablesAndReferences networkVar;

    void Start()
    {
        // If player is not connected to the server, send them back to main menu scene
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            AutoScroll.textToScrollThrough = "\n\n\n\n\n\n\n\n\n\n\nYou are not connected to the server.\n\nPlease try again!\n\n\n\n\n\n\n\n\n\n\n\n";
            SceneManager.LoadScene("OwnerLeftRoom");
        }
        networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
        if (PhotonNetwork.IsMasterClient)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", playerSpawnLocations[0].position, playerSpawnLocations[0].rotation);
            spawnedBasketPrefab = PhotonNetwork.Instantiate("Network Basket", basketSpawnLocations[0].position, basketSpawnLocations[0].rotation);
            spawnedShadowBasketPrefab = PhotonNetwork.Instantiate("Network Shadow Basket", basketSpawnLocations[0].position, basketSpawnLocations[0].rotation);
            spawnedBasketPrefab.transform.localScale = new Vector3(25,25,25);
            networkVar.UpdateBasketIDs(spawnedBasketPrefab.GetPhotonView().ViewID, 0);
            networkVar.UpdateShadowBasketIDs(spawnedShadowBasketPrefab.GetPhotonView().ViewID, 0);
            networkVar.UpdatePlayerIDs(spawnedPlayerPrefab.GetPhotonView().ViewID, 0);
        }
        else
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", playerSpawnLocations[1].position, playerSpawnLocations[1].rotation);
            XROrigin origin = FindObjectOfType<XROrigin>();
            origin.transform.position = playerSpawnLocations[1].position;
            origin.transform.rotation = playerSpawnLocations[1].rotation;
            spawnedBasketPrefab =PhotonNetwork.Instantiate("Network Basket", basketSpawnLocations[1].position, basketSpawnLocations[1].rotation);
            spawnedShadowBasketPrefab = PhotonNetwork.Instantiate("Network Shadow Basket", basketSpawnLocations[1].position, basketSpawnLocations[1].rotation);
            spawnedBasketPrefab.transform.localScale = new Vector3(25,25,25);
            networkVar.UpdateBasketIDs(spawnedBasketPrefab.GetPhotonView().ViewID, 1);
            networkVar.UpdateShadowBasketIDs(spawnedShadowBasketPrefab.GetPhotonView().ViewID, 1);
            networkVar.UpdatePlayerIDs(spawnedPlayerPrefab.GetPhotonView().ViewID, 1);
        }
        Debug.Log("Joined Room");
    }

    void Update()
    {
        if (networkVar.isGameOver && spawnedPlayerPrefab)
        {
            spawnedPlayerPrefab.SetActive(false);
        }
    }

    /// <summary>
    /// Override parent method. This method destroys the player prefabs in the game scene
    /// </summary>
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        GameObject photonVoiceClient = GameObject.Find("PhotonVoice");
        Destroy(photonVoiceClient);
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
        PhotonNetwork.Destroy(spawnedBasketPrefab);
    }

    /// <summary>
    /// Override parent method. This method forces the second player to leave the room if the room creator left.
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        PhotonNetwork.LeaveRoom();
        AutoScroll.textToScrollThrough = "\n\n\n\n\n\n\n\n\n\n\nOnver Left Room!\n\nPlease create\n\nor\n\nJoin a diffrernt room!\n\n\n\n\n\n\n\n\n\n\n\n\n";
        SceneManager.LoadScene("OwnerLeftRoom");
    }
}
