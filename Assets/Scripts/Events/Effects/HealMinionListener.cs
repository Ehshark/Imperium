﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealMinionListener : MonoBehaviour, IPointerDownHandler
{
    const byte ADJUST_HEALTH_SYNC_EVENT = 24;

    private void Start()
    {
        transform.Find("ParticleGlow").gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        if (cv.IsPromoted && (cv.CurrentHealth < cv.PromotedHealth))
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Minion's Health Increased by 1"));

            //Adjust Damage 
            cv.AdjustHealth(1, true);
            //Set Health Flag to true
            cv.HealEffect = true;

            //Remove Listener 
            StartCoroutine(RemoveListener());
        }
        else if (cv.CurrentHealth < cv.TotalHealth && !cv.IsPromoted)
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Minion's Health Increased by 1"));

            //Adjust Damage 
            cv.AdjustHealth(1, true);
            //Set Health Flag to true
            cv.HealEffect = true;

            //Remove Listener 
            StartCoroutine(RemoveListener());
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Minion is already at Max Health"));
        }
    }

    IEnumerator RemoveListener()
    {
        //Multiplayer
        int position = gameObject.transform.GetSiblingIndex();
        object[] data = new object[] { position, 1, true, true };
        EffectCommandPun.Instance.SendData(ADJUST_HEALTH_SYNC_EVENT, data);

        yield return new WaitForSeconds(2);

        GameManager.Instance.EnableOrDisablePlayerControl(true);

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            HealMinionListener hml = t.gameObject.GetComponent<HealMinionListener>();

            if (hml)
            {
                Destroy(hml);
            }
        }

        //Call Heal Minion Power Effect
        EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_HEAL_MINION);

        //Call the Next Power in the Queue
        InvokeEventCommand.InvokeNextEvent();

        UIManager.Instance.RemoveEffectIcon = true;
    }

    private void OnDestroy()
    {
        transform.Find("ParticleGlow").gameObject.SetActive(false);
    }
}
