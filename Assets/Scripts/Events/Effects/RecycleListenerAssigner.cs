using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecycleListenerAssigner : MonoBehaviour
{
    public GameObject AllyDiscardList;
    private Transform DiscardPile;
    public Button recycleButton;
    private List<Card> deckList = new List<Card>();
    TMP_Text titleText;    

    private void Awake()
    {
        titleText = GameManager.Instance.GetActiveDiscardUI(true).Find("DiscardListTitle/Text").GetComponent<TMP_Text>();
    }

    public void StartEvent()
    {
        GameManager.Instance.allyDiscardPileButton.interactable = false;
        GameManager.Instance.enemyDiscardPileButton.interactable = false;
        SetupRecycleUI(true);
        //show the discardpile for player
        if (GameManager.Instance.GetCurrentPlayer() == 0)
        {
            UIManager.Instance.DisplayAllyDiscards();
        }
        else
        {
            UIManager.Instance.DisplayEnemyDiscards();
        }
        recycleButton = GameManager.Instance.recycleButton.GetComponent<Button>();
        recycleButton.onClick.AddListener(RecycleConfirmButton);
        DiscardPile = GameManager.Instance.GetActiveDiscardUI(true).transform.Find("CardPile/Cards");

        int i = 0;
        foreach (Transform t in DiscardPile)
        {
            t.gameObject.AddComponent<RecycleListener>();
            t.gameObject.GetComponent<RecycleListener>().index = i;
            i++;
        }
        //recycle button add the clicked card as reference. and put it top of the deck? 
    }

    //Adjusts the ui for discard pile and becomes a recycle ui pass true for recycle then false for turning it back to regular 
    public void SetupRecycleUI(bool on)
    {
        if (on)
        {
            //turn off exit exit attach recycle button and change title
            titleText.text = "Recycle";
            GameManager.Instance.exitDiscardsButton.gameObject.SetActive(false);
            GameManager.Instance.recycleButton.gameObject.SetActive(true);
        }
        else
        {
            //turn on exit button 
            titleText.text = "Discard";
            GameManager.Instance.exitDiscardsButton.gameObject.SetActive(true);
            GameManager.Instance.recycleButton.gameObject.SetActive(false);
        }
    }

    public void RecycleConfirmButton()
    {
        if (UIManager.Instance.LastSelectedCard)
        {
            CardVisual cv = UIManager.Instance.LastSelectedCard.GetComponent<CardVisual>();
            //move the selected card to top of the deck. 
            deckList = UIManager.Instance.GetActiveDeckList(true);
            deckList.Insert(0, cv.GetCardData());

            TMP_Text deckCounter = GameManager.Instance.GetActiveDeck(true).transform.Find("DeckCounter").GetComponent<TMP_Text>();
            deckCounter.text = deckList.Count.ToString();

            Destroy(GameManager.Instance.GetActiveDiscardPile(true).GetChild(GameManager.Instance.RemoveCardAtIndex).gameObject);
            UIManager.Instance.GetActiveDiscardList(true).RemoveAt(GameManager.Instance.RemoveCardAtIndex);
            GameManager.Instance.RemoveCardAtIndex = -1;

            //Add the power to the Queue
            EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_RECYCLE);
        }

        GameManager.Instance.GetActiveDiscardUI(true).gameObject.SetActive(false);
        SetupRecycleUI(false);
        GameManager.Instance.allyDiscardPileButton.interactable = true;
        GameManager.Instance.enemyDiscardPileButton.interactable = true;

        //Call the Next Effect in the Queue
        InvokeEventCommand.InvokeNextEvent();
    }
}

