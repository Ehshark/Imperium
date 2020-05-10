using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeEventDistributor : MonoBehaviour, IListener
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SACRIFICE_MINION, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        if (GameManager.Instance.IsPromoting)
            foreach (Transform t in GameManager.Instance.alliedMinionZone)
                t.gameObject.AddComponent<SacrificeMinionListener>();
        else
        {
            foreach (Transform t in GameManager.Instance.alliedMinionZone)
            {
                SacrificeMinionListener sml = t.GetComponent<SacrificeMinionListener>();
                if (sml)
                    Destroy(sml);
            }
        }
    }
}
