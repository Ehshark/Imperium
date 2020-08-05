using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EffectCommand : MonoBehaviour
{
    public static EffectCommand Instance { get; private set; } = null;

    public Queue<EVENT_TYPE> EffectQueue = new Queue<EVENT_TYPE>();
    public bool inEffect = false;

    const byte TAP_ANIMATION_SYNC_EVENT = 43;
    const byte ANIMATION_SYNC_EVENT = 44;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(Instance);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        InvokeRepeating("ContinueExecution", 2f, 0.5f);
    }

    public void StartCommandExecution()
    {
        if (!inEffect)
        {
            inEffect = true;

            EVENT_TYPE effect = EffectQueue.Dequeue();

            if (effect != EVENT_TYPE.TAP_MINION)
                EventManager.Instance.PostNotification(effect);

            InvokeEventCommand.InvokeNextEvent();
        }
    }

    public void ContinueExecution()
    {
        UIManager.Instance.RemoveEffectIcon = true;
        if (EffectQueue.Count != 0 && !inEffect)
        {
            if (CanEffectBeCalled())
            {
                StartCommandExecution();
            }
        }
        else
            GameManager.Instance.EffectIconQueue.gameObject.SetActive(false);
    }

    public bool CanEffectBeCalled()
    {
        bool result = false;
        bool isAttacking = GameManager.Instance.ActiveHero(true).StartedCombat;
        bool shopOpen = GameManager.Instance.shop.gameObject.activeSelf;
        bool isDefending = GameManager.Instance.IsDefending;        

        if (!isAttacking && !shopOpen && !isDefending)
        {
            result = true;
        }

        return result;
    }

    public void InvokeMethod(InvokeEventCommand iec)
    {
        StartCoroutine(InvokeDelay(iec));
    }

    public IEnumerator InvokeDelay(InvokeEventCommand iec)
    {
        ConditionListener cl = iec.card.GetComponent<ConditionListener>();
        int position = iec.card.transform.GetSiblingIndex();
        object[] data = new object[] { position };

        if (cl.ConditionEvent == EVENT_TYPE.TAP_MINION)
        {
            PhotonNetwork.RaiseEvent(TAP_ANIMATION_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
        }
        else
        {
            PhotonNetwork.RaiseEvent(ANIMATION_SYNC_EVENT, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
            SendOptions.SendReliable);
        }

        DelayCommand dc = new DelayCommand(iec.card.transform, 2f);
        dc.AddToQueue();

        GameManager.Instance.effectText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.effectText.gameObject.SetActive(false);

        iec.method.Invoke(iec.cardType, new object[] { });

        CardVisual cv = iec.card.GetComponent<CardVisual>();
        if (cl.ConditionEvent == EVENT_TYPE.TAP_MINION)
        {
            cv.AdjustHealth(1, false);
        }
    }
}
