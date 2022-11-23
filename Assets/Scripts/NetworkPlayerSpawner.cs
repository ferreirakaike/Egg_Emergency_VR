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
  private GameObject spawnedTombstonePrefab;

  /// <summary>
  /// Array of locations where players can spawn.
  /// </summary>
  public Transform[] playerSpawnLocations;

  /// <summary>
  /// Array of locations where baskets can spawn.
  /// </summary>
  public Transform[] basketSpawnLocations; 

  /// <summary>
  /// Array of locations where gamescore/tombstone can spawn.
  /// </summary>
  public Transform[] tombstoneSpawnLocations;
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
      spawnedTombstonePrefab = PhotonNetwork.Instantiate("Game Score", tombstoneSpawnLocations[0].position, tombstoneSpawnLocations[0].rotation);
      networkVar.UpdateBasketIDs(spawnedBasketPrefab.GetPhotonView().ViewID, 0);
      networkVar.UpdateShadowBasketIDs(spawnedShadowBasketPrefab.GetPhotonView().ViewID, 0);
      networkVar.UpdatePlayerIDs(spawnedPlayerPrefab.GetPhotonView().ViewID, 0);
      networkVar.UpdateTombstoneIDs(spawnedTombstonePrefab.GetPhotonView().ViewID, 0);
      if (!NetworkManager.isMultiplayer)
      {
        spawnedTombstonePrefab.transform.Find("Deterrent_Bomb").gameObject.SetActive(false);
      }
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
      spawnedTombstonePrefab = PhotonNetwork.Instantiate("Game Score", tombstoneSpawnLocations[1].position, tombstoneSpawnLocations[1].rotation);
      networkVar.UpdateBasketIDs(spawnedBasketPrefab.GetPhotonView().ViewID, 1);
      networkVar.UpdateShadowBasketIDs(spawnedShadowBasketPrefab.GetPhotonView().ViewID, 1);
      networkVar.UpdatePlayerIDs(spawnedPlayerPrefab.GetPhotonView().ViewID, 1);
      networkVar.UpdateTombstoneIDs(spawnedTombstonePrefab.GetPhotonView().ViewID, 1);
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
    if (spawnedPlayerPrefab)
    {
      PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
    if (spawnedBasketPrefab)
    {
      PhotonNetwork.Destroy(spawnedBasketPrefab);
    }
    if (spawnedShadowBasketPrefab)
    {
      PhotonNetwork.Destroy(spawnedShadowBasketPrefab);
    }
    if (spawnedTombstonePrefab)
    {
      PhotonNetwork.Destroy(spawnedTombstonePrefab);
    }
  }

  /// <summary>
  /// Override parent method. This method forces the second player to leave the room if the room creator left.
  /// </summary>
  /// <param name="newMasterClient"></param>
  public override void OnPlayerLeftRoom(Player player)
  {
    base.OnPlayerLeftRoom(player);
    networkVar.UpdateIsGameOver(true);
  }
}
