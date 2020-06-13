﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVENT_TYPE
{
    SACRIFICE_MINION,
    SACRIFICE_SELECTED,
    DESTROY_MINION,
    TAP_MINION,
    START_COMBAT,
    ATTACK,
    ASSIGN_CONDITIONS,
    BLEED,
    ACTION_DRAW,
    BUY_FIRST_CARD,
    FIRST_CHANGE_SHOP,
    DISCARD_CARD,
    DEFEND_AGAINST,
    LEVEL_UP,
    POWER_EXPRESS_BUY,
    POWER_SILENCE,
    POWER_RECYCLE,
    POWER_UNTAP,
    POWER_POISON_TOUCH,
    POWER_PEEK_SHOP,
    POWER_HEAL_MINION,
    POWER_BUFF_MINION,
    POWER_TRASH,
    POWER_STEALTH
}

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type);
}
