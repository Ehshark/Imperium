﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShockListener : MonoBehaviour, IPointerDownHandler
{
    public void Start()
    {
        gameObject.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        if (cv.CurrentHealth - 1 == 0)
        {
            EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.MINION_DEFEATED);
        }

        cv.AdjustHealth(1, false);

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            ShockListener sl = t.GetComponent<ShockListener>();
            if (sl)
            {
                Destroy(sl);
            }
        }

        if (GameManager.Instance.InHeroPower)
        {
            GameManager.Instance.InHeroPower = false;
            EffectCommand.Instance.inEffect = false;
        }
        else
        {
            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();
            UIManager.Instance.RemoveEffectIcon = true;
        }

        if (StartGameController.Instance.tutorial)
        {
            StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
        }
    }

    public void OnDestroy()
    {
        gameObject.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(false);
    }
}
