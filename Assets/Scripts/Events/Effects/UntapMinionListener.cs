using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UntapMinionListener : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        UntapMinion(card, cv);
    }

    public void UntapMinion(GameObject card, CardVisual cv)
    {
        if (cv.IsTapped)
        {
            cv.IsTapped = false;
            GameManager.Instance.ChangeCardColour(card, cv.Md.Color);

            //removes all untap listeners from all minions on the active player's board
            foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
            {
                Destroy(t.gameObject.GetComponent<UntapMinionListener>());
            }
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("This Minion is not currently tapped, please select a Minion to untap"));
        }
    }
}
