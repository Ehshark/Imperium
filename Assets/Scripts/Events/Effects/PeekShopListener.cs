using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PeekShopListener : MonoBehaviour, IPointerDownHandler
{
    public string minionClass = "";
    public bool inDeck = true;

    private bool tapped;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (inDeck)
        {
            //Get the Top Cards
            List<MinionData> data = UIManager.Instance.GetTopCardsFromDeck(minionClass);
            //Get the Card Group
            GameObject cardGroup = GameManager.Instance.shop.gameObject.GetComponent<ShopController>().cardGroup;

            //Spawn Card
            foreach (MinionData md in data)
            {
                GameObject tmp = GameManager.Instance.SpawnCard(cardGroup.transform, md, null, null, true);
                tmp.AddComponent<PeekShopListener>();
                tmp.GetComponent<PeekShopListener>().inDeck = false;
                tmp.GetComponent<PeekShopListener>().minionClass = minionClass;
            }

            //Destroy Listener Scripts 
            GameManager.Instance.GetComponent<PeekShopEventStarter>().DestroyPeekShopListener();
            GameManager.Instance.GetComponent<PeekShopEventStarter>().moveButton.interactable = true;
        }
        else
        {
            MinionData card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<CardVisual>().Md;

            if (tapped)
            {
                GameManager.Instance.GetComponent<PeekShopEventStarter>().moveCards.Remove(card);
                GameManager.Instance.ChangeCardColour(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject, card.Color);
                tapped = false;                
            }
            else
            {
                GameManager.Instance.GetComponent<PeekShopEventStarter>().moveCards.Add(card);
                GameManager.Instance.ChangeCardColour(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject, Color.cyan);
                tapped = true;                
            }
        }
    }
}
