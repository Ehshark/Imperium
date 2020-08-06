using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShopListener : MonoBehaviour
{
    public void StartEvent()
    {
        DisableShop();
    }

    private void DisableShop()
    {
        GameManager.Instance.IsEffect = true;
        GameManager.Instance.shop.gameObject.SetActive(true);
        GameManager.Instance.buyButton.interactable = false;
        GameManager.Instance.exitShopButton.interactable = false;

        GameManager.Instance.shop.gameObject.GetComponent<ShopController>().Card = gameObject;

        foreach (Transform t in GameManager.Instance.essentialsPile)
        {
            ShowShopCard ssc = t.gameObject.GetComponent<ShowShopCard>();
            
            if (ssc)
            {
                Destroy(ssc);
            }
        }
    }

    public void EnableShop()
    {
        GameManager.Instance.IsEffect = false;
        GameManager.Instance.shop.gameObject.SetActive(false);
        GameManager.Instance.buyButton.interactable = true;
        GameManager.Instance.exitShopButton.interactable = true;

        foreach (Transform t in GameManager.Instance.essentialsPile)
        {
            t.gameObject.AddComponent<ShowShopCard>();
        }

        UIManager.Instance.RemoveEffectIcon = true;
    }
}
