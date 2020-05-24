using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BleedListener : MonoBehaviour, IListener
{
    private MinionData md;
    private GameObject card;

    public MinionData Md { get => md; set => md = value; }
    public GameObject Card { get => card; set => card = value; }

    public Dictionary<int, object> EffectCardData;

    public void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.BLEED, this);

        //Handle: 1, 2, 3, 5, 13, 15, 16, 17
        EffectCardData = new Dictionary<int, object> {
            { 1, card.GetComponent<DrawCardListener>() },
            { 2, card.GetComponent<PeekShopEventStarter>() }
        };

    }

    public void OnEvent(EVENT_TYPE Bleed)
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
}
