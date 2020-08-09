using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardPun : MonoBehaviour
{
    public static PlayCardPun Instance { get; private set; } = null;
    private RaiseEventOptions raiseEventOptions;

    private const byte PLAY_MINION = 8;
    private const byte PLAY_RESOURCE = 9;
    private const byte PROMOTE_MINION = 10;
    private const byte REMOVE_MINION = 11;

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

        if (eventCode == PLAY_MINION)
        {
            object[] data = (object[])photonData.CustomData;
            string type = (string)data[1];

            if (type == "Minion")
            {
                MinionDataPhoton mdp = (MinionDataPhoton)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);
                foreach(Transform t in GameManager.Instance.GetActiveHand(true))
                {
                    CardVisual cv = t.GetComponent<CardVisual>();
                    if (cv.Md != null)
                    {
                        if (cv.Md.MinionID == mdp.MinionID)
                        {
                            StartCoroutine(MoveCard(t, cv));
                            break;
                        }
                    }
                }
            }
            else
            {
                StarterDataPhoton sdp = (StarterDataPhoton)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);
                foreach (Transform t in GameManager.Instance.GetActiveHand(true))
                {
                    CardVisual cv = t.GetComponent<CardVisual>();
                    if (cv.Sd != null)
                    {
                        if (cv.Sd.StarterID == sdp.StarterID)
                        {
                            StartCoroutine(MoveCard(t, cv));
                        }
                    }
                }
            }
        }
        else if (eventCode == PLAY_RESOURCE)
        {
            object[] data = (object[])photonData.CustomData;
            int position = (int)data[0];
            string type = (string)data[1];

            if (type == "Starter")
            {
                Transform card = GameManager.Instance.GetActiveHand(true).GetChild(position);
                CardVisual cv = card.GetComponent<CardVisual>();
                StartCoroutine(PlayResource(card, cv.Sd));
            }
            else if (type == "Essential")
            {
                Transform card = GameManager.Instance.GetActiveHand(true).GetChild(position);
                CardVisual cv = card.GetComponent<CardVisual>();
                StartCoroutine(PlayResource(card, cv.Ed));
            }
        }
        else if (eventCode == REMOVE_MINION)
        {
            object[] data = (object[])photonData.CustomData;
            string type = (string)data[1];

            if (type == "Minion")
            {
                MinionDataPhoton mdp = (MinionDataPhoton)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);
                foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
                {
                    CardVisual cv = t.GetComponent<CardVisual>();
                    if (cv.Md != null)
                    {
                        if (cv.Md.MinionID == mdp.MinionID)
                        {
                            StartCoroutine(SendCardToDiscard(t, cv));
                        }
                    }
                }
            }
            else
            {
                StarterDataPhoton sdp = (StarterDataPhoton)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);
                foreach (Transform t in GameManager.Instance.GetActiveMinionZone(true))
                {
                    CardVisual cv = t.GetComponent<CardVisual>();
                    if (cv.Sd != null)
                    {
                        if (cv.Sd.StarterID == sdp.StarterID)
                        {
                            StartCoroutine(SendCardToDiscard(t, cv));
                        }
                    }
                }
            }
        }
        else if (eventCode == PROMOTE_MINION)
        {
            object[] data = (object[])photonData.CustomData;
            string type = (string)data[1];

            if (type == "Minion")
            {
                MinionDataPhoton mdp = (MinionDataPhoton)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);
                foreach (Transform t in GameManager.Instance.GetActiveHand(true))
                {
                    CardVisual cv = t.GetComponent<CardVisual>();
                    if (cv.Md != null)
                    {
                        if (cv.Md.MinionID == mdp.MinionID)
                        {
                            StartCoroutine(PromoteMinion(t, cv));
                        }
                    }
                }
            }
            else
            {
                StarterDataPhoton sdp = (StarterDataPhoton)DataHandler.Instance.ByteArrayToObject((byte[])data[0]);
                foreach (Transform t in GameManager.Instance.GetActiveHand(true))
                {
                    CardVisual cv = t.GetComponent<CardVisual>();
                    if (cv.Sd != null)
                    {
                        if (cv.Sd.StarterID == sdp.StarterID)
                        {
                            StartCoroutine(PromoteMinion(t, cv));
                        }
                    }
                }
            }
        }
    }

    private IEnumerator MoveCard(Transform card, CardVisual cv)
    {
        int cost = int.Parse(cv.cost.text);
        GameManager.Instance.ActiveHero(true).AdjustMana(cost, false);

        DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true), 1f);
        dc.AddToQueue();

        yield return new WaitForSeconds(1);

        MoveCardCommand mc = new MoveCardCommand(card.gameObject, GameManager.Instance.GetActiveMinionZone(true));
        mc.AddToQueue();

        card.Find("CardBack").gameObject.SetActive(false);

        if (cv.Md != null)
        {
            RemoveCardFromHand(cv.Md);
        }
        else
        {
            RemoveCardFromHand(cv.Sd);
        }
    }

    private IEnumerator PromoteMinion(Transform card, CardVisual cv)
    {
        yield return new WaitForSeconds(1);

        int cost = int.Parse(cv.cost.text);
        GameManager.Instance.ActiveHero(true).AdjustMana(cost, false);

        DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true), 1f);
        dc.AddToQueue();

        yield return new WaitForSeconds(1);

        MoveCardCommand mc = new MoveCardCommand(card.gameObject, GameManager.Instance.GetActiveMinionZone(true));
        mc.AddToQueue();

        cv.AdjustHealth(2, true);
        cv.PromotedHealth = cv.CurrentHealth;

        if (cv.Md != null)
        {
            RemoveCardFromHand(cv.Md);
        }
        else
        {
            RemoveCardFromHand(cv.Sd);
        }

        card.Find("CardBack").gameObject.SetActive(false);
    }

    private IEnumerator PlayResource(Transform card, Card cardData)
    {
        card.Find("CardBack").gameObject.SetActive(false);
        AdjustHeroResources(cardData);

        DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true), 1f);
        dc.AddToQueue();

        yield return new WaitForSeconds(1);

        MoveCardCommand mc = new MoveCardCommand(card.gameObject, GameManager.Instance.GetActiveDiscardPile(true));
        mc.AddToQueue();

        if (cardData is StarterData)
        {
            StarterData sd = (StarterData)cardData;
            RemoveCardFromHand(sd);
            AddCardToDiscardPile(sd);
        }
        else if (cardData is EssentialsData)
        {
            EssentialsData ed = (EssentialsData)cardData;
            RemoveCardFromHand(ed);
            AddCardToDiscardPile(ed);            
        }
    }

    private void AdjustHeroResources(Card card)
    {
        Hero hero = GameManager.Instance.ActiveHero(true);

        if (card is StarterData)
        {
            StarterData starter = (StarterData)card;
            if (starter.EffectId1 == 18)
            {
                hero.AdjustGold(2, true);
                hero.AdjustMana(1, false);
                hero.EnemyGainExp(1);
            }
            else if (starter.EffectId1 == 20)
            {
                hero.AdjustHealth(1, true);
            }
            else if (starter.EffectId1 == 21)
            {
                hero.AdjustMana(1, true);
            }
        }
        else if (card is EssentialsData)
        {
            EssentialsData essentials = (EssentialsData)card;
            if (essentials.EffectId1 == 18)
            {
                hero.AdjustGold(4, true);
                hero.GainExp(2);
                hero.AdjustMana(2, false);
            }
            else if (essentials.EffectId1 == 20)
            {
                hero.AdjustHealth(2, true);
            }
            else if (essentials.EffectId1 == 21)
            {
                hero.AdjustMana(2, true);
            }
            else if (essentials.EffectId1 == 14)
            {
                hero.AdjustDamage(1, true);
                hero.AdjustMana(2, false);
            }
        }
    }

    private IEnumerator SendCardToDiscard(Transform card, CardVisual cv)
    {
        DelayCommand dc = new DelayCommand(GameManager.Instance.GetActiveHand(true), 1f);
        dc.AddToQueue();

        yield return new WaitForSeconds(1f);

        MoveCardCommand mc = new MoveCardCommand(card.gameObject, GameManager.Instance.GetActiveDiscardPile(true));
        mc.AddToQueue();

        if (cv.Md != null)
        {
            AddCardToDiscardPile(cv.Md);
        }
        else
        {
            AddCardToDiscardPile(cv.Sd);
        }
    }

    private void RemoveCardFromHand(Card card)
    {
        UIManager.Instance.GetActiveHandList(true).Remove(card);
    }

    private void AddCardToDiscardPile(Card card)
    {
        UIManager.Instance.GetActiveDiscardList(true).Add(card);
    }

    public void SendData(byte byteCode, object data)
    {
        PhotonNetwork.RaiseEvent(byteCode, data, raiseEventOptions, SendOptions.SendReliable);
    }
}
