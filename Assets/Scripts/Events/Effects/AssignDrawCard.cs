using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignDrawCard : MonoBehaviour, IListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.ASSIGN_DRAW_CARD, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        GameObject card = GameManager.Instance.GetComponent<ConditionListener>().Card;
        card.AddComponent<DrawCardListener>();
    }
}
