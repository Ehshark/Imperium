using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionAndEffectAssigner : MonoBehaviour, IListener
{
    private MinionData md;
    private GameObject card;

    public MinionData Md { get => md; set => md = value; }
    public GameObject Card { get => card; set => card = value; }

    public Dictionary<int, EVENT_TYPE> Conditions;
    public Dictionary<int, string> Effects;

    private EVENT_TYPE condition;
    private string effectScriptName;

    public void Start()
    {
        //Conditions
        //1,"bleed"
        //2,"buy-first-card"
        //3,"minion-defeated"
        //4,"tap"
        //5,"change-shop"
        //6,"action-draw"
        //7,"passive"
        Conditions = new Dictionary<int, EVENT_TYPE>()
        {
            { 1, EVENT_TYPE.BLEED },
            { 2, EVENT_TYPE.BUY_FIRST_CARD },
            { 4, EVENT_TYPE.TAP_MINION },
            { 5, EVENT_TYPE.FIRST_CHANGE_SHOP }
        };

        //Effects - USE THE NAME OF THE SCRIPT YOU WROTE
        //DrawCardListener
        //PeekShopEventStarter
        //...
        //...
        Effects = new Dictionary<int, string>()
        {
            { 1, "DrawCardListener" },
            { 2, "PeekShopEventStarter" },
            { 3, "ChangeShopListener" }
        };

        EventManager.Instance.AddListener(EVENT_TYPE.ASSIGN_CONDITIONS, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        if (md != null)
        {
            if (md.ConditionID != 0 && md.EffectId1 != 0)
            {
                foreach (KeyValuePair<int, EVENT_TYPE> entry in Conditions)
                    if (md.ConditionID == entry.Key)
                        condition = entry.Value;


                card.AddComponent<ConditionListener>();
                card.GetComponent<ConditionListener>().Md = md;
                card.GetComponent<ConditionListener>().ConditionEvent = condition;

                foreach (KeyValuePair<int, string> entry in Effects)
                    if (md.EffectId1 == entry.Key)
                        effectScriptName = entry.Value;

                Type type = System.Type.GetType(effectScriptName + ",Assembly-CSharp");
                card.AddComponent(type);

                card.GetComponent<ConditionListener>().Card = card;
                card.GetComponent<ConditionListener>().enabled = true;
            }

            //TODO: Repeat logic for md.EffectId2
        }
    }

    public void AttackHero()
    {
        GameManager.Instance.topHero.AdjustHealth(1, false);

        EventManager.Instance.PostNotification(EVENT_TYPE.BLEED);
    }
}
