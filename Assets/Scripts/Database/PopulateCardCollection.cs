using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateCardCollection : MonoBehaviour
{
    public GameObject Prefab;
    public int NumberToCreate;

    private List<MinionData> AllWarriorCards;
    private List<MinionData> AllRogueCards;
    private List<MinionData> AllMageCards;

    // Start is called before the first frame update
    void Start()
    {
         LoadCards();
        // PopulateCards();
    }

    public void FillCards()
    {
        GameObject newObj;

        //fill cards up from cv
        //push to a list
        //loop num of cards then setparent to content
        //filter 
        for (int i = 0; i < NumberToCreate; i++)
        {
            newObj = (GameObject)Instantiate(Prefab, transform);
            newObj.GetComponent<Image>().color = Random.ColorHSV();
            
        }
        
    }


    public void LoadCards()
    {
        List<MinionData> mList = new List<MinionData>();
        Debug.Log("yo");
        Debug.Log(mList);
        //CardDatabaseManager.Instance.LoadScriptableObjectsToLists();
        //CardDatabaseManager.Instance.LoadSprites();


        //CardDatabaseManager.Instance.SetEssentials();




        //Load all the icon sprites to display on the cards

        //CardDatabaseManager.Instance.LoadScriptableObjectsToLists();
        //Set the starter cards for both players
        //Debug.Log(CardDatabaseManager.Instance.starters[0]  + "starters");
        //CardDatabaseManager.Instance.LoadSprites();
        //CardDatabaseManager.Instance.SetStarterDeck();

        ////Sets all 9 cards in the shop, 3 cards per pile
        //CardDatabaseManager.Instance.SetWarriorMinion();
        //CardDatabaseManager.Instance.SortPiles();
        //CardDatabaseManager.Instance.SetMageMinion();
        //CardDatabaseManager.Instance.SetEssentials();


    }
    public void SpawnWarrior()
    {
        // if(filteredCard.GetComponent<CardVisual>().Md.CardClass == "Warrior"){
               //spawncard( 

        //}
    }


    public void SpawnRogue()
    {

    }


    public void SpawnMage()
    {

    }

    public void SpawnStarterCards()
    {

    }

    public void ClearCards()
    {

    }
}
