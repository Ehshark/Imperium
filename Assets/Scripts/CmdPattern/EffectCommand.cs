using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectCommand : MonoBehaviour
{
    public static EffectCommand Instance { get; private set; } = null;

    public Queue<EVENT_TYPE> EffectQueue = new Queue<EVENT_TYPE>();
    public bool inEffect = false;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(Instance);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        InvokeRepeating("ContinueExecution", 2f, 2f);
    }

    public void StartCommandExecution()
    {
        if (!inEffect)
        {
            inEffect = true;

            EVENT_TYPE effect = EffectQueue.Dequeue();
            Debug.Log(effect);

            EventManager.Instance.PostNotification(effect);

            InvokeEventCommand.InvokeNextEvent();
        }
    }

    public void ContinueExecution()
    {
        if (EffectQueue.Count != 0 && !inEffect)
        {
            if (CanEffectBeCalled())
            {
                StartCommandExecution();
            }
        }
    }

    public bool CanEffectBeCalled()
    {
        bool result = false;
        bool isAttacking = GameManager.Instance.ActiveHero(true).StartedCombat;
        bool shopOpen = GameManager.Instance.shop.gameObject.activeSelf;
        bool isDefending = GameManager.Instance.IsDefending;

        if (!isAttacking && !shopOpen && !isDefending)
        {
            result = true;
        }

        return result;
    }
}
