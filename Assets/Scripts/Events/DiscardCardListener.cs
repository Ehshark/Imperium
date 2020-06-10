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
    //TODO: add logic for enemy side
    public void Discard(CardVisual cv, GameObject card)
    {
        int discardNum = UIManager.Instance.allyHand.Count - GameManager.Instance.ActiveHero(true).HandSize;
        //&& GameManager.Instance.selectedDiscards.Count < discardNum
        if (!GameManager.Instance.selectedDiscards.Contains(card))
        {
            GameManager.Instance.selectedDiscards.Add(card);
            GameManager.Instance.ChangeCardColour(card, Color.cyan);
            Debug.Log("card added to selected discard and color changed");
        }
        else
        {
            GameManager.Instance.selectedDiscards.Remove(card);
            GameManager.Instance.ChangeCardColour(card, cv.CardData.Color);
            Debug.Log("card removed and color reset OR not added because selected discards are full");
        }


        //GameManager.Instance.DiscardCard(card);
        /*if(cv.Md)
        {
            //GameManager.Instance.ChangeCardColour(card, cv.Md.Color);
            UIManager.Instance.allyHand.Remove(cv.Md);
            UIManager.Instance.allyDiscards.Add(cv.Md);
            Debug.Log("minion data discarded");
        }
        else if(cv.Sd)
        {
            //GameManager.Instance.ChangeCardColour(card, cv.Sd.Color);
            UIManager.Instance.allyHand.Remove(cv.Sd);
            UIManager.Instance.allyDiscards.Add(cv.Sd);
            Debug.Log("starter data discarded");
        }
        else
        {
            //GameManager.Instance.ChangeCardColour(card, cv.Ed.Color);
            UIManager.Instance.allyHand.Remove(cv.Ed);
            UIManager.Instance.allyDiscards.Add(cv.Ed);
            Debug.Log("essential data discarded");
        }

        GameManager.Instance.MoveCard(card, GameManager.Instance.GetActiveDiscardPile(true), GameManager.Instance.alliedDiscardPileList);
        GameManager.Instance.alliedDiscardPileList.Add(card);
        Debug.Log("card object discarded");

        foreach (Transform t in GameManager.Instance.alliedHand)
        {
            //Destroy(t.gameObject.GetComponent<DiscardCardListener>());
        }*/
    }
}
