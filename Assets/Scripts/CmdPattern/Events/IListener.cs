using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPE { 
    DESTROY_MINION
}

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type);
}
