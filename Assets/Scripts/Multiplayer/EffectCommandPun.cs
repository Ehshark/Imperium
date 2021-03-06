﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCommandPun : MonoBehaviour
{
    public static EffectCommandPun Instance { get; private set; } = null;
    private RaiseEventOptions raiseEventOptions;

    //Multiplayer
    const byte ADJUST_DAMAGE_SYNC_EVENT = 23;
    const byte ADJUST_HEALTH_SYNC_EVENT = 24;
    const byte ADJUST_DISCARD_SYNC_EVENT = 25;
    const byte ACTIVATE_SILENCE_SYNC_EVENT = 29;
    const byte TAP_ANIMATION_SYNC_EVENT = 43;
    const byte ANIMATION_SYNC_EVENT = 44;
    const byte PEEK_SHOP_SYNC_EVENT = 45;
    const byte ADJUST_GOLD_SYNC_EVENT = 47;
    const byte ADJUST_EXP_SYNC_EVENT = 48;
    const byte ADJUST_HERO_HEALTH_SYNC_EVENT = 49;
    const byte DRAW_DISCARD_SYNC_EVENT = 50;
    const byte ANIMATION_MESSAGE_SYNC_EVENT = 51;
    const byte COMBAT_EFFECT_SYNC_EVENT = 52;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    }

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonData)
    {
        byte eventCode = photonData.Code;

        if (eventCode == TAP_ANIMATION_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int index = (int)data[0];

            Transform card = GameManager.Instance.GetActiveMinionZone(true).GetChild(index);

            CardVisual cv = card.GetComponent<CardVisual>();
            cv.IsTapped = true;
            cv.ChangeTappedAppearance();

            DelayCommand dc = new DelayCommand(card, 2f);
            dc.AddToQueue();

            StartCoroutine(ShowEffectAnimation());

            cv.AdjustHealth(1, false);
        }
        else if (eventCode == ANIMATION_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int index = (int)data[0];

            Transform card = GameManager.Instance.GetActiveMinionZone(true).GetChild(index);
            DelayCommand dc = new DelayCommand(card, 2f);
            dc.AddToQueue();

            StartCoroutine(ShowEffectAnimation());
        }
        else if (eventCode == PEEK_SHOP_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            string cardClass = (string)data[0];
            int[] cards = (int[])data[1];
            List<MinionData> cardList = new List<MinionData>();

            foreach (int id in cards)
            {
                MinionData minion = Resources.Load("Minions/" + id) as MinionData;
                cardList.Add(minion);
            }

            UIManager.Instance.MoveTopCardsToBottom(cardClass, cardList);
        }
        else if (eventCode == ADJUST_HEALTH_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int position = (int)data[0];
            int healthAmount = (int)data[1];
            bool increaseOrDecrease = (bool)data[2];
            bool activeOrInactive = (bool)data[3];

            Transform card = GameManager.Instance.GetActiveMinionZone(activeOrInactive).GetChild(position);
            CardVisual cv = card.GetComponent<CardVisual>();
            cv.AdjustHealth(healthAmount, increaseOrDecrease);
        }
        else if (eventCode == ADJUST_DAMAGE_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int position = (int)data[0];
            int damageAmount = (int)data[1];
            bool increaseOrDecrease = (bool)data[2];

            Transform card = GameManager.Instance.GetActiveMinionZone(true).GetChild(position);
            CardVisual cv = card.GetComponent<CardVisual>();
            cv.AdjustDamage(damageAmount, increaseOrDecrease);
        }
        else if (eventCode == ADJUST_GOLD_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int amount = (int)data[0];

            GameManager.Instance.ActiveHero(true).AdjustGold(2, true);
        }
        else if (eventCode == ADJUST_EXP_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int amount = (int)data[0];

            GameManager.Instance.ActiveHero(true).EnemyGainExp(2);
        }
        else if (eventCode == ADJUST_HERO_HEALTH_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int amount = (int)data[0];

            GameManager.Instance.ActiveHero(false).AdjustHealth(amount, false);
        }
        else if (eventCode == DRAW_DISCARD_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int position = (int)data[0];

            Transform card = GameManager.Instance.GetActiveHand(true).GetChild(position);
            GameManager.Instance.DiscardCard(card.gameObject);
        }
        else if (eventCode == ACTIVATE_SILENCE_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int position = (int)data[0];

            Transform card = GameManager.Instance.GetActiveMinionZone(false).GetChild(position);
            CardVisual cv = card.GetComponent<CardVisual>();
            cv.ActivateSilence(true);
        }
        else if (eventCode == ADJUST_DISCARD_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            bool increase = (bool)data[0];

            GameManager.Instance.ActiveHero(false).AdjustEnemyDiscard(increase);
        }
        else if (eventCode == ANIMATION_MESSAGE_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            string message = (string)data[0];

            StartCoroutine(ShowEffectAnimation(message));
        }
        else if (eventCode == COMBAT_EFFECT_SYNC_EVENT)
        {
            object[] data = (object[])photonData.CustomData;
            int position = (int)data[0];
            bool enableOrDisable = (bool)data[1];
            string to = (string)data[2];

            Transform card;
            if (to.Equals("Hand"))
                card = GameManager.Instance.GetActiveHand(true).GetChild(position);
            else
                card = GameManager.Instance.GetActiveMinionZone(true).GetChild(position);

            CardVisual cv = card.GetComponent<CardVisual>();

            cv.IsCombatEffectActivated = enableOrDisable;
            cv.CombatEffectActivated(enableOrDisable);
        }
    }

    public IEnumerator ShowEffectAnimation()
    {
        GameManager.Instance.effectText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.effectText.gameObject.SetActive(false);
    }

    public IEnumerator ShowEffectAnimation(string message)
    {
        GameManager.Instance.effectText.gameObject.SetActive(true);
        GameManager.Instance.effectText.text = message;
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.effectText.text = "Effect Activated!";
        GameManager.Instance.effectText.gameObject.SetActive(false);
    }

    public void SendData(byte byteCode, object data)
    {
        PhotonNetwork.RaiseEvent(byteCode, data, raiseEventOptions, SendOptions.SendReliable);
    }
}
