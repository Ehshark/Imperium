﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartCombat : MonoBehaviour
{
    public GameObject attackButton;
    public GameObject cancelButton;

    public void StartCombatStart()
    {
        //Switch the buttons shown
        SwitchButtons();

        //Push a notification for the StartCombat Event
        EventManager.Instance.PostNotification(EVENT_TYPE.START_COMBAT);

        //Update the instructions text
        //TMP_Text text = GameManager.Instance.instructionsObj.GetComponent<TMP_Text>();
        //text.text = "Hello";
    }

    public void CancelCombat()
    {
        SwitchButtons();        

        //Remove all Combat Listener scripts from each card
        foreach (Transform t in GameManager.Instance.alliedMinionZone)
        {
            StartCombatListener scl = t.gameObject.GetComponent<StartCombatListener>();
            if (scl)
            {
                CardVisual cv = t.GetComponent<CardVisual>();
                Destroy(scl);

                if (cv.Md)
                {
                    ChangeCardColour(t.gameObject, cv.Md.Color);
                }
                else
                {
                    ChangeCardColour(t.gameObject, Color.gray);
                }
            }
        }

        //Reset the counter 
        GameManager.Instance.alliedDamageCounter.text = "0";
        GameManager.Instance.alliedStealthDamageCounter.text = "0";
    }

    private void SwitchButtons()
    {
        if (attackButton.activeSelf)
        {
            cancelButton.SetActive(true);
            attackButton.SetActive(false);
            GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(true);
            GameManager.Instance.EnableOrDisablePlayerControl(false);
        }
        else
        {
            cancelButton.SetActive(false);
            attackButton.SetActive(true);
            GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(false);
            GameManager.Instance.EnableOrDisablePlayerControl(true);
        }
    }

    public static void ChangeCardColour(GameObject card, Color color)
    {
        CardVisual cv = card.GetComponent<CardVisual>();

        if (cv.Md)
        {
            cv.cardBackground.color = color;
        }
        else if (cv.Sd)
        {
            cv.cardBackground.color = color;
        }
    }

}
