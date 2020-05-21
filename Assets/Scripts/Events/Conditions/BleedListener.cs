using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedListener : MonoBehaviour, IListener
{
    public MinionData md;
    private GameObject card;

    public MinionData Md { get => md; set => md = value; }
    public GameObject Card { get => card; set => card = value; }

    public void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.BLEED, this);
    }

    public void OnEvent(EVENT_TYPE Bleed)
    {
        Debug.Log("In event");

        //Handle: 1, 2, 3, 5, 13, 15, 16, 17
        if (md.EffectId1 == 1)
        {
            card.GetComponent<DrawCardListener>().StartEvent();
        }
        else if (md.EffectId1 == 2)
        {
            card.GetComponent<PeekShopEventStarter>().StartEvent();
        }
        else if (md.EffectId1 == 3)
        {
            //EventManager.Instance.PostNotification(EVENT_TYPE.CHANGE_SHOP);
        }
        else if (md.EffectId1 == 5)
        {
            //EventManager.Instance.PostNotification(EVENT_TYPE.RECYCLE);
        }
        else if (md.EffectId1 == 13)
        {
            //EventManager.Instance.PostNotification(EVENT_TYPE.SHOCK);
        }
        else if (md.EffectId1 == 15)
        {
            //EventManager.Instance.PostNotification(EVENT_TYPE.CARD_DISCARD);
        }
        else if (md.EffectId1 == 16)
        {
            //EventManager.Instance.PostNotification(EVENT_TYPE.LOOT);
        }
        else if (md.EffectId1 == 17)
        {
            //EventManager.Instance.PostNotification(EVENT_TYPE.TRASH);
        }
    }
}
