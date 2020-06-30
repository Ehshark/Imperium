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

    public MinionData Md { get => md; set => md = value; }
    public GameObject Card { get => card; set => card = value; }
    public EVENT_TYPE ConditionEvent { get; set; }

    public Dictionary<int, object> EffectCardData;

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
            { 3, card.GetComponent<ChangeShopListener>() },
            { 4, card.GetComponent<ExpressBuyListener>() },
            { 5, card.GetComponent<RecycleListener>() },
            { 6, card.GetComponent<HealMinionStarter>() },
            { 13, card.GetComponent<ShockListenerStarter>() },
            { 14, card.GetComponent<BuffMinionStarter>() },
            { 16, card.GetComponent<DrawDiscardStarter>() },
            { 17, card.GetComponent<TrashStarter>() }
        };
    }

    public void OnEvent(EVENT_TYPE ConditionEvent)
    {
        CardVisual cv = card.GetComponent<CardVisual>();

        if (md != null)
        {
            if (md.EffectId1 != 0)
            {
                foreach (KeyValuePair<int, object> entry in EffectCardData)
                {
                    if (entry.Key == md.EffectId1)
                    {
                        DelayCommand dc = new DelayCommand(card.transform, 1f);
                        dc.AddToQueue();
                        EffectCommand ec = new EffectCommand(md.EffectId1);
                        ec.AddToQueue();
                        Type effectType = EffectCardData.Where(t => t.Key == md.EffectId1).SingleOrDefault().Value.GetType();

                        MethodInfo startEvent = effectType.GetMethod("StartEvent");
                        startEvent.Invoke(EffectCardData.Where(t => t.Key == md.EffectId1).SingleOrDefault().Value, new object[] { });
                    }
                }
            }

            if (cv.Md.EffectId1 == 7 || cv.Md.EffectId1 == 8 || cv.Md.EffectId1 == 8 || cv.Md.EffectId1 == 8)
            {
                cv.IsCombatEffectActivated = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuffMinionListener bml = card.GetComponent<BuffMinionListener>();
        HealMinionListener hml = card.GetComponent<HealMinionListener>();
        ShockListener sl = card.GetComponent<ShockListener>();

        if (ConditionEvent == EVENT_TYPE.TAP_MINION && !GameManager.Instance.ActiveHero(true).StartedCombat && !bml && !hml && !sl)
        {
            CardVisual cv = card.GetComponent<CardVisual>();

            if (!cv.IsTapped)
            {
                OnEvent(ConditionEvent);
                GameManager.Instance.ChangeCardColour(card, Color.cyan);
                cv.AdjustHealth(1, false);
                cv.IsTapped = true;
            }
        }
    }
}
