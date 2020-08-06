using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockListenerStarter : MonoBehaviour
{
    const byte ADJUST_HERO_HEALTH_SYNC_EVENT = 49;

    public void StartEvent()
    {
        //No Minions
        if (GameManager.Instance.GetActiveMinionZone(false).childCount == 0)
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Opponent's Hero Shocked!"));

            //Shock oppenent's Hero
            GameManager.Instance.ActiveHero(false).AdjustHealth(1, false);

            //Multiplayer
            object[] data = new object[] { 1 };
            EffectCommandPun.Instance.SendData(ADJUST_HERO_HEALTH_SYNC_EVENT, data);

            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();

        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Select a Minion to Shock"));

            foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
            {
                t.gameObject.AddComponent<ShockListener>();
            }
        }

        ////Call the Next Effect in the Queue
        //InvokeEventCommand.InvokeNextEvent();
    }
}
