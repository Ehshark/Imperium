using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class RecycleListenerAssigner : MonoBehaviour, IPointerClickHandler
{
 
    bool cardSelected = true;
    Color lastSelectedColor;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = gameObject.GetComponent<CardVisual>();
        CardVisual cvsss = gameObject.GetComponent<CardVisual>();

        if (gameObject != UIManager.Instance.LastSelectedCard && UIManager.Instance.LastSelectedCard != null)
        {
            foreach (Transform t in UIManager.Instance.LastSelectedCard.transform)
            {
                if (cv.cardBackground.color != Color.green)
                {
                    if (cv.Md)
                    {
                        lastSelectedColor = cv.cardBackground.color;
                        GameManager.Instance.ChangeCardColour(card, Color.green);
                    }
                    if (cv.Ed)
                    {
                        lastSelectedColor = cv.cardBackground.color;
                        GameManager.Instance.ChangeCardColour(card, Color.green);
                    }
                    if (cv.Sd)
                    {
                        lastSelectedColor = cv.cardBackground.color;
                        GameManager.Instance.ChangeCardColour(card, Color.green);
                    }
                }
            }
        }
        //if its green and clicked again
            if (cv.cardBackground.color == Color.green)
            {
                if (cv.Md)
                {
                    cv.cardBackground.color = lastSelectedColor;
                }
                if (cv.Ed)
                {
                    cv.cardBackground.color = lastSelectedColor;

                }
                if (cv.Sd)
                {
                    cv.cardBackground.color = lastSelectedColor;
                }
            }
            else
            {
                GameManager.Instance.ChangeCardColour(card, Color.green);
            }
            UIManager.Instance.LastSelectedCard = gameObject;

            Debug.Log(UIManager.Instance.LastSelectedCard + " after func");
        }


   
}