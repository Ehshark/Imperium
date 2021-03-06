﻿
using UnityEngine;
using UnityEngine.EventSystems;

public class SilenceListener : MonoBehaviour, IPointerDownHandler
{
    const byte ACTIVATE_SILENCE_SYNC_EVENT = 29;

    public void Start()
    {
        gameObject.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();
        Transform minions = GameManager.Instance.GetActiveMinionZone(false);

        //Multiplayer 
        int position = gameObject.transform.GetSiblingIndex();
        object[] data = new object[] { position };
        EffectCommandPun.Instance.SendData(ACTIVATE_SILENCE_SYNC_EVENT, data);

        cv.ActivateSilence(true);

        foreach (Transform t in minions)
        {
            SilenceListener sl = t.GetComponent<SilenceListener>();
            if (sl)
            {
                Destroy(sl);
            }
        }

        EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_SILENCE);
        InvokeEventCommand.InvokeNextEvent();
        UIManager.Instance.RemoveEffectIcon = true;
    }

    public void OnDestroy()
    {
        gameObject.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(false);
    }
}
