using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class StartCombat : MonoBehaviour
{
    public GameObject attackButton;
    public GameObject cancelButton;
    public GameObject submitButton;

    public static Dictionary<String, int> totalDamage = new Dictionary<string, int>
    {
        { "stealth", 0 },
        { "lifesteal", 0 },
        { "poisonTouch", 0 },
        { "damage", 0 }
    };

    public void StartCombatStart()
    {
        //Switch the buttons shown
        SwitchButtons();

        //Push a notification for the StartCombat Event
        EventManager.Instance.PostNotification(EVENT_TYPE.START_COMBAT);

        //Update the instructions text
        TMP_Text text = GameManager.Instance.instructionsObj.GetComponent<TMP_Text>();
        text.text = "Please Select Minions to Attack";
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

        //Reset the Damage
        totalDamage["stealth"] = 0;
        totalDamage["lifesteal"] = 0;
        totalDamage["poisonTouch"] = 0;
        totalDamage["damage"] = 0;

        //Reset the List of Attack Minions
        GameManager.Instance.MinionsAttacking = new List<GameObject>();

        //Reset the instructions text
        TMP_Text text = GameManager.Instance.instructionsObj.GetComponent<TMP_Text>();
        text.text = "";
    }

    private void SwitchButtons()
    {
        if (attackButton.activeSelf)
        {
            cancelButton.SetActive(true);
            attackButton.SetActive(false);
            submitButton.SetActive(true);
            GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(true);
            GameManager.Instance.EnableOrDisablePlayerControl(false);
        }
        else
        {
            cancelButton.SetActive(false);
            attackButton.SetActive(true);
            submitButton.SetActive(false);
            GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(false);
            GameManager.Instance.EnableOrDisablePlayerControl(true);
        }
    }

    public void SubmitAttack()
    {
        foreach (GameObject card in GameManager.Instance.MinionsAttacking)
        {
            CardVisual cv = card.GetComponent<CardVisual>();

            cv.health.text = (Int32.Parse(cv.health.text) - 1).ToString();

            if (cv.Md != null)
            {
                cv.CurrentHealth = cv.CurrentHealth - 1;
            }
            else if (cv.Sd != null)
            {
                cv.CurrentHealth = cv.CurrentHealth - 1;
            }

            if (cv.CurrentHealth <= 0)
            {
                cv.health.text = cv.TotalHealth.ToString();
                cv.CurrentHealth = cv.TotalHealth;
                GameManager.Instance.MoveCard(card, GameManager.Instance.alliedDiscardPile, GameManager.Instance.alliedDiscardPileList, true);
            }
        }

        //Add Damage
        StartCombat.totalDamage["stealth"] = Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text);
        StartCombat.totalDamage["lifesteal"] = Int32.Parse(GameManager.Instance.alliedLifestealDamageCounter.text);
        StartCombat.totalDamage["poisonTouch"] = Int32.Parse(GameManager.Instance.alliedPoisonTouchDamageCounter.text);
        StartCombat.totalDamage["damage"] = Int32.Parse(GameManager.Instance.alliedDamageCounter.text);

        CancelCombat();
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
