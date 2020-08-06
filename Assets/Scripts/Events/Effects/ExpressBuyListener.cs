using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressBuyListener : MonoBehaviour
{
    public void StartEvent()
    {
        if (!GameManager.Instance.HasExpressBuy)
        {
            StartCoroutine(ShowEffectAnimation());

            GameManager.Instance.HasExpressBuy = true;
            GameManager.Instance.expressBuyImage.gameObject.SetActive(true);

            //Call the Next Power in the Queue
            InvokeEventCommand.InvokeNextEvent();
        }
    }
    private IEnumerator ShowEffectAnimation()
    {
        GameManager.Instance.effectText.gameObject.SetActive(true);
        GameManager.Instance.effectText.text = "Express Buy Activated!";
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.effectText.text = "Effect Activated!";
        GameManager.Instance.effectText.gameObject.SetActive(false);
    }
}
