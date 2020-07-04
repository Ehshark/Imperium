using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuffMinionListener : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        StartCoroutine(GameManager.Instance.SetInstructionsText("Minion's Damage Increased by 1"));

        //Adjust Damage 
        cv.AdjustDamage(1, true);
        //Remove Listener 
        StartCoroutine(RemoveListener());
    }

    IEnumerator RemoveListener()
    {
        yield return new WaitForSeconds(2);

        GameManager.Instance.EnableOrDisablePlayerControl(true);

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            BuffMinionListener bml = t.gameObject.GetComponent<BuffMinionListener>();

            if (bml)
            {
                Destroy(bml);
            }
        }

        //Add the power to the queue
        EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_BUFF_MINION);

        //Call the Next Power in the Queue
        InvokeEventCommand.InvokeNextEvent();
    }
}
