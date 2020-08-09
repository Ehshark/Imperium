using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialListener : MonoBehaviour
{
    private string type;
    public string Type { get => type; set => type = value; }

    const byte ADJUST_GOLD_SYNC_EVENT = 47;
    const byte ADJUST_EXP_SYNC_EVENT = 48;

    public void StartEvent()
    {
        if (type != null || type != "")
        {
            if (type == "Gold")
            {
                GameManager.Instance.ActiveHero(true).AdjustGold(2, true);

                if (!StartGameController.Instance.tutorial)
                {
                    object[] data = new object[] { 2 };
                    EffectCommandPun.Instance.SendData(ADJUST_GOLD_SYNC_EVENT, data);
                }
            }
            else if (type == "Exp")
            {
                GameManager.Instance.ActiveHero(true).GainExp(2);

                object[] data = new object[] { 2 };
                EffectCommandPun.Instance.SendData(ADJUST_EXP_SYNC_EVENT, data);
            }

            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();
        }
    }
}
