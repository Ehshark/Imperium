using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateCardCollection : MonoBehaviour
{
    public GameObject content;
    private List<Card> deck = new List<Card>();


    // Start is called before the first frame update
    void Start()
    {
        LoadEssentialCards();
       // FillCards();
        // PopulateCards();
    }

    public void LoadAllMinionCards()
    {
        ClearCards();
        GameObject g = Instantiate(UIManager.Instance.minionPrefab);
        foreach (MinionData w in UIManager.Instance.minions)
        {
           
                GameManager.Instance.SpawnCard(content.transform, w);

        }
    }


    public void LoadWarriorCards()
    {
        ClearCards();
        GameObject g = Instantiate(UIManager.Instance.minionPrefab);
        foreach (MinionData w in UIManager.Instance.minions)
        {
            if (w.CardClass == "Warrior")
            {
                GameManager.Instance.SpawnCard(content.transform, w);
            }

        }
    }

    public void LoadRogueCards()
    {
        ClearCards();
        GameObject g = Instantiate(UIManager.Instance.minionPrefab);
        foreach (MinionData w in UIManager.Instance.minions)
        {
            if (w.CardClass == "Rogue")
            {
                GameManager.Instance.SpawnCard(content.transform, w);
            }

        }
    }


    public void LoadMageCards()
    {
        ClearCards();
        GameObject g = Instantiate(UIManager.Instance.minionPrefab);
        foreach (MinionData w in UIManager.Instance.minions)
        {
            if (w.CardClass == "Mage")
            {
                GameManager.Instance.SpawnCard(content.transform, w);
            }

        }
    }

    public void LoadEssentialCards()
    {
        ClearCards();
        foreach (EssentialsData w in UIManager.Instance.essentials)
        {
                GameManager.Instance.SpawnCard(content.transform, w);
        }
    }

    public void LoadStarterCards()
    {
        ClearCards();
        foreach (StarterData w in UIManager.Instance.starters)
        {
                GameManager.Instance.SpawnCard(content.transform, w);
        }
    }

    public void ClearCards()
    {
        Debug.Log(content);
        Debug.Log(content.transform);
        
        foreach (Transform cards in content.transform)
        {
            Destroy(cards.gameObject);
        }
        //foreach (Transform w in content)
        //{
        //    Destroy(w);
        //}
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
}
