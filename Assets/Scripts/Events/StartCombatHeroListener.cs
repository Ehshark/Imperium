using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartCombatHeroListener : MonoBehaviour, IPointerDownHandler
{
    private bool isClicked;

    Hero active;

    public void Start()
    {
        active = GameManager.Instance.ActiveHero();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isClicked)
        {
            isClicked = false;
            active.IsAttacking = false;

            //Increase Mana
            active.AdjustMana(active.Damage, true);

            //Decrease Damage
            GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) - active.Damage).ToString();
        }
        else
        {
            if (active.CurrentMana >= active.Damage)
            {
                isClicked = true;
                active.IsAttacking = true;

                //Decrease Mana
                active.AdjustMana(active.Damage, false);

                //Increase Damage
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) + active.Damage).ToString();
            }
            else
            {
                StartCoroutine(GameManager.Instance.SetInstructionsText("Not Enough Mana for Hero to Attack"));
            }
        }
    }
}
