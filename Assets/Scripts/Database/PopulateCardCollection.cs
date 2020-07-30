using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateCardCollection : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject content;
    public int NumberToCreate = 15;
    private List<Card> deck = new List<Card>();


    // Start is called before the first frame update
    void Start()
    {
        LoadWarriorCards();
       // FillCards();
        // PopulateCards();
    }

    public void FillCards()
    {
        GameObject newObj;
           
        //tester function to fill 
        for (int i = 0; i < 15; i++)
        {
            newObj = Instantiate(Prefab, transform);
            newObj.GetComponent<Image>().color = Random.ColorHSV();
            
        }
        
    }


    public void LoadCards()
    {
        Debug.Log("yo");
        UIManager.Instance.LoadScriptableObjectsToLists();
        UIManager.Instance.LoadSprites();
        UIManager.Instance.SortPiles();

        UIManager.Instance.SetWarriorMinion();
        UIManager.Instance.SetMageMinion();
        UIManager.Instance.SetEssentials();
        UIManager.Instance.SetStarterDeck();
        LoadWarriorCards();

        //UIManager.Instance.SortPiles();

    }
    public void LoadWarriorCards()
    {
        GameObject g = Instantiate(UIManager.Instance.minionPrefab);
        // if(filteredCard.GetComponent<CardVisual>().Md.CardClass == "Warrior"){
        foreach (MinionData w in UIManager.Instance.minions)
        {

            GameManager.Instance.SpawnCard(content.transform, w);

        }
    }

    public void LoadRogueCards()
    {

    }


    public void LoadMageCards()
    {

    }

    public void LoadStarterCards()
    {

    }

    public void ClearCards()
    {

    }
}
