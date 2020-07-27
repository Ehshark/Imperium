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
                GameObject tmp = GameManager.Instance.SpawnCard(cardGroup.transform, md, true);
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
            CardVisual cv = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<CardVisual>();
            MinionData minion = cv.Md;

            if (tapped)
            {
                card.GetComponent<PeekShopEventStarter>().moveCards.Remove(minion);
                eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(false);
                GameManager.Instance.ChangeCardColour(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject, cv.cardBackground.color);
                tapped = false;                
            }
            else
            {
                card.GetComponent<PeekShopEventStarter>().moveCards.Add(minion);
                //GameManager.Instance.ChangeCardColour(eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject, Color.cyan);
                eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);
                tapped = true;                
            }
        }
    }
}
