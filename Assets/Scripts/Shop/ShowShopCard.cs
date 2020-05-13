using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowShopCard : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;

        if (minion.GetComponent<CardVisual>() != null)
        {
            GameManager.Instance.shop.GetComponent<ShopController>().UpdateShopCard(minion);
        }
    }
}
