using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCardCommand : Command
{
    readonly GameObject card;
    readonly Transform from;
    readonly Transform to;

    public MoveCardCommand(GameObject m, Transform f, Transform t)
    {
        card = m;
        from = f;
        to = t;
    }

    public override void StartCommandExecution()
    {
        GameManager.Instance.MoveCard(card, to);
        CommandExecutionComplete();
    }
}
