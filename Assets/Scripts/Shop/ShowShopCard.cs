using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowShopCard : MonoBehaviour, IPointerDownHandler
{
    public GameObject shop;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject rightShopUI = shop.GetComponent<ShopController>().rightShopUI;

        if (!rightShopUI.activeSelf)
        {
            rightShopUI.SetActive(true);
        }

        GameObject minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        shop.GetComponent<ShopController>().UpdateShopCard(minion);
    }
}
