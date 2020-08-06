using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InvokeEventCommand : Command
{
    //Multiplayer
    const byte DISABLE_ICON_QUEUE_EVENT = 61;

    public static Queue<InvokeEventCommand> InvokeEventQueue = new Queue<InvokeEventCommand>();

    public object cardType;
    public MethodInfo method;
    public GameObject card;

    public InvokeEventCommand(MethodInfo methodInfo, object type, GameObject c)
    {
        cardType = type;
        method = methodInfo;
        card = c;
    }

    public override void AddToQueue()
    {
        GameManager.Instance.EnableOrDisablePlayerControl(false);
        InvokeEventQueue.Enqueue(this);

        //testing
        Debug.Log("object: " + this.cardType);
    }

    public override void StartCommandExecution()
    {
        EffectCommand.Instance.InvokeMethod(this);
    }

    public static void InvokeNextEvent()
    {
        if (InvokeEventQueue.Count > 0)
        {
            InvokeEventQueue.Dequeue().StartCommandExecution();
        }
        else
        {
            GameManager.Instance.EffectIconQueue.gameObject.SetActive(false);
            PhotonNetwork.RaiseEvent(DISABLE_ICON_QUEUE_EVENT, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
            EffectCommand.Instance.inEffect = false;
            if (!playingQueue)
                GameManager.Instance.EnableOrDisablePlayerControl(true);
        }
    }
}
