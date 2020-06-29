using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCardCommand : Command
{
    readonly GameObject card;
    readonly Transform to;
    List<GameObject> dpList;

    public MoveCardCommand(GameObject m, Transform t, List<GameObject> dpl = null)
    {
        card = m;
        to = t;
        dpList = dpl;
    }

    public override void StartCommandExecution()
    {
        if (to.name.Equals("MinionArea") || to.name.Equals("EnemyMinionArea"))
        {
            GameManager.Instance.MoveCard(card, to, null, false, true);
        }
        else if (to.name.Equals("DiscardPile") || to.name.Equals("EnemyDiscardPile"))
        {
            GameManager.Instance.MoveCard(card, to, dpList, false, false);
        }
        else
        {
            GameManager.Instance.MoveCard(card, to, null, true, false);
        }

        if (to.name.Equals("Hand") || to.name.Equals("EnemyHand"))
        {
            card.AddComponent<PlayCard>();
        }
        if (!GameManager.Instance.buyButton.interactable)
        {
            GameManager.Instance.buyButton.interactable = true;
        }

        CommandExecutionComplete();
    }
}
