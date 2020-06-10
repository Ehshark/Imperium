using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealMinionListener : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();

        StartCoroutine(GameManager.Instance.SetInstructionsText("Minion's Health Increased by 1"));

        //Adjust Damage 
        cv.AdjustHealth(1, true);
        //Set Health Flag to true
        cv.HealEffect = true;

        //Remove Listener 
        StartCoroutine(RemoveListener());
    }

    IEnumerator RemoveListener()
    {
        yield return new WaitForSeconds(2);

        GameManager.Instance.EnableOrDisablePlayerControl(true);

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            HealMinionListener hml = t.gameObject.GetComponent<HealMinionListener>();

            if (hml)
            {
                Destroy(hml);
            }
        }
    }
}
