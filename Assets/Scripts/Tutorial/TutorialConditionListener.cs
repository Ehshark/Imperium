using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialConditionListener : MonoBehaviour, IPointerDownHandler, IListener
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
            { 5, card.GetComponent<RecycleListenerAssigner>() },
            { 6, card.GetComponent<HealMinionStarter>() },
            { 11, card.GetComponent<UntapMinionStarter>() },
            { 13, card.GetComponent<ShockListenerStarter>() },
            { 14, card.GetComponent<BuffMinionStarter>() },
            { 16, card.GetComponent<DrawDiscardStarter>() },
            { 17, card.GetComponent<TrashStarter>() },
            { 18, card.GetComponent<EssentialListener>() },
            { 19, card.GetComponent<EssentialListener>() },
            { 15, card.GetComponent<OpponentDiscardStarter>() },
            { 12, card.GetComponent<SilenceMinionStarter>() }
        };
    }

    public void OnEvent(EVENT_TYPE ConditionEvent)
    {
        CardVisual cv = card.GetComponent<CardVisual>();

        Transform parent = card.transform.parent;

        if (parent == GameManager.Instance.GetActiveMinionZone(true))
        {
            if (md != null)
            {
                if (md.EffectId1 != 0 && !md.IsSilenced)
                {
                    foreach (KeyValuePair<int, object> entry in EffectCardData)
                    {
                        if (entry.Key == md.EffectId1)
                        {
                            Type effectType = EffectCardData.Where(t => t.Key == md.EffectId1).SingleOrDefault().Value.GetType();

                            MethodInfo startEvent = effectType.GetMethod("StartEvent");
                            InvokeEventCommand invokeEvent = new InvokeEventCommand(startEvent, EffectCardData.Where(t => t.Key == md.EffectId1).SingleOrDefault().Value, card);
                            invokeEvent.AddToQueue();
                        }
                        else if (entry.Key == md.EffectId2)
                        {
                            Type effectType = EffectCardData.Where(t => t.Key == md.EffectId2).SingleOrDefault().Value.GetType();

                            MethodInfo startEvent = effectType.GetMethod("StartEvent");
                            InvokeEventCommand invokeEvent = new InvokeEventCommand(startEvent, EffectCardData.Where(t => t.Key == md.EffectId2).SingleOrDefault().Value, card);
                            invokeEvent.AddToQueue();
                        }
                    }
                }

                if (cv.Md.EffectId1 == 7 || cv.Md.EffectId1 == 8 || cv.Md.EffectId1 == 9 || cv.Md.EffectId1 == 10 && !md.IsSilenced)
                {
                    cv.IsCombatEffectActivated = true;
                    cv.CombatEffectActivated(true);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuffMinionListener bml = card.GetComponent<BuffMinionListener>();
        HealMinionListener hml = card.GetComponent<HealMinionListener>();
        ShockListener sl = card.GetComponent<ShockListener>();
        bool disableCondition = StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().disableCondition;

        if (ConditionEvent == EVENT_TYPE.TAP_MINION && !GameManager.Instance.ActiveHero(true).StartedCombat && !bml && !hml && !sl && !disableCondition)
        {
            CardVisual cv = card.GetComponent<CardVisual>();

            if (!cv.IsTapped && !cv.IsSilenced)
            {
                //EffectCommand.Instance.EffectQueue.Enqueue(ConditionEvent);
                OnEvent(ConditionEvent);
                EffectCommand.Instance.EffectQueue.Enqueue(ConditionEvent);
                cv.IsTapped = true;
                cv.TapEffect = true;
                cv.ChangeTappedAppearance();
            }
        }
    }
}
