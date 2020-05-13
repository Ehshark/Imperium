using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPE { 
    SACRIFICE_MINION,
    SACRIFICE_SELECTED,
    DESTROY_MINION,
    TAP_MINION,
    START_COMBAT
}

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type);
}
