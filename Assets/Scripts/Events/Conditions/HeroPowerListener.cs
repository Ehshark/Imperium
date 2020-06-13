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

        GameManager.Instance.ActiveHero(false).AdjustHealth(1, false);
    }
}
