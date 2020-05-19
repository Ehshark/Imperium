using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardListener : MonoBehaviour, IListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.DRAW_CARD, this);
    }

    public void OnEvent(EVENT_TYPE DRAWCARD)
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
