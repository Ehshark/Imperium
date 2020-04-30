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
    public List<MinionData> cards;
    private MinionData tempGo;
    public GameObject currentCard;

    Dictionary<int, string> minionConditions;
    Dictionary<int, string> minionEffects;
    Dictionary<int, string> minionClasses;

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
        minionConditions = new Dictionary<int, string>
        {
            {1,"bleed"},
            {2,"buy-first-card"},
            {3,"minion-defeated"},
            {4,"tap"},
            {5,"change-shop"},
            {6,"action-draw"},
            {7,"passive"}
        };

        minionEffects = new Dictionary<int, string>
        {
            {1,"draw-card"},
            {2,"peek-shop"},
            {3,"change-shop"},
            {4,"express-buy"},
            {5,"recycle"},
            {6,"heal-allied-minion"},
            {7,"poison-touch"},
            {8,"stealth"},
            {9,"vigilance"},
            {10,"lifesteal"},
            {11,"untap"},
            {12,"silence"},
            {13,"shock"},
            {14,"buff-allied-minion"},
            {15,"card-discard"},
            {16,"loot"},
            {17,"trash"},
            {18,"coins"},
            {19,"experience"}
        };

        minionClasses = new Dictionary<int, string>
        {
            {1,"rogue"},
            {2,"warrior"},
            {3,"mage"}
        };

        LoadSprites();

        cards = new List<MinionData>();
        for (int i = 0; i < 104; i++)
        {
            tempGo = Resources.Load("Minions/" + (i + 1)) as MinionData;
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
        foreach (KeyValuePair<int, string> entry in minionConditions)
            if (cards[index].ConditionID == entry.Key)
                condition.sprite = allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

        //set the effect1 icons
        foreach (KeyValuePair<int, string> entry in minionEffects)
            if (cards[index].EffectId1 == entry.Key)
                effect1.sprite = allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

        //set the effect2 icons
        foreach (KeyValuePair<int, string> entry in minionEffects)
            if (cards[index].EffectId2 == entry.Key)
                effect2.sprite = allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

        //set the allied class icon
        foreach (KeyValuePair<int, string> entry in minionClasses)
            if (cards[index].AllyClassID == entry.Key)
                allyClass.sprite = allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
    }

    public void NextMinionButton()
    {
        cardIndex++;
        PopulateCard(cardIndex);
        mb.UpdateCardDescriptions();
    }
}
