using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TutorialTextController : MonoBehaviour
{
    [SerializeField]
    private Text tutorialUIText;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Transform background;

    private List<KeyValuePair<string, Action>> tutorialText;
    private List<Action> enemyAI;
    private int indexUI;
    private int enemyIndex;

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
            { new KeyValuePair<string, Action>("Excellent! In every turn, we want to get rid of many cards possible.", delegate { DestroyPlayCard(); }) },
            { new KeyValuePair<string, Action>("Because our mana counter is at zero, we cannot play anymore cards this turn.", delegate { DelayOnHero("Mana", 2f); }) },
            { new KeyValuePair<string, Action>("Wait, something is different. Both are Gold counter and our Exp counter got increased.", delegate { DelayOnHero("Gold", 1f); DelayOnHero("Exp", 1f); }) },
            { new KeyValuePair<string, Action>("When Fetch Quest is played, our Hero will gain +1 Experience and +1 Gold.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Now that we have some gold, let's buy a Card from the shop.", delegate { GameManager.Instance.shopButton.interactable = true; ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("While in the shop, you can buy Minion or Essentinel Cards using the Gold your Hero obtains.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Cards that you buy in the shop are used to build your deck. Different strategies can be used from the cards you buy.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Let's try to buy a card now! See that card glowing? Use your Gold to buy it.", delegate { BuyCardTutorial(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("Excellent! When you buy a card from the shop the card will be sent to the discard pile. The shop will then be replaced with a new card.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("When you run out of cards from your deck, all cards from the discard pile will be removed and reshuffled. Making your new deck.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Other than buying cards, the shop also allows you to change a card as well.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Change Shop allows the player to change a shop card from a pile. This card will then be replaced by a new card.", delegate { DelayOnHero("ChangeButton", 2f); }) },
            { new KeyValuePair<string, Action>("To do a change shop, your gold needs to be higher than half of the selected minion's gold price.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("We have one gold left, let's try to change a card from the shop. Try it on the card that's glowing.", delegate { ChangeShopTutorial(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("Excellent! We bought a card and also did a change shop. Because our gold counter is at zero, let's exit the shop.", delegate { ExitShop(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("We learned how to play cards and we learned how to use the shop. How about we attack next.", delegate { ButtonDelay(); GameManager.Instance.shopButton.interactable = false; }) },
            { new KeyValuePair<string, Action>("Once per turn, each player is allowed to enter the combat phase and attack. Player's can use any summoned minion or their hero to attack.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("If a player uses their hero to attack, the hero must pay mana equal to half of their hero's current damage.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("If a player uses their minion(s) to attack, all attacking minions must pay one health at the end of the combat phase.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Alright let's attack! Select the Attack button to start your combat phase.", delegate { SetupAttack(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("In the combat phase, you're allowed to select both your hero and as many minions on the field to attack.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Once your selection has been made, select the Submit Button to attack your opponent.", delegate { DelayOnHero("Submit", 2f); }) },
            { new KeyValuePair<string, Action>("To cancel your combat phase, select the Cancel Button to return to your Action Phase.", delegate { DelayOnHero("Cancel", 2f); }) },
            { new KeyValuePair<string, Action>("Currently, we only have one minion summoned in the minion area. Because he has two health, he will not be destroyed after combat.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Alright let's attack; select the minion to attack with. After that, select the Submit Button.", delegate { ActivateAttack(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("Excellent! In the defending phase, the defending player has the ability to assign damage to their hero and defending minions.", delegate { DisableGlowOnMinion(); }) },
            { new KeyValuePair<string, Action>("In this case, our opponent can only assign damage to their hero. Let's take a look.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("", delegate { ActivateEnemyAI(); }) },
        };

        //tutorialText = new List<KeyValuePair<string, Action>>
        //{
        //    { new KeyValuePair<string, Action>("In our hand currently, we have four Fetch Quest cards and one Starter Minion.", delegate { DelayOnHero("Hand", 2f); }) },
        //    { new KeyValuePair<string, Action>("Let's start out by playing two Fetch Quest Cards and our Starting Minion", delegate { AttachForFetchAndMinion(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("Excellent! In every turn, we want to get rid of many cards possible.", delegate { DestroyPlayCard(); }) },
        //    { new KeyValuePair<string, Action>("Alright let's attack! Select the Attack button to start your combat phase.", delegate { SetupAttack(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("In the combat phase, you're allowed to select both your hero and as many minions on the field to attack.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Once your selection has been made, select the Submit Button to attack your opponent.", delegate { DelayOnHero("Submit", 2f); }) },
        //    { new KeyValuePair<string, Action>("To cancel your combat phase, select the Cancel Button to return to your Action Phase.", delegate { DelayOnHero("Cancel", 2f); }) },
        //    { new KeyValuePair<string, Action>("Currently, we only have one minion summoned in the minion area. Because he has two health, he will not be destroyed after combat.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Alright let's attack; select the minion to attack with. After that, select the Submit Button.", delegate { ActivateAttack(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("Excellent! In the defending phase, the defending player has the ability to assign damage to their hero and defending minions.", delegate { DisableGlowOnMinion(); }) },
        //    { new KeyValuePair<string, Action>("In this case, our opponent can only assign damage to their hero. Let's take a look.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { ActivateEnemyAI(); }) },
        //};

        enemyAI = new List<Action>
        {
            { new Action(delegate { Defend(); }) }
        };

    }

    public void Start()
    {
        indexUI = 1;
        enemyIndex = 0;
        count = 0;
        maxCount = 0;
    }

    public void OnEnable()
    {
        background.gameObject.SetActive(true);
        UpdateUI();
    }

    public void ShowUI()
    {
        background.gameObject.SetActive(true);
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
        background.gameObject.SetActive(false);
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
        else if (type.Equals("ChangeButton"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.changeButton.transform, time);
            dc.AddToQueue();
        }
        else if (type.Equals("Attack"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.ActiveHero(true).AttackButton, time);
            dc.AddToQueue();
        }
        else if (type.Equals("Submit"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.ActiveHero(true).SubmitButton, time);
            dc.AddToQueue();
        }
        else if (type.Equals("Cancel"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.ActiveHero(true).CancelButton, time);
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

        for (int i = 0; i < hand.childCount; i++)
        {
            if (i == 0 || i == 1 || i == 4)
            {
                CardVisual cv = hand.GetChild(i).GetComponent<CardVisual>();
                cv.particleGlow.gameObject.SetActive(true);
                hand.GetChild(i).gameObject.AddComponent<TutorialPlayCard>();
            }
        }

        count = 0;
        maxCount = 2;

        ButtonDelay();
    }

    private void DestroyPlayCard()
    {
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            TutorialPlayCard tpc = t.GetComponent<TutorialPlayCard>();
            if (tpc)
            {
                Destroy(tpc);
            }
        }

        ButtonDelay();
    }

    private void EnableShop()
    {
        GameManager.Instance.shopButton.interactable = true;

        foreach(Transform t in GameManager.Instance.warriorShopPile)
        {
            ShowShopCard ssc = t.GetComponent<ShowShopCard>();
            if (ssc)
            {
                Destroy(ssc);
            }
        }

        foreach (Transform t in GameManager.Instance.rogueShopPile)
        {
            ShowShopCard ssc = t.GetComponent<ShowShopCard>();
            if (ssc)
            {
                Destroy(ssc);
            }
        }

        foreach (Transform t in GameManager.Instance.mageShopPile)
        {
            ShowShopCard ssc = t.GetComponent<ShowShopCard>();
            if (ssc)
            {
                Destroy(ssc);
            }
        }

        GameManager.Instance.changeButton.interactable = false;
        GameManager.Instance.ActiveHero(true).AdjustGold(4, true);

        ButtonDelay();
    }

    private void BuyCardTutorial()
    {
        GameObject cardToBuy = GameManager.Instance.mageShopPile.GetChild(0).gameObject;
        cardToBuy.AddComponent<ShowShopCard>();
        cardToBuy.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);

        ButtonDelay();
    }

    private void ChangeShopTutorial()
    {
        GameManager.Instance.changeButton.interactable = true;
        GameManager.Instance.buyButton.interactable = false;

        GameObject cardToBuy = GameManager.Instance.rogueShopPile.GetChild(1).gameObject;
        cardToBuy.AddComponent<ShowShopCard>();
        cardToBuy.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);

        ButtonDelay();
    }

    private void ExitShop()
    {
        GameManager.Instance.changeButton.interactable = true;
        GameManager.Instance.buyButton.interactable = true;
        GameManager.Instance.exitShopButton.interactable = true;

        ButtonDelay();
    }

    private void SetupAttack()
    {
        GameManager.Instance.ActiveHero(true).AttackButton.Find("AttackIcon").GetComponent<Button>().interactable = true;
        DelayOnHero("Attack", 2f); 
    }

    private void ActivateAttack()
    {
        GameObject minion = GameManager.Instance.GetActiveMinionZone(true).GetChild(0).gameObject;
        minion.AddComponent<StartCombatListener>();
        minion.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);

        GameManager.Instance.ActiveHero(true).SubmitButton.Find("SubmitIcon").GetComponent<Button>().interactable = true;

        ButtonDelay();
    }

    private void DisableGlowOnMinion()
    {
        GameObject minion = GameManager.Instance.GetActiveMinionZone(true).GetChild(0).gameObject;
        minion.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(false);

        ButtonDelay();
    }

    public void ActivateEnemyAI()
    {
        CloseUI();

        UpdateAI();
    }

    public void UpdateAI()
    {
        enemyAI.ElementAtOrDefault(enemyIndex).Invoke();
        enemyIndex++;
    }

    private void Defend()
    {
        StartCoroutine(DefendAI());
    }

    private IEnumerator DefendAI()
    {
        GameManager.Instance.GetComponent<DefendListener>().SelectDamageType("damage");
        yield return new WaitForSeconds(1f);

        GameManager.Instance.ActiveHero(true).AssignDamageAbsorbed(true);
        yield return new WaitForSeconds(1f);
    }
}
