using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StarterVisual : MonoBehaviour
{
    private StarterData sd;
    public StarterData Sd { get => sd; set => sd = value; }

    //bool isEnlarged = false;
    public List<Transform> descriptions;

    public TMP_Text cost;
    public TMP_Text health;
    public TMP_Text damage;

    public Image cardBackground;
    public Image silenceIcon;
    public Image effect1;
    public Image effect2;

    void OnEnable()
    {
        if (sd != null)
        {
            PopulateCard();
            UpdateCardDescriptions();
        }
    }

    public void UpdateCardDescriptions()
    {
        TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
        text.text = "This card's mana cost is " + sd.ManaCost;
        text = descriptions[3].GetComponent<TMP_Text>();
        text.text = "This minion's health is " + sd.Health;
        text = descriptions[5].GetComponent<TMP_Text>();
        text.text = "This minion's damage is " + sd.AttackDamage;
        text = descriptions[7].GetComponent<TMP_Text>();
        text.text = sd.EffectText1;
        text = descriptions[9].GetComponent<TMP_Text>();
        text.text = sd.EffectText2;
    }

    void PopulateCard()
    {
        //set the cost
        cost.text = sd.ManaCost.ToString();

        //set the health
        if (sd.Health != 0)
            health.text = sd.Health.ToString();
        else
            health.gameObject.SetActive(false);

        //set the damage
        if (sd.Health != 0)
            damage.text = sd.AttackDamage.ToString();
        else
            damage.gameObject.SetActive(false);

        //set the card's color
        cardBackground.color = sd.Color;

        //set the effect1 icons
        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
            if (sd.EffectId1 == entry.Key)
                effect1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

        //set the effect2 icons
        if (!(sd.EffectText2.Equals("")))
            effect2.gameObject.SetActive(false);
        else
        {
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                if (sd.EffectId2 == entry.Key)
                    effect2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        }
    }
}
