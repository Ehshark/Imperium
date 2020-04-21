using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPE { 
    BATTLECRY
}

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type, Component Sender, Object Param = null);
}
