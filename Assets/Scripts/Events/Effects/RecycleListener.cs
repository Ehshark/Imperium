using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class RecycleListener : MonoBehaviour
{
    private GameObject sc;
    public void StartEvent()
    {

        //when the graveyard is open
        UIManager.Instance.DisplayAllyDiscards();

        //attach a button called recycle
        //get the list of cards in discard pile
        //set discard pile close button inactive
        //activate recycle button
        //change text to Recycle on title

        foreach (GameObject adp in GameManager.Instance.alliedDiscardPileList)
        {
            Debug.Log("im in adp");
            Debug.Log(adp);
        }

        Debug.Log("im in draw card");




        //put script recyclefromgraveyard on all the cards

        //when card is clicked highlight
        
        //unattach recycle button
        //revert text to Discardpile
    }

    //    Add a UI button to the scene that satisfies the condition of this minion.
    //When the condition is satisfied, the discard pile should appear in an overlay on the screen.
    //When the player selects a card, that card goes on top of their deck and stays face up(real opponent will see it as face down).

    //condition -> draw card during action phase
    //effect => recycle: select a card from discard pile goes on top of your deck
    //Debug.Log();
    //UIManager.Instance. 
}
