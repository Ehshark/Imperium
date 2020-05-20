using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionListener : MonoBehaviour, IListener
{
    private MinionData md;
    private GameObject card;

    public MinionData Md { get => md; set => md = value; }
    public GameObject Card { get => card; set => card = value; }

    public void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.ASSIGN_CONDITIONS, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        //1,"bleed"
        //2,"buy-first-card"
        //3,"minion-defeated"
        //4,"tap"
        //5,"change-shop"
        //6,"action-draw"
        //7,"passive"

        //Check Conditions
        if (md.ConditionID == 1)
        {
            card.AddComponent<BleedListener>();
            card.GetComponent<BleedListener>().Md = md;
            card.GetComponent<BleedListener>().Card = card;
        }

        //1,"draw-card"
        //2,"peek-shop"
        //3,"change-shop"
        //4,"express-buy"
        //5,"recycle"
        //6,"heal-allied-minion"
        //7,"poison-touch"
        //8,"stealth"
        //9,"vigilance"
        //10,"lifesteal"
        //11,"untap"
        //12,"silence"
        //13,"shock"
        //14,"buff-allied-minion"
        //15,"card-discard"
        //16,"loot"
        //17,"trash"
        //18,"coins"
        //19,"experience"
        //20,"health"
        //21,"mana"

        //Check Effects
        if (md.EffectId1 == 1)
        {
            card.AddComponent<DrawCardListener>();
        }
        else if (md.EffectId1 == 2)
        {
            card.AddComponent<PeekShopEventStarter>();
        }
    }

    //public void AttackHero()
    //{
    //    GameManager.Instance.topHero.AdjustHealth(1, false);

    //    foreach (Transform t in GameManager.Instance.alliedMinionZone)
    //    {
    //        BleedListener bl = t.gameObject.GetComponent<BleedListener>();

    //        if (bl)
    //        {
    //            bl.StartEvent();
    //        }
    //    }
    //}
}
