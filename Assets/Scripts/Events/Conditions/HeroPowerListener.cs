using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPowerListener : MonoBehaviour, IListener
{
    private EVENT_TYPE powerEffect;
    public EVENT_TYPE PowerEffect { get => powerEffect; set => powerEffect = value; }

    const byte ADJUST_HERO_HEALTH_SYNC_EVENT = 49;

    public void Awake()
    {
        enabled = false;
    }

    public void OnEnable()
    {
        EventManager.Instance.AddListener(powerEffect, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        Transform parent = gameObject.transform;        

        if (parent == GameManager.Instance.ActiveHero(true).transform)
        {
            StartCoroutine(EffectCommand.Instance.ShowEffectAnimation("Power Activated!"));

            //No Minions
            if (GameManager.Instance.GetActiveMinionZone(false).childCount == 0)
            {
                //Shock oppenent's Hero
                object[] data = new object[] { 1 };
                EffectCommandPun.Instance.SendData(ADJUST_HERO_HEALTH_SYNC_EVENT, data);
                GameManager.Instance.ActiveHero(false).AdjustHealth(1, false);
                EffectCommand.Instance.inEffect = false;
            }
            else
            {
                GameManager.Instance.InHeroPower = true;

                foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
                {
                    t.gameObject.AddComponent<ShockListener>();
                }
            }
        }
    }
}
