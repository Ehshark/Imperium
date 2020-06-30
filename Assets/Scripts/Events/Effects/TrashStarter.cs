using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashStarter : MonoBehaviour
{
    public void StartEvent()
    {
        if (GameManager.Instance.GetActiveDiscardPileList(true).Count != 0 || GameManager.Instance.GetActiveHand(true).childCount != 0)
        {
            GameManager.Instance.trashUI.gameObject.SetActive(true);
        }
    }
}
