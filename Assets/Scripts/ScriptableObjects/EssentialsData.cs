using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EssentialsData", menuName = "Essential Data", order = 51)]
public class EssentialsData : ScriptableObject
{
    [SerializeField]
    private Color color;
    [SerializeField]
    private int id;
    [SerializeField]
    private int manaCost;

    [SerializeField]
    private string effectText1;
    [SerializeField]
    private int effectId1;

    private object effect1;

    [SerializeField]
    private string effectText2;
    [SerializeField]
    private int effectId2;

    private object effect2;

    public Color Color { get => color; set => color = value; }
    public int Id { get => id; set => id = value; }
    public int ManaCost { get => manaCost; set => manaCost = value; }
    public string EffectText1 { get => effectText1; set => effectText1 = value; }
    public int EffectId1 { get => effectId1; set => effectId1 = value; }
    public object Effect1 { get => effect1; set => effect1 = value; }
    public string EffectText2 { get => effectText2; set => effectText2 = value; }
    public int EffectId2 { get => effectId2; set => effectId2 = value; }
    public object Effect2 { get => effect2; set => effect2 = value; }
}
