using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinTossAndHeroSelector : MonoBehaviour
{
    public TMP_Text pOneNameText;
    public TMP_Text pTwoNameText;
    public TMP_Text instructionsText;

    public Sprite warriorImage;
    public Sprite rogueImage;
    public Sprite mageImage;
    public Image bottomHeroImage;
    public Image topHeroImage;

    public Transform chooseHeroUI;
    public Transform coinTossUI;
    public Transform coin;

    public Button heads;
    public Button tails;
    public Button warrior;
    public Button rogue;
    public Button mage;

    private Animator animator;
    const byte SHOW_COIN_TOSS_EVENT = 1;
    const byte SHOW_HERO_UI_EVENT = 2;
    const byte GO_TO_GAMEBOARD_EVENT = 3;

    bool didWinToss; //true is heads
    bool hostChoice; //true is heads

    RaiseEventOptions raiseEventOptions;

    private void Awake()
    {
        animator = coin.GetComponent<Animator>();
        raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
    }

    // Start is called before the first frame update
    void Start()
    {
        heads.onClick.AddListener(delegate { StartCoinTossAnimation(true); });
        tails.onClick.AddListener(delegate { StartCoinTossAnimation(false); });
        warrior.onClick.AddListener(delegate { ShowSecondChooseHeroUI(1); });
        rogue.onClick.AddListener(delegate { ShowSecondChooseHeroUI(2); });
        mage.onClick.AddListener(delegate { ShowSecondChooseHeroUI(3); });

        if (PhotonNetwork.IsMasterClient)
        {
            pOneNameText.text = PhotonNetwork.NickName + " - Host";
            pTwoNameText.text = PhotonNetwork.PlayerListOthers[0].NickName;
        }

        else
        {
            pOneNameText.text = PhotonNetwork.NickName;
            pTwoNameText.text = PhotonNetwork.PlayerListOthers[0].NickName + " - Host";
        }

        if (!PhotonNetwork.IsMasterClient)
            instructionsText.text = "Waiting for the host to toss the coin...";

        if (PhotonNetwork.IsMasterClient)
        {
            coinTossUI.parent.gameObject.SetActive(true);
            coinTossUI.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent) //This code block will only be executed on the client, i.e. not the host
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == SHOW_COIN_TOSS_EVENT)
        {
            bool[] bools = (bool[])photonEvent.CustomData;
            hostChoice = bools[0];
            didWinToss = bools[1];
            StartCoroutine(FlipCoinAnimation());
        }

        else if (eventCode == SHOW_HERO_UI_EVENT)
        {
            int choice = (int)photonEvent.CustomData;

            if (choice == 1)
            {
                chooseHeroUI.Find("Warrior").gameObject.SetActive(false);
                topHeroImage.sprite = warriorImage;
            }

            else if (choice == 2)
            {
                chooseHeroUI.Find("Rogue").gameObject.SetActive(false);
                topHeroImage.sprite = rogueImage;
            }

            else if (choice == 3)
            {
                chooseHeroUI.Find("Mage").gameObject.SetActive(false);
                topHeroImage.sprite = mageImage;
            }

            if (PhotonNetwork.IsMasterClient)
                EventManager.Instance.ClientChoice = choice;
            else
                EventManager.Instance.HostChoice = choice;

            int count = 0;

            foreach (Transform child in chooseHeroUI)
            {
                if (child.gameObject.activeSelf)
                    count++;
            }

            if (count == 1)
            {
                PhotonNetwork.RaiseEvent(GO_TO_GAMEBOARD_EVENT, choice, new RaiseEventOptions { Receivers = ReceiverGroup.All },
                        SendOptions.SendReliable);
                return;
            }

            chooseHeroUI.gameObject.SetActive(true);
        }

        else if (eventCode == GO_TO_GAMEBOARD_EVENT)
        {
            PhotonNetwork.LoadLevel(2);
        }
    }
    public void StartCoinTossAnimation(bool choice)
    {
        hostChoice = choice;
        GetTossResult();
        bool[] bools = new bool[] { hostChoice, didWinToss };
        PhotonNetwork.RaiseEvent(SHOW_COIN_TOSS_EVENT, bools, raiseEventOptions, SendOptions.SendReliable);
        StartCoroutine(FlipCoinAnimation());
    }

    void GetTossResult()
    {
        int value = Random.Range(0, 1000);

        if (value % 2 == 0)
            didWinToss = true;

        else
            didWinToss = false;
    }

    IEnumerator FlipCoinAnimation()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            coinTossUI.parent.gameObject.SetActive(true);
            coinTossUI.gameObject.SetActive(true);
        }
        heads.interactable = false;
        tails.interactable = false;
        animator.SetBool("flip", true);
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("flip", false);
        if (didWinToss)
        {
            coin.Find("Heads").gameObject.SetActive(true);
            coin.Find("Tails").gameObject.SetActive(false);
        }
        else
        {
            coin.Find("Tails").gameObject.SetActive(true);
            coin.Find("Heads").gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        coinTossUI.gameObject.SetActive(false);

        if (hostChoice == didWinToss)
        {
            EventManager.Instance.HostFirst = true;
            if (!PhotonNetwork.IsMasterClient)
                instructionsText.text = "Your opponent has won the coin toss! They are selecting their hero...";
            else
            {
                instructionsText.text = "You have won the coin toss! Choose your hero.";
                chooseHeroUI.gameObject.SetActive(true);
            }
        }
        else
        {
            EventManager.Instance.HostFirst = false;
            if (!PhotonNetwork.IsMasterClient)
            {
                instructionsText.text = "You have won the coin toss! Choose your hero.";
                chooseHeroUI.gameObject.SetActive(true);
            }

            else
                instructionsText.text = "Your opponent has won the coin toss! They are selecting their hero...";
        }
    }


    private void ShowSecondChooseHeroUI(int choice)
    {
        if (!chooseHeroUI.gameObject.activeSelf)
            chooseHeroUI.gameObject.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
            EventManager.Instance.HostChoice = choice;
        else
            EventManager.Instance.ClientChoice = choice;

        if (choice == 1)
        {
            chooseHeroUI.Find("Warrior").gameObject.SetActive(false);
            bottomHeroImage.sprite = warriorImage;
        }

        else if (choice == 2)
        {
            chooseHeroUI.Find("Rogue").gameObject.SetActive(false);
            bottomHeroImage.sprite = rogueImage;
        }

        else if (choice == 3)
        {
            chooseHeroUI.Find("Mage").gameObject.SetActive(false);
            bottomHeroImage.sprite = mageImage;
        }

        PhotonNetwork.RaiseEvent(SHOW_HERO_UI_EVENT, choice, raiseEventOptions, SendOptions.SendReliable);
        chooseHeroUI.gameObject.SetActive(false);
    }
}
