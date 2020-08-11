using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCombatPun : MonoBehaviour
{
    public static StartCombatPun Instance { get; private set; } = null;
    public bool attackAfter = false;

    private const byte START_COMBAT = 17;
    private const byte ASSIGN_DEFENDING_DAMAGE = 18;
    private const byte ASSIGN_AFTER_EFFECT_SYNC_EVENT = 46;

    private List<CardVisual> minionsLeft = new List<CardVisual>();

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == START_COMBAT)
        {
            object[] data = (object[])photonEvent.CustomData;
            int[] damages = (int[])data[0];
            List<CardPhoton> cards = (List<CardPhoton>)DataHandler.Instance.ByteArrayToObject((byte[])data[1]);
            bool heroAttacking = (bool)data[2];

            if (heroAttacking)
            {
                GameManager.Instance.ActiveHero(true).AdjustMana(GameManager.Instance.ActiveHero(true).Damage, false);
            }

            TapMinions(cards);

            GameManager.Instance.StartCombatDamageUI.gameObject.SetActive(true);
            GameManager.Instance.alliedStealthDamageCounter.text = damages[0].ToString();
            GameManager.Instance.alliedLifestealDamageCounter.text = damages[1].ToString();
            GameManager.Instance.alliedPoisonTouchDamageCounter.text = damages[2].ToString();
            GameManager.Instance.alliedDamageCounter.text = damages[3].ToString();

            StartCombat sc = GameManager.Instance.ActiveHero(true).AttackButton.parent.GetComponent<StartCombat>();
            sc.totalDamage = new Dictionary<string, int>
            {
                { "stealth", damages[0] },
                { "lifesteal", damages[1] },
                { "poisonTouch", damages[2] },
                { "damage", damages[3] }
            };

            EventManager.Instance.PostNotification(EVENT_TYPE.DEFEND_AGAINST);
            GameManager.Instance.shopButton.interactable = false;
        }
        else if (eventCode == ASSIGN_DEFENDING_DAMAGE)
        {
            object[] data = (object[])photonEvent.CustomData;
            List<DamagePhoton> damageToAssign = (List<DamagePhoton>)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);
            Dictionary<string, int> heroDamage = (Dictionary<string, int>)DataHandler.Instance.ByteArrayToObject((byte[])data[1]);
            bool minionDefeated = (bool)data[2];
            int totalHeroDamage = 0;
            foreach (KeyValuePair<string, int> entry in heroDamage)
            {
                totalHeroDamage += entry.Value;
            }
            AssignDamageToDefenders(damageToAssign, heroDamage);
            if (totalHeroDamage > 0)
            {
                //Add Bleed to the queue
                EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.BLEED);
            }
            if (minionDefeated)
            {
                //Add Minion Defeated to the queue
                EffectCommand.Instance.EffectQueue.Enqueue(EVENT_TYPE.MINION_DEFEATED);
            }

            GameManager.Instance.IsDefending = false;

            AssignDamageToAttackers();

            DefendListener dl = GameManager.Instance.GetComponent<DefendListener>();
            dl.ChangeUiForDefense(GameManager.Instance.IsDefending);
            GameManager.Instance.EnableOrDisablePlayerControl(true);
            GameManager.Instance.endButton.interactable = true;
            dl.ResetDamageUI();

            StartCombat sc = GameManager.Instance.ActiveHero(true).AttackButton.parent.GetComponent<StartCombat>();
            sc.totalDamage = new Dictionary<string, int>
            {
                { "stealth", 0 },
                { "lifesteal", 0 },
                { "poisonTouch", 0 },
                { "damage", 0 }
            };

            GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "";
        }
        else if (eventCode == ASSIGN_AFTER_EFFECT_SYNC_EVENT)
        {
            object[] data = (object[])photonEvent.CustomData;
            int[] minions = (int[])data[0];

            int i = 0;
            foreach (Transform card in GameManager.Instance.GetActiveMinionZone(true))
            {
                CardVisual cv = card.GetComponent<CardVisual>();
                if (cv.Md.MinionID == minions[i])
                {
                    cv.AdjustHealth(1, false);
                }

                i++;
            }
        }
    }

    private void TapMinions(List<CardPhoton> cards)
    {
        foreach (CardPhoton card in cards)
        {
            foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
            {
                CardVisual cv = t.GetComponent<CardVisual>();

                if (cv.Md != null && card is MinionDataPhoton)
                {
                    MinionDataPhoton mdp = (MinionDataPhoton)card;
                    if (mdp.MinionID == cv.Md.MinionID)
                    {
                        cv.IsTapped = true;
                        cv.ChangeTappedAppearance();
                    }
                }
                else if (cv.Sd != null && card is StarterDataPhoton)
                {
                    StarterDataPhoton sdp = (StarterDataPhoton)card;
                    if (sdp.StarterID == cv.Sd.StarterID)
                    {
                        cv.IsTapped = true;
                        cv.ChangeTappedAppearance();
                    }
                }
            }
        }
    }

    private void AssignDamageToDefenders(List<DamagePhoton> damages, Dictionary<string, int> heroDamage)
    {
        int i = 0;
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            CardVisual cv = t.GetComponent<CardVisual>();
            DamagePhoton damage = damages[i];
            foreach (KeyValuePair<string, int> entry in damage.DamageAbsorbed)
            {
                if (entry.Value != 0)
                {
                    if (entry.Key.Equals("poisonTouch") && (cv.Sd || cv.Md.EffectId1 != 9))
                    {
                        cv.AdjustHealth(cv.CurrentHealth, false);
                    }
                    else
                    {
                        cv.AdjustHealth(entry.Value, false);
                    }
                }
            }
            if (cv.CurrentHealth > 0)
            {
                cv.DmgAbsorbed.ResetDamageAbsorbed();
                cv.ResetDamageObjectsUI();
            }
            i++;
        }

        foreach (KeyValuePair<string, int> entry in heroDamage)
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
            }
        }
    }

    private void AssignDamageToAttackers()
    {
        foreach (GameObject card in GameManager.Instance.MinionsAttacking)
        {
            CardVisual cv = card.GetComponent<CardVisual>();
            ConditionListener cl = card.GetComponent<ConditionListener>();

            if (cv.Md != null && cl != null)
            {
                if ((cl.ConditionEvent == EVENT_TYPE.BLEED || cl.ConditionEvent == EVENT_TYPE.MINION_DEFEATED) && (cv.CurrentHealth - 1) == 0)
                {
                    minionsLeft.Add(cv);
                }
                else
                {
                    cv.AdjustHealth(1, false);
                }
            }
            else
            {
                cv.AdjustHealth(1, false);
            }
        }

        if (minionsLeft.Count > 0)
        {
            attackAfter = true;
        }

        GameManager.Instance.MinionsAttacking.Clear();
    }

    public void DamageDelay()
    {
        StartCoroutine(AssignDamageToAttackersAfter());
    }

    public IEnumerator AssignDamageToAttackersAfter()
    {
        int[] attackingMinions = new int[minionsLeft.Count];
        for (int i = 0; i < minionsLeft.Count; i++)
        {
            attackingMinions[i] = minionsLeft[i].Md.MinionID;
        }

        object[] data = new object[] { attackingMinions };
        SendData(ASSIGN_AFTER_EFFECT_SYNC_EVENT, data);

        foreach (CardVisual card in minionsLeft)
        {
            card.AdjustHealth(1, false);
            yield return new WaitForSeconds(1f);
        }

        minionsLeft.Clear();
    }

    public void SendData(byte byteCode, object data)
    {
        PhotonNetwork.RaiseEvent(byteCode, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);
    }
}
