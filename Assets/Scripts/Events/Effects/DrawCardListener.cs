using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardListener : MonoBehaviour
{
    public void StartEvent()
    {
        Debug.Log("Draw Card Effect");

        int currentPlayer = GameManager.Instance.GetCurrentPlayer();
        //int currentPlayer = 0;

        if (currentPlayer == 0)
        {
            GameManager.Instance.DrawCard(UIManager.Instance.allyDeck, GameManager.Instance.alliedHand);
        }
        else
        {
            GameManager.Instance.DrawCard(UIManager.Instance.enemyDeck, GameManager.Instance.enemyHand);
        }
    }
}
