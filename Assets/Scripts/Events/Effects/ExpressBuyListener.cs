using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressBuyListener : MonoBehaviour
{
  public void StartEvent()
    {
        if (!GameManager.Instance.HasExpressBuy)
        {
            GameManager.Instance.HasExpressBuy = true;
            GameManager.Instance.expressBuyImage.gameObject.SetActive(true);
        }
    }
}
