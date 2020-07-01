using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InvokeEventCommand : Command
{
    //public static Queue<InvokeEventCommand> InvokeEventQueue = new Queue<InvokeEventCommand>();
    public static int effectsInQueue = 0;

    object cardType;
    MethodInfo method;
    GameObject card;

    public InvokeEventCommand(MethodInfo methodInfo, object type, GameObject c)
    {
        cardType = type;
        method = methodInfo;
        card = c;
    }

    public override void AddToQueue()
    {
        CommandQueue.Enqueue(this);
        effectsInQueue++;
    }

    public override void StartCommandExecution()
    {
        method.Invoke(cardType, new object[] { });
        effectsInQueue--;
    }

    public static void InvokeNextEvent()
    {
        Debug.Log(effectsInQueue);

        if (CommandQueue.Count > 0)
        {
            Command.PlayFirstCommandFromQueue();
        }
    }

    public static void InEffect()
    {
        if (effectsInQueue == 0)
        {
            EffectCommand.Instance.inEffect = false;
        }
    }
}
