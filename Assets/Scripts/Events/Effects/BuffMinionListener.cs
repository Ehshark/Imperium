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

        //Adjust Damage 
        cv.AdjustDamage(1, true);
        //Remove Listener 
        RemoveListener();
    }

    private void RemoveListener()
    {
        int currentPlayer = GameManager.Instance.GetCurrentPlayer();
        GameManager.Instance.EnableOrDisablePlayerControl(true);
        Transform zone;

        if (currentPlayer == 0)
        {
            zone = GameManager.Instance.alliedMinionZone;
        }
        else
        {
            zone = GameManager.Instance.enemyMinionZone;
        }


        foreach (Transform t in zone)
        {
            BuffMinionListener bml = t.gameObject.GetComponent<BuffMinionListener>();

            if (bml)
            {
                Destroy(bml);
            }
        }
    }
}
