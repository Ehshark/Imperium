using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SacrificeMinionListener : MonoBehaviour, IPointerDownHandler
{
    public void Start()
    {
        //TODO: Highlight this minion properly
        CardVisual mv = gameObject.GetComponent<CardVisual>();
        mv.cardBackground.color = Color.cyan;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.MinionToSacrifice = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        EventManager.Instance.PostNotification(EVENT_TYPE.SACRIFICE_SELECTED);
    }

    private void OnDestroy()
    {
        CardVisual mv = gameObject.GetComponent<CardVisual>();
        mv.cardBackground.color = Color.white;
    }
}
