using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashListener : MonoBehaviour, IPointerDownHandler
{
    public int index;
    public string location;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        if (cv.Md != null || cv.Sd != null || cv.Ed != null)
        {
            if (GameManager.Instance.trashUI.GetComponent<TrashController>().Card != null)
            {
                CardVisual prevCv = GameManager.Instance.trashUI.GetComponent<TrashController>().Card.GetComponent<CardVisual>();
                ChangeColourOnPrevCard(GameManager.Instance.trashUI.GetComponent<TrashController>().Card, prevCv);
            }

            Card cd = card.GetComponent<CardVisual>().CardData;

            GameManager.Instance.trashUI.GetComponent<TrashController>().CardToRemove = cd;
            GameManager.Instance.trashUI.GetComponent<TrashController>().Card = card;
            GameManager.Instance.trashUI.GetComponent<TrashController>().removeButton.interactable = true;

            //GameManager.Instance.ChangeCardColour(card, Color.cyan);
            cv.particleGlow.gameObject.SetActive(true);
        }
    }

    public void ChangeColourOnPrevCard(GameObject card, CardVisual cv)
    {
        if (cv.Md != null)
        {
            GameManager.Instance.ChangeCardColour(card, cv.Md.Color);
        }
        else if (cv.Sd != null)
        {
            GameManager.Instance.ChangeCardColour(card, cv.Sd.Color);
        }
        else if (cv.Ed != null)
        {
            GameManager.Instance.ChangeCardColour(card, cv.Ed.Color);
        }
    }
}
