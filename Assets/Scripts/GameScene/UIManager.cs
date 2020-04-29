//This script uses a magic number - the number of ScriptableObjects inside Assets/Resources/Cards

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
//using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public List<Sprite> allSprites;
    public List<CardData> cards;
    private CardData tempGo;
    public GameObject currentCard;

    public int cardIndex = 0;
    public TMP_Text cost;
    public TMP_Text health;
    public TMP_Text damage;

    public Image cardBackground;
    public Image condition;
    public Image allyClass;
    public Image silenceIcon;
    public Image effect1;
    public Image effect2;

    public static UIManager Instance { get; private set; } = null;

    MinionBehaviour mb;

    //public Text yourName;
    //public Text opponentName;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        mb = currentCard.GetComponent<MinionBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadSprites();

        cards = new List<CardData>();
        for (int i = 0; i < 104; i++)
        {
            tempGo = Resources.Load("Cards/" + (i + 1)) as CardData;
            cards.Add(tempGo);
        }

        Shuffle();

        PopulateCard(cardIndex);

        mb.UpdateCardDescriptions();

        //if (PhotonNetwork.IsMasterClient) {
        //    yourName.text = "Host: " + GameManager.UserName;
        //    opponentName.text = "Client: " + PhotonNetwork.PlayerListOthers[0].NickName;
        //}

        //else {
        //    yourName.text = "Client: " + GameManager.UserName;
        //    opponentName.text = "Host: " + PhotonNetwork.PlayerListOthers[0].NickName;
        //}
    }

    private void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int rnd = Random.Range(0, cards.Count);
            tempGo = cards[rnd];
            cards[rnd] = cards[i];
            cards[i] = tempGo;
        }
    }

    void LoadSprites()
    {
        object[] loadedIcons = Resources.LoadAll("VisualAssets/game-icons-net/CardAndHeroAttributes", typeof(Sprite));
        allSprites = new List<Sprite>();
        for (int i = 0; i < loadedIcons.Length; i++)
            allSprites.Add((Sprite)loadedIcons[i]);

        loadedIcons = Resources.LoadAll("VisualAssets/game-icons-net/HeroAbilityConditions", typeof(Sprite));
        for (int i = 0; i < loadedIcons.Length; i++)
            allSprites.Add((Sprite)loadedIcons[i]);

        loadedIcons = Resources.LoadAll("VisualAssets/game-icons-net/MinionConditions", typeof(Sprite));
        for (int i = 0; i < loadedIcons.Length; i++)
            allSprites.Add((Sprite)loadedIcons[i]);

        loadedIcons = Resources.LoadAll("VisualAssets/game-icons-net/Game-UI", typeof(Sprite));
        for (int i = 0; i < loadedIcons.Length; i++)
            allSprites.Add((Sprite)loadedIcons[i]);
    }

    void PopulateCard(int index)
    {
        //set the cost
        cost.text = cards[index].GoldAndManaCost.ToString();

        //set the health
        health.text = cards[index].Health.ToString();

        //set the damage
        damage.text = cards[index].AttackDamage.ToString();

        //set the card's color
        cardBackground.color = cards[index].Color;

        //set the condition icon
        if (cards[index].ConditionText.Equals("If the enemy hero lost health this turn"))
            condition.sprite = allSprites.Where(x => x.name == "bleed").SingleOrDefault();

        else if (cards[index].ConditionText.Equals("When you buy your first card this turn"))
            condition.sprite = allSprites.Where(x => x.name == "buy-first-card").SingleOrDefault();

        else if (cards[index].ConditionText.Equals("Whenever you defeat an enemy minion"))
            condition.sprite = allSprites.Where(x => x.name == "minion-defeated").SingleOrDefault();

        else if (cards[index].ConditionText.Equals("Tap this minion during your action phase to activate its effect"))
            condition.sprite = allSprites.Where(x => x.name == "tap").SingleOrDefault();

        else if (cards[index].ConditionText.Equals("When you first change a shop card"))
            condition.sprite = allSprites.Where(x => x.name == "change-shop").SingleOrDefault();

        else if (cards[index].ConditionText.Equals("Whenever you draw a card during your action phase"))
            condition.sprite = allSprites.Where(x => x.name == "action-draw").SingleOrDefault();

        else if (cards[index].ConditionText.Equals("Constant"))
            condition.sprite = allSprites.Where(x => x.name == "passive").SingleOrDefault();

        //set the effect1 icons
        if (cards[index].EffectText1.Equals("Draw a card"))
            effect1.sprite = allSprites.Where(x => x.name == "draw-card").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Select a shop card pile and look at the top 2 cards. Send either/both to the bottom or keep on top."))
            effect1.sprite = allSprites.Where(x => x.name == "peek-shop").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Select a card in the shop. That card goes to the bottom of the respective shop pile and is replaced by a card from the top."))
            effect1.sprite = allSprites.Where(x => x.name == "change-shop").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("The next card you buy this turn goes straight to your hand"))
            effect1.sprite = allSprites.Where(x => x.name == "express-buy").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Select a card from your discard pile. It goes on top of your deck."))
            effect1.sprite = allSprites.Where(x => x.name == "recycle").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Increase target allied minion's health by 1"))
            effect1.sprite = allSprites.Where(x => x.name == "heal-allied-minion").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("This minion has poison touch.") ||
            cards[cardIndex].EffectText1.Equals("This minion gains poison touch until the end of your turn."))
            effect1.sprite = allSprites.Where(x => x.name == "poison-touch").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("This minion has stealth.") ||
            cards[cardIndex].EffectText1.Equals("This minion gains stealth until the end of your turn."))
            effect1.sprite = allSprites.Where(x => x.name == "stealth").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("This minion has vigilance.") ||
            cards[cardIndex].EffectText1.Equals("This minion gains vigilance until the end of your turn."))
            effect1.sprite = allSprites.Where(x => x.name == "vigilance").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("This minion has lifesteal.") ||
            cards[cardIndex].EffectText1.Equals("This minion gains lifesteal until the end of your turn."))
            effect1.sprite = allSprites.Where(x => x.name == "lifesteal").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Untap target allied minion."))
            effect1.sprite = allSprites.Where(x => x.name == "untap").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Target enemy minion loses its effect(s) until the end of your opponent's next turn."))
            effect1.sprite = allSprites.Where(x => x.name == "silence").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Your opponent discards a card at the start of their next turn."))
            effect1.sprite = allSprites.Where(x => x.name == "card-discard").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Draw a card then discard a card"))
            effect1.sprite = allSprites.Where(x => x.name == "loot").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Select 1 card from your hand or discard pile. Remove that card from the game."))
            effect1.sprite = allSprites.Where(x => x.name == "trash").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Inflict 1 damage to target minion"))
            effect1.sprite = allSprites.Where(x => x.name == "shock").SingleOrDefault();

        else if (cards[index].EffectText1.Equals("Target allied minion's damage increases by 1 until the end of your turn"))
            effect1.sprite = allSprites.Where(x => x.name == "buff-allied-minion").SingleOrDefault();


        //set the effect2 icons
        if (cards[index].EffectText2.Equals("2 Gold"))
            effect2.sprite = allSprites.Where(x => x.name == "coins").SingleOrDefault();
        else if (cards[index].EffectText2.Equals("2 Exp"))
            effect2.sprite = allSprites.Where(x => x.name == "experience").SingleOrDefault();
        else
            effect2.enabled = false;

        //set the allied class icon
        if (cards[index].AllyClass.Equals("Rogue"))
            allyClass.sprite = allSprites.Where(x => x.name == "rogue").SingleOrDefault();
        else if (cards[index].AllyClass.Equals("Warrior"))
            allyClass.sprite = allSprites.Where(x => x.name == "warrior").SingleOrDefault();
        else if (cards[index].AllyClass.Equals("Mage"))
            allyClass.sprite = allSprites.Where(x => x.name == "mage").SingleOrDefault();
    }

    public void NextMinionButton()
    {
        cardIndex++;
        PopulateCard(cardIndex);
        mb.UpdateCardDescriptions();
    }
}
