using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionListener : MonoBehaviour, IListener
{
    private MinionData md;
    private GameObject card;    

    public MinionData Md { get => md; set => md = value; }
    public GameObject Card { get => card; set => card = value; }

    public Dictionary<int, EVENT_TYPE> Conditions;
    public Dictionary<int, EVENT_TYPE> Effects;

    public void Start()
    {
        Conditions = new Dictionary<int, EVENT_TYPE>()
        {
            { 1, EVENT_TYPE.ASSIGN_BLEED }
        };

        Effects = new Dictionary<int, EVENT_TYPE>()
        {
            { 1, EVENT_TYPE.ASSIGN_DRAW_CARD },
            { 2, EVENT_TYPE.ASSIGN_PEEK_SHOP }
        };

        EventManager.Instance.AddListener(EVENT_TYPE.ASSIGN_CONDITIONS, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        //Conditions
        //1,"bleed"
        //2,"buy-first-card"
        //3,"minion-defeated"
        //4,"tap"
        //5,"change-shop"
        //6,"action-draw"
        //7,"passive"

        //Effects
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

        if (md != null)
        {
            if (md.ConditionID != 0 && md.EffectId1 != 0)
            {
                EventManager.Instance.PostNotification(Conditions[md.ConditionID]);
                EventManager.Instance.PostNotification(Effects[md.EffectId1]);
            }
        }
    }

    public void AttackHero()
    {
        GameManager.Instance.topHero.AdjustHealth(1, false);

        EventManager.Instance.PostNotification(EVENT_TYPE.BLEED);
    }
}
