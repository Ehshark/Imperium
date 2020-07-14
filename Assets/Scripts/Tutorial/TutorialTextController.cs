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

    private Dictionary<string, Action> tutorialText;
    private List<Action> tutorialMethods;
    private int indexUI;
    private bool closeUI; 

    public void Awake()
    {
        tutorialText = new Dictionary<string, Action>
        {
            { "Welcome to Imperium! In this tutorial, you will learn the gameplay, features, and general mechanics of this card game. ", null },
            { "Each Player will be given three Hero's to select from; heros being, Mage, Warrior, and Rogue. For this tutorial our Hero will be a Rogue.", delegate { DelayOnHero("Hero"); } },
            { "Each Hero is supplied with a specific set of resources which is different for each Hero.", null },
            { "The Rogue, will start with 2 Damage, 4 Health, and 4 Mana. These resources will change depending on the Hero's level.", delegate { DelayOnHero("ResourcesAndLevel"); } },
            { "When certain cards or effects are played, the Hero's Gold counter will increase by either 1 or 2 gold.", delegate { DelayOnHero("Gold"); } },
            { "Gold in this game is used to buy Cards from the shop. We'll come back to this later.", delegate { DelayOnHero("Shop"); } },
            { "Now, let's take a look at our hand. Currently, we have 5 cards in our hand but our opponent has 6.", null},
            { "Each hero has a specific ability that is assigned to them at the beginning of the game.", null },
            { "The opponents Hero, the Mage, is allowed to hold up to six cards every turn.", delegate { DelayOnHero("Opponent");  } },
            { "Looking back at our Hero. What special ability has been added to our hero?", null },
            { "For the Rogue, our damage counter has been increased by 1. Meaning, the Rogue can attack with 2 damage.", delegate { DelayOnHero("Damage"); } },
            { "For the Warrior, the Warrior is allowed to select any power at the start of the game. We'll come back to powers later.", null },
            { "In our hand currently, we have four Fetch Quest cards and one Starter Minion.", delegate { DelayOnHero("Hand"); } },
            { "Let's start out by playing two Fetch Quest Cards and our Starting Minion", null }
        };
    }

    public void Start()
    {
        indexUI = 1;
    }

    public void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (!closeUI)
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
        else
        {
            gameObject.SetActive(false);
            closeUI = false;
        }
    }

    public void CloseUI()
    {
        closeUI = true;
    }

    private void DelayOnHero(string type)
    {
        Hero heroObject = GameManager.Instance.ActiveHero(true);
        Transform hero = GameManager.Instance.ActiveHero(true).gameObject.transform;

        if (type.Equals("Hero"))
        {
            DelayCommand dc = new DelayCommand(heroObject.HeroImage.transform, 2f);
            dc.AddToQueue();
        }
        else if (type.Equals("ResourcesAndLevel"))
        {
            DelayCommand dc = new DelayCommand(hero.Find("Level"), 1f);
            dc.AddToQueue();
            dc = new DelayCommand(hero.Find("HealthBarBorder"), 1f);
            dc.AddToQueue();
            dc = new DelayCommand(hero.Find("ManaBarBorder"), 1f);
            dc.AddToQueue();
            dc = new DelayCommand(hero.Find("HeroDamage"), 1f);
            dc.AddToQueue();
        }
        else if (type.Equals("Damage"))
        {
            DelayCommand dc = new DelayCommand(hero.Find("HeroDamage"), 2f);
            dc.AddToQueue();
        }
        else if (type.Equals("Gold"))
        {
            DelayCommand dc = new DelayCommand(hero.Find("GoldOwned"), 2f);
            dc.AddToQueue();
        }
        else if (type.Equals("Shop"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.shop, 2f);
            dc.AddToQueue();
        }
        else if (type.Equals("Opponent"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.ActiveHero(false).HeroImage.transform, 2f);
            dc.AddToQueue();
        }
        else if (type.Equals("Hand"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true), 2f);
            dc.AddToQueue();
        }

        StartCoroutine(ButtonDelay(2f));
    }

    private IEnumerator ButtonDelay(float time)
    {
        yield return new WaitForSeconds(time);

        nextButton.interactable = true;
    }
}
