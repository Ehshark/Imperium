using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;

public class TutorialTextController : MonoBehaviour
{
    [SerializeField]
    private Text tutorialUIText;
    [SerializeField]
    private Button nextButton;

    private List<KeyValuePair<string, Action>> tutorialText;
    private int indexUI;

    public int count;
    public int maxCount;

    public void Awake()
    {
        tutorialText = new List<KeyValuePair<string, Action>>
        {
            { new KeyValuePair<string, Action>("Welcome to Imperium! In this tutorial, you will learn the gameplay, features, and general mechanics of this card game. ", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Each Player will be given three Hero's to select from; heros being, Mage, Warrior, and Rogue. For this tutorial our Hero will be a Rogue.", delegate { DelayOnHero("Hero", 2f); }) },
            { new KeyValuePair<string, Action>("Each Hero is supplied with a specific set of resources which is different for each Hero.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("The Rogue, will start with 2 Damage, 4 Health, and 4 Mana. These resources will change depending on the Hero's level.", delegate { DelayOnHero("ResourcesAndLevel", 1f); }) },
            { new KeyValuePair<string, Action>("When certain cards or effects are played, the Hero's Gold counter will increase by either 1 or 2 gold.", delegate { DelayOnHero("Gold", 2f); }) },
            { new KeyValuePair<string, Action>("Gold in this game is used to buy Cards from the shop. We'll come back to this later.", delegate { DelayOnHero("Shop", 2f); }) },
            { new KeyValuePair<string, Action>("Now, let's take a look at our hand. Currently, we have 5 cards in our hand but our opponent has 6.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Each hero has a specific ability that is assigned to them at the beginning of the game.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("The opponents Hero, the Mage, is allowed to hold up to six cards every turn.", delegate { DelayOnHero("Opponent", 2f);  }) },
            { new KeyValuePair<string, Action>("Looking back at our Hero. What special ability has been added to our hero?", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("For the Rogue, our damage counter has been increased by 1. Meaning, the Rogue can attack with 2 damage.", delegate { DelayOnHero("Damage", 2f); }) },
            { new KeyValuePair<string, Action>("For the Warrior, the Warrior is allowed to select any power at the start of the game. We'll come back to powers later.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("In our hand currently, we have four Fetch Quest cards and one Starter Minion.", delegate { DelayOnHero("Hand", 2f); }) },
            { new KeyValuePair<string, Action>("Let's start out by playing two Fetch Quest Cards and our Starting Minion",  delegate { AttachForFetchAndMinion(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("Excellent! In every turn, we want to get rid of many cards possible.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Because our mana counter is at zero, we cannot play anymore cards this turn.", delegate { DelayOnHero("Mana", 2f); }) },
            { new KeyValuePair<string, Action>("Wait, something is different. Both are Gold counter and our Exp counter got increased.", delegate { DelayOnHero("Gold", 1f); DelayOnHero("Exp", 1f); }) },
            { new KeyValuePair<string, Action>("When Fetch Quest is played, our Hero will gain +1 Experience and +1 Gold.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Now that we have some gold, let's buy a Card from the shop.", delegate { GameManager.Instance.shopButton.interactable = true; ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) }
        };

        //tutorialText = new List<KeyValuePair<string, Action>>
        //{
        //    { new KeyValuePair<string, Action>("When Fetch Quest is played, our Hero will gain +1 Experience and +1 Gold.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Now that we have some gold, let's buy a Card from the shop.", delegate { EnableShop(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) }
        //};
    }

    public void Start()
    {
        indexUI = 1;
        count = 0;
        maxCount = 0;
    }

    public void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (indexUI < tutorialText.Count)
        {
            if (tutorialText.ElementAtOrDefault(indexUI).Value != null)
            {
                nextButton.interactable = false;
                tutorialText.ElementAtOrDefault(indexUI).Value.Invoke();
            }

            tutorialUIText.text = tutorialText.ElementAtOrDefault(indexUI).Key;
            indexUI++;
        }
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    private void DelayOnHero(string type, float time)
    {
        Hero heroObject = GameManager.Instance.ActiveHero(true);
        Transform hero = GameManager.Instance.ActiveHero(true).gameObject.transform;

        if (type.Equals("Hero"))
        {
            DelayCommand dc = new DelayCommand(heroObject.HeroImage.transform, time);
            dc.AddToQueue();
        }
        else if (type.Equals("ResourcesAndLevel"))
        {
            DelayCommand dc = new DelayCommand(hero.Find("HeroDamage"), time);
            dc.AddToQueue();
            dc = new DelayCommand(hero.Find("HealthBarBorder"), time);
            dc.AddToQueue();
            dc = new DelayCommand(hero.Find("ManaBarBorder"), time);
            dc.AddToQueue();
            dc = new DelayCommand(hero.Find("Level"), time);
            dc.AddToQueue();
        }
        else if (type.Equals("Damage"))
        {
            DelayCommand dc = new DelayCommand(hero.Find("HeroDamage"), time);
            dc.AddToQueue();
        }
        else if (type.Equals("Gold"))
        {
            DelayCommand dc = new DelayCommand(hero.Find("GoldOwned"), time);
            dc.AddToQueue();
        }
        else if (type.Equals("Shop"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.shopButton.transform, time);
            dc.AddToQueue();
        }
        else if (type.Equals("Opponent"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.ActiveHero(false).HeroImage.transform, time);
            dc.AddToQueue();
        }
        else if (type.Equals("Hand"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true), time);
            dc.AddToQueue();
        }
        else if (type.Equals("Mana"))
        {
            DelayCommand dc = new DelayCommand(hero.Find("ManaBarBorder"), time);
            dc.AddToQueue();
        }
        else if (type.Equals("Exp"))
        {
            DelayCommand dc = new DelayCommand(hero.Find("ExpBarBorder"), time);
            dc.AddToQueue();
        }

        StartCoroutine(ButtonDelay(2f));
    }

    private void ButtonDelay()
    {
        StartCoroutine(ButtonDelay(1f));
    }

    private IEnumerator ButtonDelay(float time)
    {
        yield return new WaitForSeconds(time);

        nextButton.interactable = true;
    }

    private void AttachForFetchAndMinion()
    {
        Transform hand = GameManager.Instance.GetActiveHand(true);

        hand.GetChild(0).gameObject.AddComponent<TutorialPlayCard>();
        hand.GetChild(1).gameObject.AddComponent<TutorialPlayCard>();
        hand.GetChild(4).gameObject.AddComponent<TutorialPlayCard>();

        count = 0;
        maxCount = 2;

        StartCoroutine(ButtonDelay(1f));
    }

    private void EnableShop()
    {
        GameManager.Instance.shopButton.interactable = true;
        ButtonDelay();
    }
}
