using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Autolobby : MonoBehaviourPunCallbacks
{
    public Button ConnectButton;
    public Button JoinRandomButton;
    public Text Log;
    public Text PlayerCount;
    

    public byte maxPlayersPerRoom = 6;
    public byte minPlayersPerRoom = 2;
    public int playersCount;

    private bool IsLoading = false;
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
                Log.text += "\nConnected to server";

            }
            else
            {
                Log.text += "\n Failing Connecting to Server";
            }

        }
    }

    public override void OnConnectedToMaster()
    {
        ConnectButton.interactable = false;
        JoinRandomButton.interactable = true;
    }

    public void JoinRandom()
    {
        if (!PhotonNetwork.JoinRandomRoom()){
            Log.text += "\nFall Joining Room";
        }
        
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log.text += "\nNo Rooms To Join. Creating One...";

        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() { MaxPlayers = maxPlayersPerRoom }))
        {
            Log.text += "\nRoom Created";
        }
        else
        {
            Log.text += "\nFail Creating Room";
        }
    }

    public override void OnJoinedRoom()
    {
        Log.text += "\nJoinned";
        JoinRandomButton.interactable = false;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            playersCount = PhotonNetwork.CurrentRoom.PlayerCount;

            PlayerCount.text = playersCount + "/" + maxPlayersPerRoom;
        }

        if (!IsLoading && playersCount >= minPlayersPerRoom)
        {
            LoadMap();
        }
       
    }

    private void LoadMap()
    {
        IsLoading = true;
        PhotonNetwork.LoadLevel("MultiplayerTest");
    }

}
