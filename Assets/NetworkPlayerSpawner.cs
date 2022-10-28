using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
  private GameObject spawnedPlayerPrefab;
  private GameObject spawnedBasketPrefab;
  public Transform[] playerSpawnLocations;
  public Transform[] basketSpawnLocations;

  public override void OnJoinedRoom()
  {
      base.OnJoinedRoom();
      if (PhotonNetwork.IsMasterClient)
      {
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", playerSpawnLocations[0].position, playerSpawnLocations[0].rotation);
        // spawnedBasketPrefab =PhotonNetwork.Instantiate("Basket", basketSpawnLocations[0].position, basketSpawnLocations[0].rotation);
        // spawnedBasketPrefab.transform.localScale = new Vector3(25,25,25);
      }
      else
      {
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", playerSpawnLocations[1].position, playerSpawnLocations[1].rotation);
        XROrigin origin = FindObjectOfType<XROrigin>();
        origin.transform.position = playerSpawnLocations[1].position;
        origin.transform.rotation = playerSpawnLocations[1].rotation;
        // spawnedBasketPrefab =PhotonNetwork.Instantiate("Basket", basketSpawnLocations[1].position, basketSpawnLocations[1].rotation);
        // spawnedBasketPrefab.transform.localScale = new Vector3(25,25,25);
      }
  }

  public override void OnLeftRoom()
  {
    base.OnLeftRoom();
    PhotonNetwork.Destroy(spawnedPlayerPrefab);
    PhotonNetwork.Destroy(spawnedBasketPrefab);
  }
}
