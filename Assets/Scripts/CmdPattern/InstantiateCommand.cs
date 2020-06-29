using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCommand : Command
{
    readonly Transform to;
    readonly Card cardData;

    public InstantiateCommand(Card c, Transform t)
    {
        cardData = c;
        to = t;
    }

    public override void StartCommandExecution()
    {
        GameManager.Instance.InstantiateCardToHand(cardData, to);

        CommandExecutionComplete();
    }
}
