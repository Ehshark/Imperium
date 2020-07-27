using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardCardListener : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();
        Discard(cv, card);
    }

    //Discard function, removes card object and card data from hand and adds them to the discard pile
    public void Discard(CardVisual cv, GameObject card)
    {
        if (!GameManager.Instance.selectedDiscards.Contains(card))
        {
            GameManager.Instance.selectedDiscards.Add(card);
            //GameManager.Instance.ChangeCardColour(card, Color.cyan);
            cv.particleGlow.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.selectedDiscards.Remove(card);
            cv.particleGlow.gameObject.SetActive(false);
            GameManager.Instance.ChangeCardColour(card, cv.cardBackground.color);
        }
    }
}
