using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeSelectedListener : MonoBehaviour, IListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SACRIFICE_SELECTED, this);
    }

    public void OnEvent(EVENT_TYPE SACRIFICE_SELECTED)
    {
        PlayCard pc = GameManager.Instance.MinionToPromote.GetComponent<PlayCard>();
        pc.StartPromotion();
    }
}
