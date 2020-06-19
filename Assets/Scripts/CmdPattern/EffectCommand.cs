using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectCommand : MonoBehaviour
{
    public static Queue<EVENT_TYPE> EffectQueue = new Queue<EVENT_TYPE>();
    public static bool inEffect = false;

    public static void StartCommandExecution()
    {
        if (!inEffect)
        {
            EVENT_TYPE effect = EffectQueue.Dequeue();
            Debug.Log(effect);
            inEffect = true;
            EventManager.Instance.PostNotification(effect);
        }
    }

    public static void ContinueExecution()
    {
        inEffect = false;

        if (EffectQueue.Count != 0)
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveMinionZone(true));
            dc.AddToQueue();

            StartCommandExecution();
        }
    }
}
