using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefendListener : MonoBehaviour, IListener
{
    private const byte ASSIGN_DEFENDING_DAMAGE = 18;
    private const byte END_GAME_DEFENDER = 41;

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

        if (!StartGameController.Instance.tutorial)
        {
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
    }

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        if (!GameManager.Instance.IsDefending)
        {
            GameManager.Instance.ActiveHero(false).DefendButton.GetComponentInChildren<Button>().onClick.AddListener(SubmitDefenseButtonFunc);
            GameManager.Instance.IsDefending = true;
            ChangeUiForDefense(GameManager.Instance.IsDefending);
            
            if (StartGameController.Instance.tutorial)
            {
                StartGameController.Instance.TutorialObject.GetComponent<TutorialTextController>().ShowUI();
            }
        }
    }

    public void ChangeUiForDefense(bool isDefending)
    {
        //GameManager.Instance.ActiveHero(true).AttackButton.parent.gameObject.SetActive(!isDefending);
        GameManager.Instance.ActiveHero(false).AttackButton.gameObject.SetActive(!isDefending);
        if (isDefending)
        {
            GameManager.Instance.ActiveHero(false).CancelButton.gameObject.SetActive(!isDefending);
            GameManager.Instance.ActiveHero(false).SubmitButton.gameObject.SetActive(!isDefending);
        }
        GameManager.Instance.ActiveHero(false).AttackButton.parent.gameObject.SetActive(isDefending);
        GameManager.Instance.ActiveHero(false).DefendButton.gameObject.SetActive(isDefending);

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            if (t.gameObject.activeSelf && !t.GetComponent<CardVisual>().IsTapped)
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
            sc.totalDamage = new Dictionary<string, int>
            {
                { "stealth", 0 },
                { "lifesteal", 0 },
                { "poisonTouch", 0 },
                { "damage", 0 }
            };
            AssignDamageToDefenders();
            GameManager.Instance.IsDefending = false;
            sc.AssignDamageToAttackers();
            ChangeUiForDefense(GameManager.Instance.IsDefending);
            GameManager.Instance.EnableOrDisablePlayerControl(true);
            StartCoroutine(GameManager.Instance.SetInstructionsText("Good Submission!"));
            ResetDamageUI();
        }
        else
        {
            StartCoroutine(GameManager.Instance.SetInstructionsText("Damage assignment does not match!"));
        }
    }

    private void AssignDamageToDefenders()
    {
        List<DamagePhoton> damageToSend = new List<DamagePhoton>();
        int index = 0;

        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            if (t.gameObject.activeSelf)
            {
                CardVisual cv = t.GetComponent<CardVisual>();
                int damage = 0;
                foreach (KeyValuePair<string, int> entry in cv.DmgAbsorbed.DamageAbsorbed)
                {
                    if (entry.Value != 0)
                    {
                        if (entry.Key.Equals("poisonTouch") && cv.Md.EffectId1 != 9)
                        {
                            cv.AdjustHealth(cv.CurrentHealth, false);
                            //EventManager.Instance.PostNotification(EVENT_TYPE.MINION_DEFEATED);
                            EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.MINION_DEFEATED);
                        }
                        else
                        {
                            if ((cv.CurrentHealth - entry.Value) == 0)
                                EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.MINION_DEFEATED);

                            cv.AdjustHealth(entry.Value, false);
                        }

                        damage += entry.Value;
                    }
                }
                if (t.GetComponent<CardVisual>().CurrentHealth > 0)
                {
                    t.GetComponent<CardVisual>().DmgAbsorbed.ResetDamageAbsorbed();
                    t.GetComponent<CardVisual>().ResetDamageObjectsUI();
                }

                damageToSend.Add(new DamagePhoton(index, damage));
                index++;
            }
        }

        int heroDamageAmount = 0;
        foreach (KeyValuePair<string, int> entry in GameManager.Instance.ActiveHero(false).GetComponent<Hero>().DmgAbsorbed.DamageAbsorbed)
        {
            if (entry.Value != 0)
            {
                GameManager.Instance.ActiveHero(false).AdjustHealth(entry.Value, false);
                if (entry.Key.Equals("lifesteal"))
                {
                    GameManager.Instance.ActiveHero(true).AdjustHealth(entry.Value, true);
                    EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_LIFESTEAL);
                }
                else if (entry.Key.Equals("poisonTouch"))
                {
                    EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_POISON_TOUCH);
                }
                else if (entry.Key.Equals("stealth"))
                {
                    EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.POWER_STEALTH);
                }

                heroDamageAmount += entry.Value;
            }
        }

        GameManager.Instance.ActiveHero(false).DmgAbsorbed.ResetDamageAbsorbed();
        GameManager.Instance.ActiveHero(false).ResetDamageObjectsUI();

        //Send the data to Attacker
        byte[] damageByte = DataHandler.Instance.ObjectToByteArray(damageToSend);
        object[] data = new object[] { damageByte, heroDamageAmount };
        StartCombatPun.Instance.SendData(ASSIGN_DEFENDING_DAMAGE, data);

        // TODO: End the game if the hero's health is 0.
        if (GameManager.Instance.ActiveHero(false).CurrentHealth == 0)
        {
            EndGamePun.Instance.SendData(END_GAME_DEFENDER);
            EndGamePun.Instance.LoseEvent();
        }
    }

    public void ResetDamageUI()
    {
        damageSelected = "";
        foreach (Transform t in GameManager.Instance.StartCombatDamageUI)
        {
            t.gameObject.GetComponentInChildren<Button>().interactable = true;
        }

        GameManager.Instance.alliedDamageCounter.text = "0";
        GameManager.Instance.alliedLifestealDamageCounter.text = "0";
        GameManager.Instance.alliedPoisonTouchDamageCounter.text = "0";
        GameManager.Instance.alliedStealthDamageCounter.text = "0";
    }
}
