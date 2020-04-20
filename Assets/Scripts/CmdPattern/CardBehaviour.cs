using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour, IPointerDownHandler
{
    public Transform minionZone;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.transform.parent.name != "AlledMinionsPanel")
            this.transform.SetParent(minionZone);
    }
}
