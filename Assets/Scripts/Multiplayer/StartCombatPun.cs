using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombatPun : MonoBehaviour
{
    public static StartCombatPun Instance { get; private set; } = null;

    private const byte START_COMBAT = 17;
    private const byte ASSIGN_DEFENDING_DAMAGE = 18;

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
            int heroDamage = (int)data[1];

            AssignDamageToDefenders(damageToAssign);
            GameManager.Instance.ActiveHero(false).AdjustHealth(heroDamage, false);
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
        }
    }

    private void TapMinions(List<CardPhoton> cards)
    {
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
        {
            CardVisual cv = t.GetComponent<CardVisual>();

            if (cv.Md != null)
            {
                foreach (CardPhoton card in cards)
                {
                    MinionDataPhoton mdp = (MinionDataPhoton)card;
                    if (mdp.MinionID == cv.Md.MinionID)
                    {
                        cv.IsTapped = true;
                        cv.ChangeTappedAppearance();
                    }
                }
            }
            else
            {
                foreach (CardPhoton card in cards)
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

    private void AssignDamageToDefenders(List<DamagePhoton> damages)
    {
        int i = 0;
        foreach (Transform t in GameManager.Instance.GetActiveMinionZone(false))
        {
            CardVisual cv = t.GetComponent<CardVisual>();
            DamagePhoton damage = damages[i];

            cv.AdjustHealth(damage.GetDamage(), false);
            i++;
        }
    }

    private void AssignDamageToAttackers()
    {
        foreach (GameObject card in GameManager.Instance.MinionsAttacking)
        {
            CardVisual cv = card.GetComponent<CardVisual>();
            cv.AdjustHealth(1, false);
        }

        GameManager.Instance.MinionsAttacking.Clear();
    }

    public void SendData(byte byteCode, object data)
    {
        PhotonNetwork.RaiseEvent(byteCode, data, new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable);
    }
}
