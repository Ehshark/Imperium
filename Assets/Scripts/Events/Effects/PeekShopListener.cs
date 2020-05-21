using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PeekShopListener : MonoBehaviour, IPointerDownHandler
{
    public string minionClass = "";
    public bool inDeck = true;
    public GameObject card;

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
                tmp.GetComponent<PeekShopListener>().card = this.card;
            }

            //Destroy Listener Scripts 
            card.GetComponent<PeekShopEventStarter>().DestroyPeekShopListener();
            GameManager.Instance.moveButton.interactable = true;
            GameManager.Instance.moveButton.onClick.AddListener(card.GetComponent<PeekShopEventStarter>().MoveCardsToBottom);
        }
        else
        {
            MinionData minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<CardVisual>().Md;

            if (tapped)
            {
                GameManager.Instance.GetComponent<PeekShopEventStarter>().moveCards.Remove(minion);
                GameManager.Instance.ChangeCardColour(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject, minion.Color);
                tapped = false;                
            }
            else
            {
                card.GetComponent<PeekShopEventStarter>().moveCards.Add(minion);
                GameManager.Instance.ChangeCardColour(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject, Color.cyan);
                tapped = true;                
            }
        }
    }
}
