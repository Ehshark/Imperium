using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InvokeEventCommand : Command
{
    public static Queue<InvokeEventCommand> InvokeEventQueue = new Queue<InvokeEventCommand>();

    public object cardType;
    public MethodInfo method;
    public GameObject card;

    public InvokeEventCommand(MethodInfo methodInfo, object type, GameObject c)
    {
        cardType = type;
        method = methodInfo;
        card = c;
    }

    public override void AddToQueue()
    {
        InvokeEventQueue.Enqueue(this);
    }

    public override void StartCommandExecution()
    {
        EffectCommand.Instance.InvokeMethod(this);
    }

    public static void InvokeNextEvent()
    {
        if (InvokeEventQueue.Count > 0)
        {
            InvokeEventQueue.Dequeue().StartCommandExecution();
        }
        else
        {
            EffectCommand.Instance.inEffect = false;
        }
    }
}
