using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public static Queue<Command> CommandQueue = new Queue<Command>();
    public static bool playingQueue = false;

    public virtual void AddToQueue()
    {
        CommandQueue.Enqueue(this);
        if (!playingQueue)
        {
            GameManager.Instance.EnableOrDisablePlayerControl(false);
            PlayFirstCommandFromQueue();
        }
    }

    public virtual void StartCommandExecution()
    {
        // list of everything that we have to do with this command (draw a card, play a card, play spell effect, etc...)
        // there are 2 options of timing : 
        // 1) use tween sequences and call CommandExecutionComplete in OnComplete()
        // 2) use coroutines (IEnumerator) and WaitFor... to introduce delays, call CommandExecutionComplete() in the end of coroutine
    }

    public virtual void CommandExecutionComplete()
    {
        Debug.Log("Inside command execution complete");
        if (CommandQueue.Count > 0)
            PlayFirstCommandFromQueue();
        else
        {
            playingQueue = false;
            GameManager.Instance.EnableOrDisablePlayerControl(true);
        }
    }

    public static void PlayFirstCommandFromQueue()
    {
        playingQueue = true;
        CommandQueue.Dequeue().StartCommandExecution();
    }
}
