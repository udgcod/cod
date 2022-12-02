using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class PhotonRoom : MonoBehaviourPunCallbacks
{

    public static PhotonRoom room;
    private PhotonView PV;

    public bool isGameLoaded;
    public int currentScene;

    //Player Info

    Player[] PhotonPlayers;
    public int playerInRoom;
    public int myNumberInRoom;

    [SerializeField]
    public int playersInGame;

   

    public GameObject lobbyGameObject;

    public GameObject roomGameObject;

    public Transform playersPanel;

    public GameObject playerListingPrefab;

    public GameObject startButton;

    private int multiplayerSceneIndex;

    void ClearPlayerListings()
    {
        for (int i = playersPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }

    void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject templisting = Instantiate(playerListingPrefab, playersPanel);
                Text tempText = templisting.transform.GetChild(0).GetComponent<Text>();
                tempText.text = player.NickName;
            }
        }
    }

    public override void OnJoinedRoom()
    {
 

        roomGameObject.SetActive(true);
        lobbyGameObject.SetActive(false);
  
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
        ClearPlayerListings();
        ListPlayers();


        PhotonPlayers = PhotonNetwork.PlayerList;
        playerInRoom = PhotonPlayers.Length;
        myNumberInRoom = playerInRoom;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        ClearPlayerListings();
        ListPlayers();
        PhotonPlayers = PhotonNetwork.PlayerList;
        playerInRoom++;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
     
        playerInRoom--;
        ClearPlayerListings();
        ListPlayers();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }

    }


    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);

        }
    }

    void Start()
    {
        PV = GetComponent < PhotonView>();
    }



    // Update is called once per frame
    void Update()
    {
    }
        
    

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length)
        {
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
}
