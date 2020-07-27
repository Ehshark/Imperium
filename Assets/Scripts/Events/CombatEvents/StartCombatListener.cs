using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartCombatListener : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
            CardVisual cv = card.GetComponent<CardVisual>();
            TapMinion(cv, card);
        }
    }

    private void TapMinion(CardVisual cv, GameObject card)
    {
        if (cv.Md != null)
        {
            if (cv.IsTapped)
            {
                cv.IsTapped = false;
                cv.particleGlow.gameObject.SetActive(false);
                GameManager.Instance.ChangeCardColour(card, cv.cardBackground.color);
                GameManager.Instance.MinionsAttacking.Remove(card);
            }
            else
            {
                cv.IsTapped = true;
                //GameManager.Instance.ChangeCardColour(card, Color.cyan);
                cv.particleGlow.gameObject.SetActive(true);
                GameManager.Instance.MinionsAttacking.Add(card);
            }

            HasAbilites(cv);
        }
        else if (cv.Sd != null)
        {
            if (cv.IsTapped)
            {
                cv.IsTapped = false;
                GameManager.Instance.ChangeCardColour(card, Color.gray);
                cv.particleGlow.gameObject.SetActive(false);

                //Decrease damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) - cv.Sd.AttackDamage).ToString();
                GameManager.Instance.MinionsAttacking.Remove(card);
            }
            else
            {
                cv.IsTapped = true;
                //GameManager.Instance.ChangeCardColour(card, Color.cyan);
                cv.particleGlow.gameObject.SetActive(true);

                //Increase damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) + cv.Sd.AttackDamage).ToString();
                GameManager.Instance.MinionsAttacking.Add(card);
            }
        }
    }

    private void HasAbilites(CardVisual cv)
    {
        if (cv.Md.EffectId1 == 8 && cv.IsCombatEffectActivated)
        {
            if (cv.IsTapped)
            {
                GameManager.Instance.alliedStealthDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text) + cv.Md.AttackDamage).ToString();
            }
            else
            {
                GameManager.Instance.alliedStealthDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text) - cv.Md.AttackDamage).ToString();
            }
        }
        else if (cv.Md.EffectId1 == 10 && cv.IsCombatEffectActivated)
        {
            if (cv.IsTapped)
            {
                GameManager.Instance.alliedLifestealDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedLifestealDamageCounter.text) + cv.Md.AttackDamage).ToString();
            }
            else
            {
                GameManager.Instance.alliedLifestealDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedLifestealDamageCounter.text) - cv.Md.AttackDamage).ToString();
            }
        }
        else if (cv.Md.EffectId1 == 7 && cv.IsCombatEffectActivated)
        {
            if (cv.IsTapped)
            {
                GameManager.Instance.alliedPoisonTouchDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedPoisonTouchDamageCounter.text) + cv.Md.AttackDamage).ToString();
            }
            else
            {
                GameManager.Instance.alliedPoisonTouchDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedPoisonTouchDamageCounter.text) - cv.Md.AttackDamage).ToString();
            }
        }
        else
        {
            if (cv.IsTapped)
            {
                //Increase damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) + cv.CurrentDamage).ToString();
            }
            else
            {
                //Decrease damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) - cv.CurrentDamage).ToString();
            }
        }
    }
}
