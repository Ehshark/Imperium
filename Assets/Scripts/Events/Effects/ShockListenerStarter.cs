using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockListenerStarter : MonoBehaviour
{
    public void StartEvent()
    {
        //No Minions
        if (GameManager.Instance.GetActiveMinionZone(false).childCount == 0)
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Opponent's Hero Shocked!"));

            //Shock oppenent's Hero
            GameManager.Instance.ActiveHero(false).AdjustHealth(1, false);
            EffectCommand.Instance.inEffect = false;
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Select a Minion to Shock"));

            foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
            {
                t.gameObject.AddComponent<ShockListener>();
            }
        }
    }
}
