using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConditionListener : MonoBehaviour, IListener, IPointerDownHandler
{
    private MinionData md;
    private GameObject card;
    private bool tapped;

    public MinionData Md { get => md; set => md = value; }
    public GameObject Card { get => card; set => card = value; }
    public EVENT_TYPE ConditionEvent { get; set; }

    public Dictionary<int, object> EffectCardData;

    public bool Tapped { get => tapped; set => tapped = value; }

    private void Awake()
    {
        enabled = false;
    }

    public void OnEnable()
    {        
        EventManager.Instance.AddListener(ConditionEvent, this);

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
        EffectCardData = new Dictionary<int, object> {
            { 1, card.GetComponent<DrawCardListener>() },
            { 2, card.GetComponent<PeekShopEventStarter>() },
            { 3, card.GetComponent<ChangeShopListener>() }
        };
    }

    public void OnEvent(EVENT_TYPE ConditionEvent)
    {
        if (md != null)
        {
            if (md.EffectId1 != 0)
            {
                foreach (KeyValuePair<int, object> entry in EffectCardData)
                {
                    if (entry.Key == md.EffectId1)
                    {
                        Type effectType = EffectCardData.Where(t => t.Key == md.EffectId1).SingleOrDefault().Value.GetType();

                        MethodInfo startEvent = effectType.GetMethod("StartEvent");
                        startEvent.Invoke(EffectCardData.Where(t => t.Key == md.EffectId1).SingleOrDefault().Value, new object[] { });
                    }
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ConditionEvent == EVENT_TYPE.TAP_MINION && !GameManager.Instance.ActiveHero().StartedCombat)
        {
            if (!tapped)
            {
                OnEvent(ConditionEvent);
                GameManager.Instance.ChangeCardColour(card, Color.cyan);
                tapped = true;
            }
        }
    }
}
