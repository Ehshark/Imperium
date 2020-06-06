using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawDiscardListener : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        GameManager.Instance.DiscardCard(card);

        RemoveListeners();
    }

    public void RemoveListeners()
    {
        foreach(Transform t in GameManager.Instance.GetActiveHand(true))
        {
            DrawDiscardListener ddl = t.GetComponent<DrawDiscardListener>();

            if (ddl)
            {
                Destroy(ddl);
            }

            t.gameObject.AddComponent<PlayCard>();
        }
    }
}
