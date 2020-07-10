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
        Transform parent = gameObject.transform;        

        if (parent == GameManager.Instance.ActiveHero(true).transform)
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Power Activated!"));

            //No Minions
            if (GameManager.Instance.GetActiveMinionZone(false).childCount == 0)
            {
                //Shock oppenent's Hero
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
