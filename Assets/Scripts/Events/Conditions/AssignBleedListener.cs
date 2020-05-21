using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignBleedListener : MonoBehaviour, IListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.ASSIGN_BLEED, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        MinionData md = GameManager.Instance.GetComponent<ConditionListener>().Md;
        GameObject card = GameManager.Instance.GetComponent<ConditionListener>().Card;

        card.AddComponent<BleedListener>();
        card.GetComponent<BleedListener>().Md = md;
        card.GetComponent<BleedListener>().Card = card;
    }

}
