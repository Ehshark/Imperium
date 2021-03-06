﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawDiscardStarter : MonoBehaviour
{
    private const byte DRAW_CARDS = 13;

    public void StartEvent()
    {
        //Multiplayer 
        object[] data = new object[] { 1 };
        PhotonNetwork.RaiseEvent(DRAW_CARDS, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);

        GameManager.Instance.DrawCard(UIManager.Instance.GetActiveDeckList(true), GameManager.Instance.GetActiveHand(true));
        GameManager.Instance.EnableOrDisablePlayerControl(true);

        foreach(Transform t in GameManager.Instance.GetActiveHand(true))
        {
            PlayCard pc = t.GetComponent<PlayCard>();

            if (pc)
            {
                Destroy(pc);
            }

            t.gameObject.AddComponent<DrawDiscardListener>();
        }

        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Select One Card to Discard";
    }
}
