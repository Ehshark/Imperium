using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardListener : MonoBehaviour
{
    public void StartEvent()
    {
        //Debug.Log("Draw Card Effect");
        StartCoroutine(GameManager.Instance.SetInstructionsText("Draw Card Effect"));

        GameManager.Instance.DrawCard(UIManager.Instance.GetActiveDeckList(true), GameManager.Instance.GetActiveHand(true));

        //Call the Next Power in the Queue
        InvokeEventCommand.InvokeNextEvent();
    }
}
