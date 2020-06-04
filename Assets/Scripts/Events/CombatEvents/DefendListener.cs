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
            foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
            {
                t.GetComponent<CardVisual>().damageObjects.SetActive(true);
            }
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
            foreach (KeyValuePair<string, int> entry in t.GetComponent<CardVisual>().DmgAbsorbed.DamageAbsorbed)
            {
                if (entry.Value != 0)
                {
                    totalAbsorbed[entry.Key] += entry.Value;
                }
            }
        }
    }

    public void SubmitDefenseButtonFunc()
    {
        bool goodSubmission = CompareDamage();
        if (goodSubmission)
        {
            GameManager.Instance.IsDefending = false;
            ChangeUiForDefense(GameManager.Instance.IsDefending);
            GameManager.Instance.EnableOrDisablePlayerControl(true);
            Debug.Log("Good Submission!");
        }
        else
        {
            Debug.Log("Damage assignment does not match!");
        }
    }
}
