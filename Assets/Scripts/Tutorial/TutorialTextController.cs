using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.SceneManagement;

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

    public bool disablePlay;
    public bool disablePromote;
    public bool disableCondition;
    public bool secondCondition;

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
            { new KeyValuePair<string, Action>("Now that we have some gold, let's buy a Card from the shop.", delegate { EnableShop(); }) },
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
            { new KeyValuePair<string, Action>("If a player uses their hero to attack, the hero must pay mana equal to their hero's current damage.", delegate { ButtonDelay(); }) },
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
            { new KeyValuePair<string, Action>("There we go! The opponent's hero took one damage and the control was placed back to you.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Oh, what happened to our Minion? After the combat phase, each minion that was selected to attack will be tapped until your next turn.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Any minions that are tapped cannot delcare an attack or defend against an attack.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Alright, now that our turn is over, let's give our opponent control.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Press the End Turn Button to end our turn.", delegate { ActivateEndTurn(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("", delegate { ActivateEnemyAI(); }) },
            { new KeyValuePair<string, Action>("Cool, our opponent ended his turn and gave control over to us. Now, let's look at our hand.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("We drew two health potions and one starter minion. Health potions are new.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("While using a health potion, our hero will be able to regenerate one health. Since he's at full health now, we don't need to use this.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Since we drew another minion, let's Promote the minion we currently have on our field.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Click on the minion thats glowing and select the Promote button.", delegate { SetupPromote(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("When you promote a minion, the minion you selected will be sent to the discard pile. The minion you chose to promote will gain two extra health.", delegate { ButtonDelay(); DestroyPlayCard(); }) },
            { new KeyValuePair<string, Action>("Since our minion was at one health, it made sense to sacrifice him.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Let's talk about conditions and effects. Each minion you buy from the shop will have certain conditions and effects.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("If the condition was achieved, the effect or effects of that minion will activate.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("To show you, I will add one minion to your hand and increase some mana for you. Play the glowing minion.", delegate { SetupMinion(4); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("The minion summoned has the condition called tap. If you tap or select this minion, the minion's effect will activate.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("If the player decides to use this effect, the minion who's tapped will be dealt one damage.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("In this case, if the player taps this minion, the player will be able to draw one card from their deck. Let's try that.", delegate { SetupCondition(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("To sum up of what just happened, the Tap Minion Condition activated allowing the player to draw one card from their deck.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("There are over 100 cards to buy from the shop, each minion having a different condition and different effect.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Buy the right cards from the shop to activate a chain of effects.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Alright there is one more thing I have to show you, powers. When a hero levels up, based on their level, we can equip the hero with one power.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Each power will have the same effect but triggering the power will be different everytime.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("To demonstrate powers, I will give your hero 4 Exp. That should be good to level up your hero.", delegate { LevelUpHero(); }) },
            { new KeyValuePair<string, Action>("This here is the skill tree. In the skill tree, you can select one power to give to your Hero. Each power will trigger differently based on the condition.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Every Condition in the skill tree is an effect. Meaning, if you trigger an effect, the power with that condition will trigger as well.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("You see the effect that just flashed? That effect is called Buff Minion; it allows you to increase the damage of one minion for that turn.", delegate { DelayOnHero("SkillEffect", 2f); }) },
            { new KeyValuePair<string, Action>("Click on that effect and then select unlock. I'll explain more after.", delegate { EnablePower(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("Awesome! So now our hero has a power, when the Buff Minion Effect is triggered the power will trigger as well.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Alright, so let's demonstrate this. I will give you one minion with the Buff Minion Effect and I will refill your hero's mana.", delegate { SetupMinion(101); }) },
            { new KeyValuePair<string, Action>("When ready, play the newly added minion.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("Oh look! This minion has two effects, Buff Minion and +2 Gold. Most minion's who have a second will either be +2 Gold or +2 Exp.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Alright, so when the Tap Minion condition is activated we should see the Buff Minion effect, the +2 Gold effect and then our power.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("When ready, tap the minion to activate it's effect.", delegate { SetupCondition(); }) },
            { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
            { new KeyValuePair<string, Action>("As we experienced, his minion was chosen to take one damage. The effect of the power, shock, will either target the hero or the opponent's minions if they have any.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("And with that, I have shown you everything you need to know about Imperium. Please play through the tutorial again if you need any help.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("Click on the next button to return to the Main Menu.", delegate { ButtonDelay(); }) },
            { new KeyValuePair<string, Action>("", delegate { ExitTutorial(); }) },
        };

        //tutorialText = new List<KeyValuePair<string, Action>>
        //{
        //    { new KeyValuePair<string, Action>("In our hand currently, we have four Fetch Quest cards and one Starter Minion.", delegate { DelayOnHero("Hand", 2f); }) },
        //    { new KeyValuePair<string, Action>("Let's start out by playing two Fetch Quest Cards and our Starting Minion",  delegate { AttachForFetchAndMinion(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("Excellent! In every turn, we want to get rid of many cards possible.", delegate { DestroyPlayCard(); }) },
        //    { new KeyValuePair<string, Action>("Since our minion was at one health, it made sense to sacrifice him.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Let's talk about conditions and effects. Each minion you buy from the shop will have certain conditions and effects.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("If the condition was achieved, the effect or effects of that minion will activate.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("To show you, I will add one minion to your hand and increase some mana for you. Play the glowing minion.", delegate { SetupMinion(4); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("The minion summoned has the condition called tap. If you tap or select this minion, the minion's effect will activate.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("If the player decides to use this effect, the minion who's tapped will be dealt one damage.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("In this case, if the player taps this minion, the player will be able to draw one card from their deck. Let's try that.", delegate { SetupCondition(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("To sum up of what just happened, the Tap Minion Condition activated allowing the player to draw one card from their deck.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("There are over 100 cards to buy from the shop, each minion having a different condition and different effect.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Buy the right cards from the shop to activate a chain of effects.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Alright there is one more thing I have to show you, powers. When a hero levels up, based on their level, we can equip the hero with one power.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Each power will have the same effect but triggering the power will be different everytime.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("To demonstrate powers, I will give your hero 4 Exp. That should be good to level up your hero.", delegate { LevelUpHero(); }) },
        //    { new KeyValuePair<string, Action>("This here is the skill tree. In the skill tree, you can select one power to give to your Hero. Each power will trigger differently based on the condition.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Every Condition in the skill tree is an effect. Meaning, if you trigger an effect, the power with that condition will trigger as well.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("You see the effect that just flashed? That effect is called Buff Minion; it allows you to increase the damage of one minion for that turn.", delegate { DelayOnHero("SkillEffect", 2f); }) },
        //    { new KeyValuePair<string, Action>("Click on that effect and then select unlock. I'll explain more after.", delegate { EnablePower(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("Awesome! So now our hero has a power, when the Buff Minion Effect is triggered the power will trigger as well.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Alright, so let's demonstrate this. I will give you one minion with the Buff Minion Effect and I will refill your hero's mana.", delegate { SetupMinion(101); }) },
        //    { new KeyValuePair<string, Action>("When ready, play the newly added minion.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("Oh look! This minion has two effects, Buff Minion and +2 Gold. Most minion's who have a second will either be +2 Gold or +2 Exp.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Alright, so when the Tap Minion condition is activated we should see the Buff Minion effect, the +2 Gold effect and then our power.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("When ready, tap the minion to activate it's effect.", delegate { SetupCondition(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { CloseUI(); }) },
        //    { new KeyValuePair<string, Action>("As we experienced, his minion was chosen to take one damage. The effect of the power, shock, will either target the hero or the opponent's minions if they have any.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("And with that, I have shown you everything you need to know about Imperium. Please play through the tutorial again if you need any help.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("Click on the next button to return to the Main Menu.", delegate { ButtonDelay(); }) },
        //    { new KeyValuePair<string, Action>("", delegate { ExitTutorial(); }) },
        //};

        enemyAI = new List<Action>
        {
            { new Action(delegate { Defend(); }) },
            { new Action(delegate { EnemyTurn(); }) },
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
            Transform topObject = hero.Find("TopUiElements");
            DelayCommand dc = new DelayCommand(topObject.Find("HeroDamage"), time);
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
            Transform topObject = hero.Find("TopUiElements");
            DelayCommand dc = new DelayCommand(topObject.Find("HeroDamage"), time);
            dc.AddToQueue();
        }
        else if (type.Equals("Gold"))
        {
            Transform topObject = hero.Find("TopUiElements");
            DelayCommand dc = new DelayCommand(topObject.Find("GoldOwned"), time);
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
        else if (type.Equals("EndTurn"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.endButton.transform, time);
            dc.AddToQueue();
        }
        else if (type.Equals("SkillEffect"))
        {
            DelayCommand dc = new DelayCommand(GameManager.Instance.skillTree.GetComponent<SkillTreeController>().buffIcon.transform, time);
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

        if (disablePlay)
        {
            disablePlay = false;
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

        foreach (Transform t in GameManager.Instance.essentialsPile)
        {
            ShowShopCard ssc = t.GetComponent<ShowShopCard>();
            if (ssc)
            {
                Destroy(ssc);
            }
        }

        GameManager.Instance.changeButton.interactable = false;        

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
        if (enemyAI.ElementAtOrDefault(enemyIndex) != null)
        {
            enemyAI.ElementAtOrDefault(enemyIndex).Invoke();
        }
        enemyIndex++;
    }

    private void Defend()
    {
        StartCoroutine(DefendAI());
    }

    private IEnumerator DefendAI()
    {
        DefendListener dl = GameManager.Instance.GetComponent<DefendListener>();

        dl.SelectDamageType("damage");
        yield return new WaitForSeconds(1f);

        GameManager.Instance.ActiveHero(false).AssignDamageAbsorbed(true);
        yield return new WaitForSeconds(1f);

        dl.SubmitDefenseButtonFunc();
        yield return new WaitForSeconds(1f);

        ShowUI();
    }

    private void ActivateEndTurn()
    {
        GameManager.Instance.endButton.interactable = true;
        DelayOnHero("EndTurn", 2f);
    }

    private void EnemyTurn()
    {
        StartCoroutine(EnemyTurnAI());
    }

    private IEnumerator EnemyTurnAI()
    {
        //Disable all buttons
        GameManager.Instance.ActiveHero(true).AttackButton.Find("AttackIcon").GetComponent<Button>().interactable = false;
        GameManager.Instance.endButton.interactable = false;
        GameManager.Instance.enemyDiscardPileButton.interactable = false;
        yield return new WaitForSeconds(1f);

        //Add PlayCard to Enemy's hand
        Transform hand = GameManager.Instance.GetActiveHand(true);
        foreach (Transform t in hand)
        {
            t.gameObject.AddComponent<PlayCard>();
        }
        yield return new WaitForSeconds(2.5f);

        //Play Cards
        hand.GetChild(0).GetComponent<PlayCard>().PlayItem();
        yield return new WaitForSeconds(2f);
        hand.GetChild(0).GetComponent<PlayCard>().PlayItem();
        yield return new WaitForSeconds(2f);
        hand.GetChild(hand.childCount - 1).GetComponent<PlayCard>().PlayMinion();
        yield return new WaitForSeconds(2f);

        //Remove Card Back 
        GameManager.Instance.GetActiveMinionZone(true).GetChild(0).Find("CardBack").gameObject.SetActive(false);

        //Destroy Play Card
        foreach (Transform t in hand)
        {
            PlayCard pc = t.gameObject.GetComponent<PlayCard>();
            if (pc)
            {
                Destroy(pc);
            }
        }        

        //Open Shop
        GameManager.Instance.shop.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        ShopController shop = GameManager.Instance.shop.GetComponent<ShopController>();
        GameObject card = GameManager.Instance.warriorShopPile.GetChild(2).gameObject;
        shop.UpdateShopCard(card);
        yield return new WaitForSeconds(2f);

        StartGameController.Instance.tutorial = false;
        shop.BuyCard();
        yield return new WaitForSeconds(2f);

        card = GameManager.Instance.warriorShopPile.GetChild(2).gameObject;
        shop.UpdateShopCard(card);
        yield return new WaitForSeconds(2f);

        shop.BuyCard();
        yield return new WaitForSeconds(2f);

        StartGameController.Instance.tutorial = true;

        GameManager.Instance.shop.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);

        GameManager.Instance.EndTurn();
        yield return new WaitForSeconds(2f);

        ShowUI();

        //Disable Player's Buttons
        GameManager.Instance.ActiveHero(true).AttackButton.Find("AttackIcon").GetComponent<Button>().interactable = false;
        GameManager.Instance.endButton.interactable = false;
        GameManager.Instance.shopButton.interactable = false;
        GameManager.Instance.allyDiscardPileButton.interactable = false;
    }

    private void SetupPromote()
    {
        Transform hand = GameManager.Instance.GetActiveHand(true);
        hand.GetChild(2).gameObject.AddComponent<TutorialPlayCard>();
        hand.GetChild(2).GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);
        count = 0;
        maxCount = 0;
        disablePlay = true;

        ButtonDelay();
    }

    private void SetupMinion(int num)
    {
        Card minion = Resources.Load("Minions/" + num) as Card;
        GameObject card = GameManager.Instance.SpawnCard(GameManager.Instance.GetActiveHand(true), minion);
        card.AddComponent<TutorialPlayCard>();
        card.GetComponent<CardVisual>().particleGlow.gameObject.SetActive(true);
        disablePromote = true;
        disableCondition = true;

        GameManager.Instance.ActiveHero(true).AdjustMana(4, true);

        if (num == 101)
        {
            GameObject c = GameManager.Instance.GetActiveMinionZone(true).GetChild(1).gameObject;
            Destroy(c.GetComponent<TutorialConditionListener>());
        }

        ButtonDelay();
    }

    private void SetupCondition()
    {
        disableCondition = false;
        DestroyPlayCard();

        ButtonDelay();
    }

    private void LevelUpHero()
    {
        StartCoroutine(LevelUp());
    }

    private IEnumerator LevelUp()
    {
        Transform hero = GameManager.Instance.ActiveHero(true).gameObject.transform;
        int cnt = 4;
        
        for (int i = 0; i < cnt; i++)
        {
            DelayCommand dc = new DelayCommand(hero.Find("ExpBarBorder"), 1f);
            dc.AddToQueue();
            GameManager.Instance.ActiveHero(true).GainExp(1);

            yield return new WaitForSeconds(1f);
        }

        UpdateUI();
    }

    private void EnablePower()
    {
        GameManager.Instance.skillTree.GetComponent<SkillTreeController>().buffIcon.interactable = true;
        ButtonDelay();
    }

    private void ExitTutorial()
    {
        //SceneManager.LoadScene(0);
        LevelLoader.Instance.LoadNextScene(2);
        Music.Instance.PlayTitleMusic();
    }
}
