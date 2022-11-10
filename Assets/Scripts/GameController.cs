using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void DropPlayer()
    {
        Photon.Pun.PhotonNetwork.Instantiate(charPrefab.name, , Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
