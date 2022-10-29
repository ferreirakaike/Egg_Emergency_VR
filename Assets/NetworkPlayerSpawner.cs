using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
  private GameObject spawnedPlayerPrefab;
  private GameObject spawnedBasketPrefab;
  public Transform[] playerSpawnLocations;
  public Transform[] basketSpawnLocations;
  private NetworkVariablesAndReferences networkVar;

    void Start()
    {
        networkVar = GameObject.Find("Network Interaction Statuses").GetComponent<NetworkVariablesAndReferences>();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("HERE");
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("HERE");
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", playerSpawnLocations[0].position, playerSpawnLocations[0].rotation);
            spawnedBasketPrefab =PhotonNetwork.Instantiate("Network Basket", basketSpawnLocations[0].position, basketSpawnLocations[0].rotation);
            spawnedBasketPrefab.transform.localScale = new Vector3(25,25,25);
            Debug.Log(networkVar.basketIDs[0].ToString());
            networkVar.basketIDs[0] = spawnedBasketPrefab.GetPhotonView().ViewID;
            Debug.Log(spawnedBasketPrefab.GetPhotonView().ViewID.ToString());
            networkVar.playerIDs[0] = spawnedPlayerPrefab.GetPhotonView().ViewID;
            Debug.Log(networkVar.basketIDs[0].ToString());
        }
        else
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", playerSpawnLocations[1].position, playerSpawnLocations[1].rotation);
            XROrigin origin = FindObjectOfType<XROrigin>();
            origin.transform.position = playerSpawnLocations[1].position;
            origin.transform.rotation = playerSpawnLocations[1].rotation;
            spawnedBasketPrefab =PhotonNetwork.Instantiate("Network Basket", basketSpawnLocations[1].position, basketSpawnLocations[1].rotation);
            spawnedBasketPrefab.transform.localScale = new Vector3(25,25,25);
            networkVar.basketIDs[1] = spawnedBasketPrefab.GetPhotonView().ViewID;
            networkVar.playerIDs[1] = spawnedPlayerPrefab.GetPhotonView().ViewID;
        }
        Debug.Log("Joined Room");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
        PhotonNetwork.Destroy(spawnedBasketPrefab);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        PhotonNetwork.LeaveRoom();
    }
}
