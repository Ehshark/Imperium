using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedListener : MonoBehaviour, IListener
{
    public void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.BLEED, this);
    }

    public void OnEvent(EVENT_TYPE BLEED)
    {
        int currentPlayer = GameManager.Instance.GetCurrentPlayer();
        //int currentPlayer = 0;

        foreach (GameObject tmp in GameManager.Instance.MinionsAttacking)
        {
            CardVisual cv = tmp.GetComponent<CardVisual>();

            if (cv.Md != null && cv.Md.ConditionID == 1)
            {
                Debug.Log(cv.Md.EffectText1);

                //Handle: 1, 2, 3, 5, 13, 15, 16, 17
                if (cv.Md.EffectId1 == 1)
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.DRAW_CARD);
                }
                else if (cv.Md.EffectId1 == 2)
                {
                    EventManager.Instance.PostNotification(EVENT_TYPE.PEEK_SHOP);
                }
                else if (cv.Md.EffectId1 == 3)
                {
                    //EventManager.Instance.PostNotification(EVENT_TYPE.CHANGE_SHOP);
                }
                else if (cv.Md.EffectId1 == 5)
                {
                    //EventManager.Instance.PostNotification(EVENT_TYPE.RECYCLE);
                }
                else if (cv.Md.EffectId1 == 13)
                {
                    //EventManager.Instance.PostNotification(EVENT_TYPE.SHOCK);
                }
                else if (cv.Md.EffectId1 == 15)
                {
                    //EventManager.Instance.PostNotification(EVENT_TYPE.CARD_DISCARD);
                }
                else if (cv.Md.EffectId1 == 16)
                {
                    //EventManager.Instance.PostNotification(EVENT_TYPE.LOOT);
                }
                else if (cv.Md.EffectId1 == 17)
                {
                    //EventManager.Instance.PostNotification(EVENT_TYPE.TRASH);
                }
            }
        }
    }

    public void AttackHero()
    {
        int currentPlayer = GameManager.Instance.GetCurrentPlayer();

        if (currentPlayer == 0)
        {
            foreach (Transform t in GameManager.Instance.alliedMinionZone)
            {
                GameManager.Instance.MinionsAttacking.Add(t.gameObject);
            }

            GameManager.Instance.topHero.AdjustHealth(1, false);
        }
        else
        {
            foreach (Transform t in GameManager.Instance.enemyMinionZone)
            {
                GameManager.Instance.MinionsAttacking.Add(t.gameObject);
            }

            GameManager.Instance.bottomHero.AdjustHealth(1, false);
        }

        GameManager.Instance.MinionsAttacking.Clear();
    }
}
