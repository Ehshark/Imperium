using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UntapMinionListener : MonoBehaviour, IPointerDownHandler
{
    public void Start()
    {
        //GameManager.Instance.ChangeCardColour(gameObject, Color.cyan);
        gameObject.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        UntapMinion(cv);
    }

    public void UntapMinion(CardVisual cv)
    {
        Transform alliedMinions = GameManager.Instance.GetActiveMinionZone(true);
        if (cv.IsTapped)
        {
            cv.IsTapped = false;
            cv.ChangeTappedAppearance();

            //removes all untap listeners from all minions on the active player's board
            foreach (Transform t in alliedMinions)
            {
                Destroy(t.gameObject.GetComponent<UntapMinionListener>());
            }

            InvokeEventCommand.InvokeNextEvent();
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("This Minion is not currently tapped, please select a Minion to untap"));
        }
    }

    public void OnDestroy()
    {
        CardVisual cv = gameObject.GetComponent<CardVisual>();
        GameManager.Instance.ChangeCardColour(gameObject, cv.cardBackground.color);
    }
}
