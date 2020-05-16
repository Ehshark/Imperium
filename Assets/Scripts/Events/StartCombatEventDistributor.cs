using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombatEventDistributor : MonoBehaviour, IListener
{
    [SerializeField]
    private GameObject heroImage;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.START_COMBAT, this);   
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        foreach (Transform t in GameManager.Instance.alliedMinionZone)
        {
            t.gameObject.AddComponent<StartCombatListener>();
        }

        heroImage.AddComponent<StartCombatHeroListener>();
    }

    
}
