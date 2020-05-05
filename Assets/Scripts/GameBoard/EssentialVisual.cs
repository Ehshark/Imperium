using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class EssentialVisual : MonoBehaviour
{
    //bool isEnlarged = false;
    public List<Transform> descriptions;

    public TMP_Text cost;

    public Image cardBackground;
    public Image effect1;
    public Image effect2;

    void Start()
    {
        PopulateCard();
        UpdateCardDescriptions();
    }

    public void UpdateCardDescriptions()
    {
        TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
        text.text = "This card's mana cost is " + UIManager.Instance.currentStarter.ManaCost;
        text = descriptions[3].GetComponent<TMP_Text>();
        text.text = UIManager.Instance.currentStarter.EffectText1;
        text = descriptions[5].GetComponent<TMP_Text>();
        text.text = UIManager.Instance.currentStarter.EffectText2;
    }

    void PopulateCard()
    {
        //set the cost
        cost.text = UIManager.Instance.currentStarter.ManaCost.ToString();

        //set the card's color
        cardBackground.color = UIManager.Instance.currentStarter.Color;

        //set the effect1 icons
        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
            if (UIManager.Instance.currentStarter.EffectId1 == entry.Key)
                effect1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

        //set the effect2 icons
        if (!(UIManager.Instance.currentStarter.EffectText2.Equals("")))
            effect2.gameObject.SetActive(false);
        else
        {
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                if (UIManager.Instance.currentStarter.EffectId2 == entry.Key)
                    effect2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        }
    }
}
