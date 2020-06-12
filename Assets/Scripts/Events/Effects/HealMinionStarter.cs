using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMinionStarter : MonoBehaviour
{
    public void StartEvent()
    {
        bool needToHeal = false;

        //Loop through each Minion and Compare to see if any minion is already at full health 
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            CardVisual cv = t.GetComponent<CardVisual>();

            if (cv.IsPromoted)
            {
                if (cv.CurrentHealth < cv.PromotedHealth)
                {
                    needToHeal = true;
                }
            }
            else
            {
                if (cv.CurrentHealth < cv.TotalHealth)
                {
                    needToHeal = true;
                }
            }
        }

        if (needToHeal)
        {
            //Display Start message
            StartCoroutine(GameManager.Instance.SetInstructionsText("Select one Minion to Heal"));

            //Disable Player Interaction 
            GameManager.Instance.EnableOrDisablePlayerControl(false);

            //Assign each Minion a Listener Script 
            foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
            {
                t.gameObject.AddComponent<HealMinionListener>();
            }
        }
    }
}
