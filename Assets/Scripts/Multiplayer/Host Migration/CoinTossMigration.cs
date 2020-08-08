using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CoinTossMigration : MonoBehaviourPunCallbacks
{
    public GameObject kickObject;
    public TMP_Text kickText;
    public TMP_Text instructionText;

    private IEnumerator KickPlayer()
    {
        int count = 4;
        for (int i = 0; i < 5; i++)
        {
            kickText.text = (count + " Seconds").ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        PhotonNetwork.LeaveRoom();
        LevelLoader.Instance.LoadNextScene(2);
        Music.Instance.PlayTitleMusic();
        if (Music.Instance.skipToMain)
        {
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        instructionText.text = "";
        kickObject.SetActive(true);
        StartCoroutine(KickPlayer());
    }

}