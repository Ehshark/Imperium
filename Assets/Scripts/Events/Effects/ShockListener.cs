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

        if (cv.CurrentHealth - 1 == 0)
        {
            EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.MINION_DEFEATED);
        }

        cv.AdjustHealth(1, false);

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            ShockListener sl = t.GetComponent<ShockListener>();
            if (sl)
            {
                Destroy(sl);
            }
        }

        if (GameManager.Instance.InHeroPower)
        {
            GameManager.Instance.InHeroPower = false;
            EffectCommand.Instance.inEffect = false;
        }
        else
        {
            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();
        }
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
