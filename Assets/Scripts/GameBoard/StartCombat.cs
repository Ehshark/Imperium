using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class StartCombat : MonoBehaviour
{
    public Dictionary<string, int> totalDamage = new Dictionary<string, int>
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
        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Please Select Minions to Attack";

        GameManager.Instance.buyButton.interactable = false;
        GameManager.Instance.changeButton.interactable = false;
    }

    public void CancelCombat()
    {
        SwitchButtons();

        //Remove all Combat Listener scripts from each card
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            StartCombatListener scl = t.gameObject.GetComponent<StartCombatListener>();
            if (scl)
            {
                CardVisual cv = t.GetComponent<CardVisual>();
                Destroy(scl);

                if (cv.Md)
                {
                    GameManager.Instance.ChangeCardColour(t.gameObject, cv.Md.Color);
                }
                else
                {
                    GameManager.Instance.ChangeCardColour(t.gameObject, Color.gray);
                }
            }
        }

        //Loop through all minions that are currently attacking and set there tap state to false
        if (!GameManager.Instance.IsDefending)
        {
            foreach (GameObject go in GameManager.Instance.MinionsAttacking)
            {
                go.GetComponent<CardVisual>().IsTapped = false;
            }
        }

        //Reset Hero 
        Hero active = GameManager.Instance.ActiveHero(true);
        StartCombatHeroListener schl = active.HeroImage.GetComponent<StartCombatHeroListener>();

        if (active.IsAttacking)
        {
            active.AdjustMana(active.Damage, true);
            active.IsAttacking = false;
        }

        if (schl)
        {
            Destroy(schl);
        }

        //Reset the counter if combat was cancelled
        if (!GameManager.Instance.IsDefending)
        {
            GameManager.Instance.alliedDamageCounter.text = "0";
            GameManager.Instance.alliedStealthDamageCounter.text = "0";
            GameManager.Instance.alliedLifestealDamageCounter.text = "0";
            GameManager.Instance.alliedPoisonTouchDamageCounter.text = "0";
        }

        //Reset the Damage if combat was cancelled
        if (!GameManager.Instance.IsDefending)
        {
            totalDamage["stealth"] = 0;
            totalDamage["lifesteal"] = 0;
            totalDamage["poisonTouch"] = 0;
            totalDamage["damage"] = 0;
        }

        //Reset the List of Attack Minions if combat was cancelled
        if (!GameManager.Instance.IsDefending)
        {
            GameManager.Instance.MinionsAttacking = new List<GameObject>();
        }
    }

    private void SwitchButtons()
    {
        if (GameManager.Instance.ActiveHero(true).AttackButton.gameObject.activeSelf)
        {
            GameManager.Instance.ActiveHero(true).CancelButton.gameObject.SetActive(true);
            GameManager.Instance.ActiveHero(true).AttackButton.gameObject.SetActive(false);
            GameManager.Instance.ActiveHero(true).SubmitButton.gameObject.SetActive(true);
            GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(true);
            GameManager.Instance.EnableOrDisablePlayerControl(false);
            GameManager.Instance.ActiveHero(true).StartedCombat = true;
        }
        else
        {
            GameManager.Instance.ActiveHero(true).CancelButton.gameObject.SetActive(false);
            if (!GameManager.Instance.IsDefending)
            {
                GameManager.Instance.ActiveHero(true).AttackButton.gameObject.SetActive(true);
                GameManager.Instance.EnableOrDisablePlayerControl(true);
                GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(false);
            }
            GameManager.Instance.ActiveHero(true).SubmitButton.gameObject.SetActive(false);
            GameManager.Instance.ActiveHero(true).StartedCombat = false;
        }
    }

    public void SubmitAttack()
    {
        CardVisual cv;
        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "";
        if (GameManager.Instance.MinionsAttacking.Count != 0 || GameManager.Instance.ActiveHero(true).IsAttacking)
        {
            //Set current Hero is false
            GameManager.Instance.ActiveHero(true).IsAttacking = false;

            //Add Damage
            totalDamage["stealth"] = Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text);
            totalDamage["lifesteal"] = Int32.Parse(GameManager.Instance.alliedLifestealDamageCounter.text);
            totalDamage["poisonTouch"] = Int32.Parse(GameManager.Instance.alliedPoisonTouchDamageCounter.text);
            totalDamage["damage"] = Int32.Parse(GameManager.Instance.alliedDamageCounter.text);

            EventManager.Instance.PostNotification(EVENT_TYPE.DEFEND_AGAINST);
            CancelCombat();
            foreach (GameObject g in GameManager.Instance.MinionsAttacking)
            {
                cv = g.GetComponent<CardVisual>();
                cv.ChangeTappedAppearance();
            }
        }
        else
        {
            //Update the instructions text
            StartCoroutine(GameManager.Instance.SetInstructionsText("No Minions or Hero Selected to Attack"));
        }
    }
    public void AssignDamageToAttackers()
    {
        foreach (GameObject card in GameManager.Instance.MinionsAttacking)
        {
            CardVisual cv = card.GetComponent<CardVisual>();
            cv.AdjustHealth(1, false);
        }
    }
}
