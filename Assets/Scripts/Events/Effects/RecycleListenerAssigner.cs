using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleListenerAssigner : MonoBehaviour, IListener
{
    public void OnEvent(EVENT_TYPE Event_Type)
    {
       foreach (Transform t in GameManager.Instance.GetActiveDiscardPile(true))
        {
            t.gameObject.AddComponent<RecycleListener>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.RECYCLE_LISTENER_ASSIGNER, this);
    }


}
