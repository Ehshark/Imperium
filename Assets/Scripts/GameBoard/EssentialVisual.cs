using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class EssentialVisual : MonoBehaviour
{
    private EssentialsData ed;
    public EssentialsData Ed { get => ed; set => ed = value; }

    //bool isEnlarged = false;
    public List<Transform> descriptions;

    public TMP_Text cost;

    public Image cardBackground;
    public Image effect1;
    public Image effect2;

    void OnEnable()
    {
        if (Ed != null)
        {
            PopulateCard();
            UpdateCardDescriptions();
        }
    }

    public void UpdateCardDescriptions()
    {
        TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
        text.text = "This card's mana cost is " + ed.ManaCost;
        text = descriptions[3].GetComponent<TMP_Text>();
        text.text = ed.EffectText1;
        text = descriptions[5].GetComponent<TMP_Text>();
        text.text = ed.EffectText2;
    }

    void PopulateCard()
    {
        //set the cost
        cost.text = ed.ManaCost.ToString();

        //set the card's color
        cardBackground.color = ed.Color;

        //set the effect1 icons
        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
            if (ed.EffectId1 == entry.Key)
                effect1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

        //set the effect2 icons
        if (!(ed.EffectText2.Equals("")))
            effect2.gameObject.SetActive(false);
        else
        {
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                if (ed.EffectId2 == entry.Key)
                    effect2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        }
    }
}
