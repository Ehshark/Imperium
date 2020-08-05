using ExitGames.Client.Photon;
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
    const byte ACTIVATE_SILENCE_SYNC_EVENT = 29;
    const byte TAP_ANIMATION_SYNC_EVENT = 43;
    const byte ANIMATION_SYNC_EVENT = 44;
    const byte PEEK_SHOP_SYNC_EVENT = 44;

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
            List<int> cards = (List<int>)data[1];
            List<MinionData> cardList = new List<MinionData>();

            foreach (int id in cards)
            {
                MinionData minion = Resources.Load("Minions/" + id) as MinionData;
                cardList.Add(minion);
            }

            UIManager.Instance.MoveTopCardsToBottom(cardClass, cardList);
        }
    }

    private IEnumerator ShowEffectAnimation()
    {
        GameManager.Instance.effectText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.effectText.gameObject.SetActive(false);
    }

    public void SendData(byte byteCode, object data)
    {
        PhotonNetwork.RaiseEvent(byteCode, data, raiseEventOptions, SendOptions.SendReliable);
    }
}
