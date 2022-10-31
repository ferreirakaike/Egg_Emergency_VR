using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.XR.CoreUtils;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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

    void Update()
    {
        if (networkVar.isGameOver)
        {
            spawnedPlayerPrefab.SetActive(false);
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", playerSpawnLocations[0].position, playerSpawnLocations[0].rotation);
            spawnedBasketPrefab =PhotonNetwork.Instantiate("Network Basket", basketSpawnLocations[0].position, basketSpawnLocations[0].rotation);
            spawnedBasketPrefab.transform.localScale = new Vector3(25,25,25);
            networkVar.UpdateBasketIDs(spawnedBasketPrefab.GetPhotonView().ViewID, 0);
            networkVar.UpdatePlayerIDs(spawnedPlayerPrefab.GetPhotonView().ViewID, 0);
        }
        else
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", playerSpawnLocations[1].position, playerSpawnLocations[1].rotation);
            XROrigin origin = FindObjectOfType<XROrigin>();
            origin.transform.position = playerSpawnLocations[1].position;
            origin.transform.rotation = playerSpawnLocations[1].rotation;
            spawnedBasketPrefab =PhotonNetwork.Instantiate("Network Basket", basketSpawnLocations[1].position, basketSpawnLocations[1].rotation);
            spawnedBasketPrefab.transform.localScale = new Vector3(25,25,25);
            networkVar.UpdateBasketIDs(spawnedBasketPrefab.GetPhotonView().ViewID, 1);
            networkVar.UpdatePlayerIDs(spawnedPlayerPrefab.GetPhotonView().ViewID, 1);
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
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("OwnerLeftRoom");
    }
}
