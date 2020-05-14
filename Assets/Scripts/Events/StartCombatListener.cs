using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartCombatListener : MonoBehaviour, IPointerDownHandler
{
    private bool tapped;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();
        TapMinion(cv, card);
    }

    private void TapMinion(CardVisual cv, GameObject card)
    { 
        if (cv.Md)
        {
            if (tapped)
            {
                StartCombat.ChangeCardColour(card, cv.Md.Color);
                cv.Md.IsTapped = false;
                tapped = false;
                GameManager.Instance.MinionsAttacking.Remove(card);
            }
            else
            {
                StartCombat.ChangeCardColour(card, Color.cyan);
                cv.Md.IsTapped = true;
                tapped = true;
                GameManager.Instance.MinionsAttacking.Add(card);
            }

            HasAbilites(cv.Md);
        }
        else if (cv.Sd)
        {
            if (tapped)
            {
                StartCombat.ChangeCardColour(card, Color.gray);
                cv.Sd.IsTapped = false;
                tapped = false;

                //Decrease damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) - cv.Sd.AttackDamage).ToString();
                GameManager.Instance.MinionsAttacking.Add(card);
            }
            else
            {
                StartCombat.ChangeCardColour(card, Color.cyan);
                cv.Sd.IsTapped = true;
                tapped = true;

                //Increase damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) + cv.Sd.AttackDamage).ToString();
                GameManager.Instance.MinionsAttacking.Remove(card);
            }
        }
    }

    private void HasAbilites(MinionData md)
    {
        if (md.EffectId1 == 8)
        {
            if (tapped)
            {
                GameManager.Instance.alliedStealthDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text) + md.AttackDamage).ToString();
            }
            else
            {
                GameManager.Instance.alliedStealthDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text) - md.AttackDamage).ToString();
            }
        }
        else if (md.EffectId1 == 10)
        {
            if (tapped)
            {
                GameManager.Instance.alliedLifestealDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedLifestealDamageCounter.text) + md.AttackDamage).ToString();
            }
            else
            {
                GameManager.Instance.alliedLifestealDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedLifestealDamageCounter.text) - md.AttackDamage).ToString();
            }
        }
        else if (md.EffectId1 == 7)
        {
            if (tapped)
            {
                GameManager.Instance.alliedPoisonTouchDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedPoisonTouchDamageCounter.text) + md.AttackDamage).ToString();
            }
            else
            {
                GameManager.Instance.alliedPoisonTouchDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedPoisonTouchDamageCounter.text) - md.AttackDamage).ToString();
            }
        }
        else
        {
            if (tapped)
            {
                //Increase damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) + md.AttackDamage).ToString();
            }
            else
            {
                //Decrease damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) - md.AttackDamage).ToString();
            }
        }       
    }
}
