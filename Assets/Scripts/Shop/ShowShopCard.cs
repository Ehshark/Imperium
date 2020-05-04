using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowShopCard : MonoBehaviour, IPointerDownHandler
{
    public GameObject rightShopUI;

    private ShopController shop;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!rightShopUI.activeSelf)
        {
            rightShopUI.SetActive(true);
            Debug.Log(eventData.selectedObject);
            //shop.UpdateShop(eventData.selectedObject.GetComponent<MinionVisual>());
        }
    }
}
