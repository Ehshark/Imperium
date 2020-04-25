using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New CardData", menuName = "Card Data", order = 51)]
public class CardData : ScriptableObject
{
    [SerializeField]
    private Color color;
    [SerializeField]
    private int minionID;
    [SerializeField]
    private int goldAndManaCost;
    [SerializeField]
    private Image goldOrManaIcon;
    [SerializeField]
    private string conditionText;
    [SerializeField]
    private object condition;
    [SerializeField]
    private Image conditionIcon;
    [SerializeField]
    private string effectText1;
    [SerializeField]
    private object effect1;
    [SerializeField]
    private string effectText2;
    [SerializeField]
    private object effect2;
    [SerializeField]
    private Image cardArt;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private int health;
    [SerializeField]
    private Image healthIcon;
    [SerializeField]
    private string cardClass;
    [SerializeField]
    private Image classIcon;
    [SerializeField]
    private bool isPromoted;
    [SerializeField]
    private Image promotedIcon;
    [SerializeField]
    private bool isTapped;
    [SerializeField]
    private Image tappedIcon;
    [SerializeField]
    private bool isSilenced;
    [SerializeField]
    private Image silencedIcon;
    [SerializeField]
    private object enemyDamageDealt;
    [SerializeField]
    private string allyClass;
    [SerializeField]
    private Image allyClassIcon;

    public Color Color { get => color; set => color = value; }
    public int GoldAndManaCost { get => goldAndManaCost; set => goldAndManaCost = value; }
    public int MinionID { get => minionID; set => minionID = value; }
    public string ConditionText { get => conditionText; set => conditionText = value; }
    public object Condition { get => condition; set => condition = value; }
    public string EffectText1 { get => effectText1; set => effectText1 = value; }
    public object Effect1 { get => effect1; set => effect1 = value; }
    public string EffectText2 { get => effectText2; set => effectText2 = value; }
    public object Effect2 { get => effect2; set => effect2 = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int Health { get => health; set => health = value; }
    public string CardClass { get => cardClass; set => cardClass = value; }
    public bool IsPromoted { get => isPromoted; set => isPromoted = value; }
    public bool IsTapped { get => isTapped; set => isTapped = value; }
    public bool IsSilenced { get => isSilenced; set => isSilenced = value; }
    public object EnemyDamageDealt { get => enemyDamageDealt; set => enemyDamageDealt = value; }
    public string AllyClass { get => allyClass; set => allyClass = value; }
    public Image GoldOrManaIcon { get => goldOrManaIcon; set => goldOrManaIcon = value; }
    public Image ConditionIcon { get => conditionIcon; set => conditionIcon = value; }
    public Image CardArt { get => cardArt; set => cardArt = value; }
    public Image HealthIcon { get => healthIcon; set => healthIcon = value; }
    public Image ClassIcon { get => classIcon; set => classIcon = value; }
    public Image PromotedIcon { get => promotedIcon; set => promotedIcon = value; }
    public Image TappedIcon { get => tappedIcon; set => tappedIcon = value; }
    public Image SilencedIcon { get => silencedIcon; set => silencedIcon = value; }
    public Image AllyClassIcon { get => allyClassIcon; set => allyClassIcon = value; }
}
