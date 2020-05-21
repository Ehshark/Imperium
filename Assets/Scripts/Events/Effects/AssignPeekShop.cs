using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignPeekShop : MonoBehaviour, IListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.ASSIGN_PEEK_SHOP, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        GameObject card = GameManager.Instance.GetComponent<ConditionListener>().Card;
        card.AddComponent<PeekShopEventStarter>();
    }
}
