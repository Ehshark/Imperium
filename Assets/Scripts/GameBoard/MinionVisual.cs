using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class MinionVisual : MonoBehaviour, IPointerClickHandler
{
    private MinionData md;
    public MinionData Md { get => md; set => md = value; }

    bool isEnlarged = false;
    public List<Transform> descriptions;

    public TMP_Text cost;
    public TMP_Text health;
    public TMP_Text damage;

    public Image cardBackground;
    public Image condition;
    public Image allyClass;
    public Image silenceIcon;
    public Image effect1;
    public Image effect2;    private PlayCard pc;
    void OnEnable()
    {
        if (md != null)
        {
            PopulateCard();
            UpdateCardDescriptions();
        }
    }

    public void UpdateCardDescriptions()
    {
        TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
        text.text = "This minion's gold and mana cost is " + md.GoldAndManaCost;
        text = descriptions[3].GetComponent<TMP_Text>();
        text.text = "This minion's health is " + md.Health;
        text = descriptions[5].GetComponent<TMP_Text>();
        text.text = "This minion's damage is " + md.AttackDamage;
        text = descriptions[7].GetComponent<TMP_Text>();
        text.text = "When this minion attacks, if you control a " + md.AllyClass + " minion, this minion's damage increases by 1";
        text = descriptions[9].GetComponent<TMP_Text>();
        text.text = md.ConditionText;
        text = descriptions[11].GetComponent<TMP_Text>();
        text.text = md.EffectText1;
        text = descriptions[13].GetComponent<TMP_Text>();
        text.text = md.EffectText2;
    }

    void PopulateCard()
    {
        //set the cost
        cost.text = Md.GoldAndManaCost.ToString();
        //set the health
        health.text = Md.Health.ToString();
        //set the damage
        damage.text = Md.AttackDamage.ToString();
        //set the card's color
        cardBackground.color = Md.Color;
        //set the condition icon
        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionConditions)
            if (Md.ConditionID == entry.Key)
                condition.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        //set the effect1 icons
        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
            if (Md.EffectId1 == entry.Key)
                effect1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        //set the effect2 icons
        if (Md.EffectText2.Equals(""))
            effect2.gameObject.SetActive(false);
        else
        {
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                if (Md.EffectId2 == entry.Key)
                    effect2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        }
        //set the allied class icon
        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionClasses)
            if (Md.AllyClassID == entry.Key)
                allyClass.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
    }

    void CollapseMinionCard()
    {
        transform.localScale = new Vector3(1, 1, 1);
        isEnlarged = false;
        foreach (Transform t in descriptions)
            t.gameObject.SetActive(false);
    }

    void EnlargeMinionCard()
    {
        transform.localScale = new Vector3(4, 4, 4);
        isEnlarged = true;
        TMP_Text text = descriptions[13].GetComponent<TMP_Text>();
        if (!text.text.Equals(""))
            foreach (Transform t in descriptions)
                t.gameObject.SetActive(true);
        else
            for (int i = 0; i < descriptions.Count - 2; i++)
                descriptions[i].gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!isEnlarged)
                EnlargeMinionCard();
            else
                CollapseMinionCard();
        }

        else if (eventData.button == PointerEventData.InputButton.Left && 
            GameManager.Instance.ActiveHero().CanPlayCards &&
            (transform.parent.name.Equals("Hand") || transform.parent.name.Equals("EnemyHand")))
        {
            pc = gameObject.GetComponent<PlayCard>();
            if (pc)
                pc.ShowSummonPanel();

            UIManager.Instance.LastSelectedCard = gameObject;
        }
    }

    //TODO: OnHover Function to highlight the card
}
