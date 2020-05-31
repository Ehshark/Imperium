using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardCardEventDistributor : MonoBehaviour, IListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.DISCARD_CARD, this);
    }

    // Update is called once per frame
    public void OnEvent(EVENT_TYPE Event_Type)
    {
        foreach (Transform t in GameManager.Instance.alliedHand)
        {
            t.gameObject.AddComponent<DiscardCardListener>();
        }
    }
}
