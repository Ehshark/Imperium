using System.Collections;
using System.Collections.Generic;
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

        StartCoroutine(GameManager.Instance.SetInstructionsText("Select One Card to Discard"));
    }
}
