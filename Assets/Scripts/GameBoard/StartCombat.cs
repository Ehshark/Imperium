using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class StartCombat : MonoBehaviour
{
    private const byte START_COMBAT = 17;

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

        if (!StartGameController.Instance.tutorial)
        {
            //Push a notification for the StartCombat Event
            EventManager.Instance.PostNotification(EVENT_TYPE.START_COMBAT);

            //Update the instructions text
            GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Please Select Minions to Attack";
        }
        else
        {
            GameManager.Instance.ActiveHero(true).SubmitButton.Find("SubmitIcon").GetComponent<Button>().interactable = false;
            GameManager.Instance.ActiveHero(true).CancelButton.Find("CancelIcon").GetComponent<Button>().interactable = false;

            StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
        }

        GameManager.Instance.shopButton.interactable = false;
        GameManager.Instance.endButton.interactable = false;

        AssignAllyDamageBonus(true);
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
                GameManager.Instance.ActiveHero(true).canAttackParticle.Stop(); //stops the "can attack" particle effect on hero
                GameManager.Instance.ActiveHero(true).canAttackParticle.gameObject.SetActive(false); //hides the particle system

                if (cv.Md != null)
                {
                    GameManager.Instance.ChangeCardColour(t.gameObject, cv.cardBackground.color);
                    cv.particleGlow.gameObject.SetActive(false);
                }
                else
                {
                    GameManager.Instance.ChangeCardColour(t.gameObject, cv.cardBackground.color);
                    cv.particleGlow.gameObject.SetActive(false);
                }
            }
        }

        //Loop through all minions that are currently attacking and set there tap state to false
        if (!GameManager.Instance.IsDefending)
        {
            foreach (GameObject go in GameManager.Instance.MinionsAttacking)
            {
                go.GetComponent<CardVisual>().IsTapped = false;
                go.GetComponent<CardVisual>().ChangeTappedAppearance();
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
            GameManager.Instance.ActiveHero(true).canAttackParticle.Stop(); //stops the "can attack" particle effect on hero
            GameManager.Instance.ActiveHero(true).canAttackParticle.gameObject.SetActive(false); //hides the particle system
        }

        //Reset the counter if combat was cancelled
        if (!GameManager.Instance.IsDefending)
        {
            GameManager.Instance.alliedDamageCounter.text = "0";
            GameManager.Instance.alliedStealthDamageCounter.text = "0";
            GameManager.Instance.alliedLifestealDamageCounter.text = "0";
            GameManager.Instance.alliedPoisonTouchDamageCounter.text = "0";

            totalDamage["stealth"] = 0;
            totalDamage["lifesteal"] = 0;
            totalDamage["poisonTouch"] = 0;
            totalDamage["damage"] = 0;

            GameManager.Instance.MinionsAttacking = new List<GameObject>();

            GameManager.Instance.shopButton.interactable = true;
            GameManager.Instance.endButton.interactable = true;
        }

        AssignAllyDamageBonus(false);
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

            GameManager.Instance.ActiveHero(true).canAttackParticle.Stop(); //stops the "can attack" particle effect on hero
            GameManager.Instance.ActiveHero(true).canAttackParticle.gameObject.SetActive(false); //hides the particle system

            if (CheckForVictory())
            {
                CancelCombat();
                StartCoroutine(GameManager.Instance.SetInstructionsText("Attacking player has won the game."));
                return;
            }

            //Add Damage
            totalDamage["stealth"] = Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text);
            totalDamage["lifesteal"] = Int32.Parse(GameManager.Instance.alliedLifestealDamageCounter.text);
            totalDamage["poisonTouch"] = Int32.Parse(GameManager.Instance.alliedPoisonTouchDamageCounter.text);
            totalDamage["damage"] = Int32.Parse(GameManager.Instance.alliedDamageCounter.text);

            GameManager.Instance.IsDefending = true;
            CancelCombat();

            GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Waiting for Opponent...";
            GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(false);
            GameManager.Instance.ActiveHero(true).SubmitButton.gameObject.SetActive(false);
            GameManager.Instance.ActiveHero(true).CancelButton.gameObject.SetActive(false);

            List<CardPhoton> cards = new List<CardPhoton>();
            foreach (GameObject g in GameManager.Instance.MinionsAttacking)
            {
                cv = g.GetComponent<CardVisual>();
                cv.ChangeTappedAppearance();
                cv.particleGlow.gameObject.SetActive(false);

                if (cv.Md != null)
                {
                    cards.Add(new MinionDataPhoton(cv.Md));
                }
                else
                {
                    cards.Add(new StarterDataPhoton(cv.Sd));
                }
            }

            int[] totalDamageToSend = new int[] { totalDamage["stealth"], totalDamage["lifesteal"], totalDamage["poisonTouch"], totalDamage["damage"] };
            byte[] cardByte = DataHandler.Instance.ObjectToByteArray(cards);
            object[] data = new object[] { totalDamageToSend, cardByte };

            StartCombatPun.Instance.SendData(START_COMBAT, data);
        }
        else
        {
            //Update the instructions text
            StartCoroutine(GameManager.Instance.SetInstructionsText("No Minions or Hero Selected to Attack"));
        }
    }
    public void AssignDamageToAttackers()
    {
        foreach (Transform card in GameManager.Instance.GetActiveMinionZone(true))
        {
            CardVisual cv = card.GetComponent<CardVisual>();
            if (cv.IsTapped)
            {
                cv.AdjustHealth(1, false);
            }
        }
    }

    private void AssignAllyDamageBonus(bool increase)
    {
        CardVisual cv;
        Transform alliedMinions = GameManager.Instance.GetActiveMinionZone(true);
        foreach (Transform t in alliedMinions)
        {
            cv = t.GetComponent<CardVisual>();
            if (cv.GetCardData() is MinionData)
            {
                foreach (Transform t2 in alliedMinions)
                {
                    string clan = t2.GetComponent<CardVisual>().GetCardData().CardClass;
                    if (clan != null && clan.Equals(cv.GetCardData().AllyClass))
                    {
                        if (increase)
                            cv.AdjustDamage(1, true);
                        else
                            cv.AdjustDamage(1, false);
                    }
                }
            }
        }
    }

    private bool CheckForVictory()
    {
        int totalAttackingDamage = Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text) +
            Int32.Parse(GameManager.Instance.alliedLifestealDamageCounter.text) +
            Int32.Parse(GameManager.Instance.alliedPoisonTouchDamageCounter.text) +
            Int32.Parse(GameManager.Instance.alliedDamageCounter.text);

        int totalDefendingHealth = 0;

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            if (t.gameObject.activeSelf && !t.GetComponent<CardVisual>().IsTapped)
            {
                totalDefendingHealth += t.GetComponent<CardVisual>().CurrentHealth;
            }
        }

        totalDefendingHealth += GameManager.Instance.ActiveHero(false).CurrentHealth;

        if (totalAttackingDamage >= totalDefendingHealth)
        {
            return true;
        }

        return false;
    }
}
