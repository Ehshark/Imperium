using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefendListener : MonoBehaviour, IListener
{
    private string damageSelected;
    public string DamageSelected { get => damageSelected; }

    public Dictionary<string, int> totalAbsorbed = new Dictionary<string, int>
    {
        { "stealth", 0 },
        { "lifesteal", 0 },
        { "poisonTouch", 0 },
        { "damage", 0 }
    };

    private void Start()
    {
        damageSelected = "";
        EventManager.Instance.AddListener(EVENT_TYPE.DEFEND_AGAINST, this);
        foreach (Transform t in GameManager.Instance.StartCombatDamageUI)
        {
            if (t.name.Equals("poisonTouch"))
            {
                t.gameObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectDamageType("poisonTouch"); });
            }
            else if (t.name.Equals("stealth"))
            {
                t.gameObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectDamageType("stealth"); });
            }
            else if (t.name.Equals("damage"))
            {
                t.gameObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectDamageType("damage"); });
            }
            else if (t.name.Equals("lifesteal"))
            {
                t.gameObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { SelectDamageType("lifesteal"); });
            }
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        if (!GameManager.Instance.IsDefending)
        {
            GameManager.Instance.ActiveHero(false).DefendButton.GetComponentInChildren<Button>().onClick.AddListener(SubmitDefenseButtonFunc);
            GameManager.Instance.IsDefending = true;
            ChangeUiForDefense(GameManager.Instance.IsDefending);
        }
    }

    private void ChangeUiForDefense(bool isDefending)
    {
        GameManager.Instance.ActiveHero(true).AttackButton.parent.gameObject.SetActive(!isDefending);
        GameManager.Instance.ActiveHero(false).AttackButton.gameObject.SetActive(!isDefending);
        GameManager.Instance.ActiveHero(false).CancelButton.gameObject.SetActive(!isDefending);
        GameManager.Instance.ActiveHero(false).SubmitButton.gameObject.SetActive(!isDefending);
        GameManager.Instance.ActiveHero(false).AttackButton.parent.gameObject.SetActive(isDefending);
        GameManager.Instance.ActiveHero(false).DefendButton.gameObject.SetActive(isDefending);

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            t.GetComponent<CardVisual>().damageObjects.SetActive(isDefending);
        }
        GameManager.Instance.ActiveHero(false).DamageObjects.gameObject.SetActive(isDefending);
        GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(isDefending);
    }

    public void SelectDamageType(string damageType)
    {
        damageSelected = damageType;
        foreach (Transform t in GameManager.Instance.StartCombatDamageUI)
        {
            if (t.name.Equals(damageType))
            {
                t.gameObject.GetComponentInChildren<Button>().interactable = false;
            }
            else
            {
                t.gameObject.GetComponentInChildren<Button>().interactable = true;
            }
        }
    }

    private bool CompareDamage()
    {
        totalAbsorbed = new Dictionary<string, int>
        {
            { "stealth", 0 },
            { "lifesteal", 0 },
            { "poisonTouch", 0 },
            { "damage", 0 }
        };
        StartCombat sc = GameManager.Instance.ActiveHero(true).AttackButton.parent.GetComponent<StartCombat>();
        CalculateDamageAssigned();
        foreach (KeyValuePair<string, int> entry in totalAbsorbed)
        {
            if (totalAbsorbed[entry.Key] == sc.totalDamage[entry.Key])
            {
                continue;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private void CalculateDamageAssigned()
    {
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            if (t.gameObject.activeSelf)
            {
                foreach (KeyValuePair<string, int> entry in t.GetComponent<CardVisual>().DmgAbsorbed.DamageAbsorbed)
                {
                    if (entry.Value != 0)
                    {
                        totalAbsorbed[entry.Key] += entry.Value;
                    }
                }
            }
        }
        foreach (KeyValuePair<string, int> entry in GameManager.Instance.ActiveHero(false).GetComponent<Hero>().DmgAbsorbed.DamageAbsorbed)
        {
            if (entry.Value != 0)
            {
                totalAbsorbed[entry.Key] += entry.Value;
            }
        }
    }

    public void SubmitDefenseButtonFunc()
    {
        bool goodSubmission = CompareDamage();
        if (goodSubmission)
        {
            StartCombat sc = GameManager.Instance.ActiveHero(true).AttackButton.parent.GetComponent<StartCombat>();
            sc.AssignDamageToAttackers();
            sc.totalDamage = new Dictionary<string, int>
            {
                { "stealth", 0 },
                { "lifesteal", 0 },
                { "poisonTouch", 0 },
                { "damage", 0 }
            };
            AssignDamageToDefenders();
            GameManager.Instance.IsDefending = false;
            ChangeUiForDefense(GameManager.Instance.IsDefending);
            GameManager.Instance.EnableOrDisablePlayerControl(true);
            StartCoroutine(GameManager.Instance.SetInstructionsText("Good Submission!"));
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Damage assignment does not match!"));
        }
    }

    private void AssignDamageToDefenders()
    {
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            if (t.gameObject.activeSelf)
            {
                CardVisual cv = t.GetComponent<CardVisual>();
                foreach (KeyValuePair<string, int> entry in cv.DmgAbsorbed.DamageAbsorbed)
                {
                    if (entry.Value != 0)
                    {
                        if (entry.Key.Equals("poisonTouch") && cv.Md.EffectId1 != 9)
                        {
                            cv.CurrentHealth = 0;
                        }
                        else
                        {
                            cv.AdjustHealth(entry.Value, false);
                        }
                    }
                }

                if (t.GetComponent<CardVisual>().CurrentHealth == 0)
                {
                    GameManager.Instance.MoveCard(t.gameObject, GameManager.Instance.GetActiveDiscardPile(false), GameManager.Instance.enemyDiscardPileList);
                }
                else
                {
                    t.GetComponent<CardVisual>().DmgAbsorbed.ResetDamageAbsorbed();
                }
            }
        }

        foreach (KeyValuePair<string, int> entry in GameManager.Instance.ActiveHero(false).GetComponent<Hero>().DmgAbsorbed.DamageAbsorbed)
        {
            if (entry.Value != 0)
            {
                GameManager.Instance.ActiveHero(false).AdjustHealth(entry.Value, false);
                if (entry.Key.Equals("lifesteal"))
                {
                    GameManager.Instance.ActiveHero(true).AdjustHealth(entry.Value, true);
                }
            }
        }

        GameManager.Instance.ActiveHero(false).DmgAbsorbed.ResetDamageAbsorbed();
        // TODO: End the game if the hero's health is 0.
    }
}
