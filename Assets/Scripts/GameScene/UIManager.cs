using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public Text yourName;
    public Text opponentName;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient) {
            yourName.text = "Host: " + GameManager.userName;
            opponentName.text = "Client: " + PhotonNetwork.PlayerListOthers[0].NickName;
        }
            
        else {
            yourName.text = "Client: " + GameManager.userName;
            opponentName.text = "Host: " + PhotonNetwork.PlayerListOthers[0].NickName;
        }
    }
}
