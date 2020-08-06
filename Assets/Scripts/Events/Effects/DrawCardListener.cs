using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardListener : MonoBehaviour
{
    private const byte DRAW_CARDS = 13;

    public void StartEvent()
    {
        object[] data = new object[] { 1 };
        PhotonNetwork.RaiseEvent(DRAW_CARDS, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        GameManager.Instance.DrawCard(UIManager.Instance.GetActiveDeckList(true), GameManager.Instance.GetActiveHand(true));
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.GetActiveHand(true).GetChild(GameManager.Instance.GetActiveHand(true).childCount - 1).gameObject.AddComponent<PlayCard>();

        //Call the Next Power in the Queue
        InvokeEventCommand.InvokeNextEvent();

        if (StartGameController.Instance.tutorial)
        {
            StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
        }
    }
}
