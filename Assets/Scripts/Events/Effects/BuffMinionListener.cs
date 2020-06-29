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

        foreach (Command c in Command.CommandQueue)
        {
            if (c is EffectCommand data)
            {
                EffectCommand ec = data;
                if (ec.EffectId == 14)
                {
                    ec.CommandExecutionComplete();
                }
            }
        }
    }
}
