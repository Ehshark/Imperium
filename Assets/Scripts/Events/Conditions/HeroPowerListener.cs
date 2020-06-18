using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPowerListener : MonoBehaviour, IListener
{
    private EVENT_TYPE powerEffect;

    public EVENT_TYPE PowerEffect { get => powerEffect; set => powerEffect = value; }

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
        StartCoroutine(GameManager.Instance.SetInstructionsText("Power Activated!"));

        //No Minions
        if (GameManager.Instance.GetActiveMinionZone(false).childCount == 0)
        {
            //Shock oppenent's Hero
            GameManager.Instance.ActiveHero(false).AdjustHealth(1, false);
        }        
        else
        {
            foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
            {
                t.gameObject.AddComponent<ShockListener>();
            }
        }
    }
}
