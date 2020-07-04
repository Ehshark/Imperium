using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialListener : MonoBehaviour
{
    private string type;
    public string Type { get => type; set => type = value; }

    public void StartEvent()
    {
        if (type != null || type != "")
        {
            if (type == "Gold")
            {
                GameManager.Instance.ActiveHero(true).AdjustGold(2, true);
            }
            else if (type == "Exp")
            {
                GameManager.Instance.ActiveHero(true).GainExp(2);
            }

            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();
        }
    }
}
