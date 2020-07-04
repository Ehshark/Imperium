using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressBuyListener : MonoBehaviour
{
  public void StartEvent()
    {
        if (!GameManager.Instance.HasExpressBuy)
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Express Buy Enabled"));

            GameManager.Instance.HasExpressBuy = true;
            GameManager.Instance.expressBuyImage.gameObject.SetActive(true);

            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();
        }
    }
}
