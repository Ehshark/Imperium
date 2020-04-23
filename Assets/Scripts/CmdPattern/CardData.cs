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
    private string goldAndManaCost;
    [SerializeField]
    private string conditionText;
    [SerializeField]
    private object condition;
    [SerializeField]
    private string effectText1;
    [SerializeField]
    private object effect1;
    [SerializeField]
    private string effectText2;
    [SerializeField]
    private object effect2;
    [SerializeField]
    private Image image;
    [SerializeField]
    private int attackDamage;
    [SerializeField]
    private string health;
    [SerializeField]
    private string cardClass;
    [SerializeField]
    private bool isPromoted;
    [SerializeField]
    private bool isTapped;
    [SerializeField]
    private bool isSilenced;
    [SerializeField]
    private object enemyDamageDealt;
    [SerializeField]
    private string allyClass;
}
