using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardCardListener : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();
        Discard(cv, card);
    }

    //Discard function, removes card object and card data from hand and adds them to the discard pile
    //TODO: add logic for enemy side
    //NOTE: can possibly be moved to GameManager.cs 
    public void Discard(CardVisual cv, GameObject card)
    {
        if(cv.Md)
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

        GameManager.Instance.MoveCard(card, GameManager.Instance.alliedHand, GameManager.Instance.alliedDiscardPile);
        GameManager.Instance.alliedDiscardPileList.Add(card);
        Debug.Log("card object discarded");

        foreach (Transform t in GameManager.Instance.alliedHand)
        {
            //Destroy(t.gameObject.GetComponent<DiscardCardListener>());
        }
    }
}
