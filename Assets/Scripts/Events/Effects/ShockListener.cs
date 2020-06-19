using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShockListener : MonoBehaviour, IPointerDownHandler
{
    public void Start()
    {
        GameManager.Instance.ChangeCardColour(gameObject, Color.cyan);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        cv.AdjustHealth(1, false);

        foreach(Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            ShockListener sl = t.GetComponent<ShockListener>();
            if (sl)
            {
                Destroy(sl);
            }
        }

        EffectCommand.ContinueExecution();
    }

    public void OnDestroy()
    {
        CardVisual cv = gameObject.GetComponent<CardVisual>();
        if (cv.Md != null)
        {
            GameManager.Instance.ChangeCardColour(gameObject, cv.Md.Color);
        }
        else if (cv.Sd != null)
        {
            GameManager.Instance.ChangeCardColour(gameObject, cv.Sd.Color);
        }
    }
}
