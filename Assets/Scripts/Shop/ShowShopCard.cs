using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowShopCard : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;        

        if (card.GetComponent<CardVisual>() != null)
        {
            GameManager.Instance.shop.GetComponent<ShopController>().UpdateShopCard(card);
        }
    }
}
