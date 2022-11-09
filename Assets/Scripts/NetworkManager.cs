using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
  public bool isMultiplayer = false;
  public List<RoomInfo> roomCreated = new List<RoomInfo>();
  public List<int> availableRoom = new List<int>(){1,2,3,4,5,6,7,8,9,10};
  // Start is called before the first frame update
  void Start()
  {
    ConnectToServer();
  }

  // Update is called once per frame
  void Update()
  {
      
  }

  private void ConnectToServer() 
  {
    PhotonNetwork.ConnectUsingSettings();
    Debug.Log("Trying to connect to server ... ");
  }

  public override void OnConnectedToMaster()
  {
    Debug.Log("Connected to server");
    base.OnConnectedToMaster();
    PhotonNetwork.JoinLobby();
  }

  public override void OnRoomListUpdate(List<RoomInfo> roomList)
  {
    base.OnRoomListUpdate(roomList);
    foreach (RoomInfo room in roomList)
    {
      if (!room.RemovedFromList && !roomCreated.Contains(room))
      {
        roomCreated.Add(room);
      }
      else if (room.RemovedFromList && roomCreated.Contains(room))
      {
        roomCreated.Remove(room);
        roomCreated.Sort();
        photonView.RPC("UpdateAvailability", RpcTarget.AllBuffered, Int32.Parse(room.Name));
      }
    }
  }

  public override void OnJoinedLobby()
  {
    base.OnJoinedLobby();
    Debug.Log("Joined Lobby");
  }

  public bool InitializeRoom(int roomNumber)
  {
    RoomOptions roomOptions = new RoomOptions();
    if (isMultiplayer)
    {
      roomOptions.MaxPlayers = (byte)2;
    }
    else
    {
      roomOptions.MaxPlayers = (byte)1;
    }
    
    roomOptions.IsVisible = true;
    roomOptions.IsOpen = true;
    if (!PhotonNetwork.CreateRoom(roomNumber.ToString(), roomOptions, TypedLobby.Default))
    {
      return false;
    }
    photonView.RPC("UpdateAvailability", RpcTarget.AllBuffered, roomNumber * -1);
    return true;
  }

  public bool JoinRoom(int roomNumber)
  {
    return PhotonNetwork.JoinRoom(roomNumber.ToString());
  }

  [PunRPC]
  private void UpdateAvailability(int roomNumber = 0)
  {
    if (roomNumber < 0)
    {
      availableRoom.Remove(roomNumber * -1);
    }
    else if (roomNumber > 0)
    {
      availableRoom.Add(roomNumber);
      availableRoom.Sort();
    }
  }

  public override void OnJoinedRoom()
  {
    Debug.Log("Joined room successfully");
    base.OnJoinedRoom();
    SceneManager.LoadScene("KaiScene");
  }

  public override void OnPlayerEnteredRoom(Player newPlayer)
  {
    Debug.Log("A new player joined the room");
    base.OnPlayerEnteredRoom(newPlayer);
  }

  public void SetMultiplayer(Toggle setting)
  {
    isMultiplayer = setting.isOn;
  }
}
