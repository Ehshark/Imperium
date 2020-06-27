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
        InvokeRepeating("ContinueExecution", 2.0f, 2.5f);
    }

    public void StartCommandExecution()
    {
        if (!inEffect)
        {
            inEffect = true;
            DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true));
            dc.AddToQueue();

            EVENT_TYPE effect = EffectQueue.Dequeue();
            Debug.Log(effect);

            EventManager.Instance.PostNotification(effect);
        }
    }

    public void ContinueExecution()
    {
        if (EffectQueue.Count != 0 && !inEffect)
        {
            StartCommandExecution();
        }
    }
}
