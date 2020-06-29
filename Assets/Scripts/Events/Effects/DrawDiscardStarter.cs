using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawDiscardStarter : MonoBehaviour
{
    public void StartEvent()
    {
        GameManager.Instance.DrawCard(UIManager.Instance.GetActiveDeckList(true), GameManager.Instance.GetActiveHand(true));
        GameManager.Instance.EnableOrDisablePlayerControl(true);

        foreach(Transform t in GameManager.Instance.GetActiveHand(true))
        {
            PlayCard pc = t.GetComponent<PlayCard>();

            if (pc)
            {
                Destroy(pc);
            }

            t.gameObject.AddComponent<DrawDiscardListener>();
        }

        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "Select One Card to Discard";
    }
}
