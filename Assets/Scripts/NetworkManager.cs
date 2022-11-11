using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

/// <summary>
/// This class handles communication with the server, creating rooms, and putting users into rooms
/// </summary>
public class NetworkManager : MonoBehaviourPunCallbacks
{
  /// <summary>
  /// Variable that determines whether the room's setting is set to Multiplayer
  /// </summary>
  public static bool isMultiplayer = true;

  /// <summary>
  /// A List that holds the number of available room that user can join/create
  /// </summary>
  /// <typeparam name="int">Room number</typeparam>
  /// <returns></returns>
  public List<int> availableRoom = new List<int>(){1,2,3,4,5,6,7,8,9,10};
  // Start is called before the first frame update
  void Start()
  {
    isMultiplayer = true;
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

  /// <summary>
  /// Override parent method. As user connect to the server, he is immediately joining the lobby.
  /// </summary>
  public override void OnConnectedToMaster()
  {
    Debug.Log("Connected to server");
    base.OnConnectedToMaster();
    PhotonNetwork.JoinLobby();
    PhotonNetwork.AutomaticallySyncScene = true;
  }

  /// <summary>
  /// Override parent method. This was mainly used for debugging
  /// </summary>
  public override void OnJoinedLobby()
  {
    base.OnJoinedLobby();
    Debug.Log("Joined Lobby");
  }

  /// <summary>
  /// This method is used to create a room with the room name set to the passed in room number.
  /// </summary>
  /// <param name="roomNumber">Room number that is used to set the name of the room.</param>
  /// <returns>Returns true if the room creation request is sucessfully put into the network queue. Returns false otherwise.</returns>
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
    return true;
  }

  /// <summary>
  /// This method allows user to join room that was previously created.
  /// </summary>
  /// <param name="roomNumber">Room number that the user wants to join</param>
  /// <returns>Returns true if the room joining request is sucessfully put into the network queue. Returns false otherwise.</returns>
  public bool JoinRoom(int roomNumber)
  {
    return PhotonNetwork.JoinRoom(roomNumber.ToString());
  }

  /// <summary>
  /// Override parent method. Upon successfully joining a room, the user is immediately teleported into the game scene
  /// </summary>
  public override void OnJoinedRoom()
  {
    Debug.Log("Joined room successfully");
    base.OnJoinedRoom();
  }

  /// <summary>
  /// Override parent method. Upon a new player entering the room, load game scene
  /// </summary>
  /// <param name="newPlayer">New Player that joined the room</param>
  public override void OnPlayerEnteredRoom(Player newPlayer)
  {
    base.OnPlayerEnteredRoom(newPlayer);
    PhotonNetwork.LoadLevel("KaiScene");
  }

  /// <summary>
  /// This method sets the game mode to either single player or multiplayer based on the passed in toggle state.
  /// </summary>
  /// <param name="setting">The toggle that holds the value of whether the game mode should be set to multiplayer or not</param>
  public void SetMultiplayer(Toggle setting)
  {
    isMultiplayer = setting.isOn;
  }
}
