using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecycleListener : MonoBehaviour
{
    public GameObject AllyDiscardList;
    private Transform DiscardPile;
    public Button recycleButton;
    private int selectedCardNum = 0;
    private string message;
    private List<Card> deckList = new List<Card>();


    public void StartEvent()
    {


        SetupRecycle();
        recycleButton = GameManager.Instance.alliedDiscardUI.transform.Find("RecycleButton").GetComponent<Button>();
        recycleButton.onClick.AddListener(RecycleConfirmButton);
        DiscardPile = GameManager.Instance.alliedDiscardUI.transform.Find("CardPile/Cards");


        foreach (Transform t in DiscardPile)
        {
            t.gameObject.AddComponent<RecycleListenerAssigner>();
        }
        //recycle button add the clicked card as reference. and put it top of the deck? 
    }

    public void SetupRecycle()
    {
        GameObject tmp;
        SetupRecycleUI(true);


        //show the discardpile for player
        UIManager.Instance.DisplayAllyDiscards();

        //move discard pile to recycle
        foreach (GameObject g in GameManager.Instance.GetActiveDiscardPileList(true))
        {

            if (!g.name.Equals("DiscardPileText"))
            {
                tmp = g;
                //tmp.transform.SetParent(GameManager.Instance.alliedDiscardUI.transform.Find("CardPile/Cards"));
                //tmp.transform.position = GameManager.Instance.alliedDiscardUI.transform.Find("CardPile/Cards").position;
                GameManager.Instance.MoveCard(tmp, GameManager.Instance.alliedDiscardUI.transform.Find("CardPile/Cards"));
            }
        }

    }



    //Adjusts the ui for discard pile and becomes a recycle ui pass true for recycle then false for turning it back to regular 
    public void SetupRecycleUI(bool m)
    {
        if (m)
        {
            //turn off exit exit attach recycle button and change title
            foreach (Transform t in GameManager.Instance.alliedDiscardUI)
            {
                if (t.name.Equals("ExitButton"))
                {
                    t.gameObject.SetActive(false);
                }
                if (t.name.Equals("RecycleButton"))
                {
                    t.gameObject.SetActive(true);
                }
                //TODO: change name to recycle
                if (t.name.Equals("DiscardListTitle/Text"))
                {

                }
            }
        }
        else
        {
            //turn on exit button 
            foreach (Transform t in GameManager.Instance.alliedDiscardUI)
            {
                if (t.name.Equals("ExitButton"))
                {
                    t.gameObject.SetActive(true);
                }
                if (t.name.Equals("RecycleButton"))
                {
                    t.gameObject.SetActive(false);
                }
                //TODO: change name to recycle
                if (t.name.Equals("DiscardListTitle/Text"))
                {

                }
            }
        }

    }

    public void RecycleConfirmButton()
    {
        CardVisual cv = UIManager.Instance.LastSelectedCard.GetComponent<CardVisual>();

    SetupRecycleUI(false);
        //submit button is pressed
        //turn off the UI
        // move the card to the top of the deck 

        //move the selected card to top of the deck. 
        Debug.Log(UIManager.Instance.LastSelectedCard);
        deckList = UIManager.Instance.GetActiveDeckList(true);
        Debug.Log(deckList + "deck list");

        if (cv.Md)
        {
            deckList.Insert(0, cv.Md);
        }
        if (cv.Ed)
        {
            deckList.Insert(0, cv.Ed);
        }
        if (cv.Sd)
        {
            deckList.Insert(0, cv.Sd);
        }

        for ( int i = 0; i < GameManager.Instance.GetActiveDiscardPile(true).childCount; i++)
        {
            Debug.Log(UIManager.Instance.LastSelectedCard.transform.parent.GetChild(i) + "last sel");
            Transform t = UIManager.Instance.LastSelectedCard.transform.parent.GetChild(i);

            if (t == UIManager.Instance.LastSelectedCard)
            {
                Destroy(GameManager.Instance.GetActiveDiscardPile(true).transform.GetChild(i));
            }
        }

        GameManager.Instance.alliedDiscardUI.gameObject.SetActive(false);
        Debug.Log(UIManager.Instance.GetActiveDeckList(true));

 
        //message = "Must select atleast 1 card";
        //StartCoroutine(GameManager.Instance.SetInstructionsText(message));


    }

   
}
    //    Add a UI button to the scene that satisfies the condition of this minion.
    //When the condition is satisfied, the discard pile should appear in an overlay on the screen.
    //When the player selects a card, that card goes on top of their deck and stays face up(real opponent will see it as face down).

    //condition -> draw card during action phase
    //effect => recycle: select a card from discard pile goes on top of your deck
    //Debug.Log();
    //UIManager.Instance. 

