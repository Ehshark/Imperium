using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class RecycleListener : MonoBehaviour, IPointerClickHandler
{
    GameObject card;
    CardVisual cv;
    public int index;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
            cv = gameObject.GetComponent<CardVisual>();
            if (cv)
            {
                if (gameObject != UIManager.Instance.LastSelectedCard)
                {
                    cv.particleGlow.gameObject.SetActive(true);
                    UIManager.Instance.LastSelectedCard = gameObject;
                    GameManager.Instance.RemoveCardAtIndex = index;
                }
                else
                {
                    cv.particleGlow.gameObject.SetActive(false);
                    UIManager.Instance.LastSelectedCard = null;
                    GameManager.Instance.RemoveCardAtIndex = -1;
                }
            }
        }
    }
}