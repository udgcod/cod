using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{

    public static PhotonLobby lobby;


    public string roomName;
    public int roomSize;

    public GameObject roomListingPrefab;
    public Transform roomPanel;

    public List<RoomInfo> roomListings;

     

    // Start is called before the first frame update

    private void Awake()
    {
        lobby = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        roomListings = new List<RoomInfo>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        //RemoveRoomListings();
        int tempIndex;
        foreach(RoomInfo room in roomList)
        {
            if(roomListings != null)
            {
                tempIndex = roomListings.FindIndex(ByName(room.Name));
            }
            else
            {
                tempIndex = -1;
            }
            if(tempIndex != -1)
            {
                roomListings.RemoveAt(tempIndex);
                Destroy(roomPanel.GetChild(tempIndex).gameObject);
            }
            else
            {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }

    void RemoveRoomListings()
    {
        int i = 0;
        while (roomPanel.childCount != 0)
        {
            Destroy(roomPanel.GetChild(i).gameObject);
            i++;
        }
    }

    void ListRoom(RoomInfo room)
    {
        if(room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomPanel);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.roomName = room.Name;
            tempButton.roomSize = room.MaxPlayers;
            tempButton.setRoom();
        }
    }


    public void CreateRoom()
    {
        Debug.Log("Trying to create a new Room");
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create  a new room but failed, there must already be a room with the same name");
    }

    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn;
    }

    public void OnRoomSizeChanged(string  sizeIn)
    {
        roomSize = int.Parse(sizeIn);
    }

    public void JoinLobbyOnClick()
    {

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
