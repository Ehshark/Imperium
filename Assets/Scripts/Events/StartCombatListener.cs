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

        MinionVisual mv = card.GetComponent<MinionVisual>();
        StarterVisual sv = card.GetComponent<StarterVisual>();

        TapMinion(mv, sv, card);
    }

    private void TapMinion(MinionVisual mv, StarterVisual sv, GameObject card)
    {
        if (mv)
        {
            if (tapped)
            {
                StartCombat.ChangeCardColour(card, mv.Md.Color);
                mv.Md.IsTapped = false;
                tapped = false;

                //Decrease damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) - mv.Md.AttackDamage).ToString();
            }
            else
            {
                StartCombat.ChangeCardColour(card, Color.cyan);
                mv.Md.IsTapped = true;
                tapped = true;

                //Increase damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) + mv.Md.AttackDamage).ToString();
            }

            HasAbilites(mv);
        }
        else if (sv)
        {
            Debug.Log(sv.Sd.AttackDamage);

            if (tapped)
            {
                StartCombat.ChangeCardColour(card, Color.gray);
                sv.Sd.IsTapped = false;
                tapped = false;

                //Decrease damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) - sv.Sd.AttackDamage).ToString();
            }
            else
            {
                StartCombat.ChangeCardColour(card, Color.cyan);
                sv.Sd.IsTapped = true;
                tapped = true;

                //Increase damage counter
                GameManager.Instance.alliedDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedDamageCounter.text) + sv.Sd.AttackDamage).ToString(); 
            }
        }
    }

    private void HasAbilites(MinionVisual mv)
    {
        if (mv.Md.EffectId1 == 8)
        {
            if (tapped)
            {
                GameManager.Instance.alliedStealthDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text) + mv.Md.AttackDamage).ToString();
            }
            else
            {
                GameManager.Instance.alliedStealthDamageCounter.text = (Int32.Parse(GameManager.Instance.alliedStealthDamageCounter.text) - mv.Md.AttackDamage).ToString();
            }
        }
    }
}
