﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPE { 
    SACRIFICE_MINION,
    SACRIFICE_SELECTED,
    DESTROY_MINION,
    TAP_MINION,
    START_COMBAT,
    ATTACK,
    ASSIGN_CONDITIONS,
    ASSIGN_BLEED,
    BLEED,
    ASSIGN_DRAW_CARD,
    ASSIGN_PEEK_SHOP
}

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type);
}
