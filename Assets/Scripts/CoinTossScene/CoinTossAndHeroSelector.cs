using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    const byte UPDATE_DIDWINTOSS_EVENT = 1;
    const byte SHOW_COIN_TOSS_EVENT = 2;
    const byte SHOW_HERO_UI_EVENT = 3;

    bool didWinToss; //true is heads
    bool hostChoice; //true is heads

    RaiseEventOptions raiseEventOptions;

    private void Awake()
    {
        animator = coin.GetComponent<Animator>();
        raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
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
            pOneNameText.text = PhotonNetwork.NickName + " (Host)";
            pTwoNameText.text = PhotonNetwork.PlayerListOthers[0].NickName;
        }

        else
        {
            pOneNameText.text = PhotonNetwork.NickName;
            pTwoNameText.text = PhotonNetwork.PlayerListOthers[0].NickName + " (Host)";
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

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == UPDATE_DIDWINTOSS_EVENT)
        {
            didWinToss = (bool)photonEvent.CustomData;
            StartCoroutine(FlipCoinAnimation());
        }

        else if (eventCode == SHOW_HERO_UI_EVENT)
        {
            int choice = (int)photonEvent.CustomData;

            if (choice == 1)
                chooseHeroUI.Find("Warrior").gameObject.SetActive(false);
            else if (choice == 2)
                chooseHeroUI.Find("Rogue").gameObject.SetActive(false);
            else
                chooseHeroUI.Find("Mage").gameObject.SetActive(false);

            if (chooseHeroUI.gameObject.activeSelf)
            {
                chooseHeroUI.gameObject.SetActive(false);
                instructionsText.text = "Your opponent is choosing their hero...";
            }

            else
            {
                if (StartGameController.Instance.BottomHeroChosen == 0 && StartGameController.Instance.TopHeroChosen == 0)
                {
                    if (choice == 1)
                        topHeroImage.sprite = warriorImage;
                    else if (choice == 2)
                        topHeroImage.sprite = rogueImage;
                    else
                        topHeroImage.sprite = mageImage;

                    StartGameController.Instance.TopHeroChosen = choice;
                }

                else if (StartGameController.Instance.BottomHeroChosen != 0 && StartGameController.Instance.TopHeroChosen == 0)
                {
                    if (choice == 1)
                        topHeroImage.sprite = warriorImage;
                    else if (choice == 2)
                        topHeroImage.sprite = rogueImage;
                    else
                        topHeroImage.sprite = mageImage;
                    StartGameController.Instance.TopHeroChosen = choice;
                    SceneManager.LoadScene(2);
                }

                chooseHeroUI.gameObject.SetActive(true);
                instructionsText.text = "Choose your hero.";
            }

        }

        else if (eventCode == SHOW_COIN_TOSS_EVENT)
        {
            if (!PhotonNetwork.IsMasterClient)
                hostChoice = (bool)photonEvent.CustomData;

            if (PhotonNetwork.IsMasterClient)
                GetTossResult();
        }
    }

    void GetTossResult()
    {
        int value = Random.Range(0, 1000);

        if (value % 2 == 0)
            didWinToss = true;

        else
            didWinToss = false;

        PhotonNetwork.RaiseEvent(UPDATE_DIDWINTOSS_EVENT, didWinToss, raiseEventOptions, SendOptions.SendReliable);
    }

    IEnumerator FlipCoinAnimation()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            coinTossUI.parent.gameObject.SetActive(true);
            coinTossUI.gameObject.SetActive(true);
            instructionsText.text = "";
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
            if (!PhotonNetwork.IsMasterClient)
            {
                instructionsText.text = "You have won the coin toss! Choose your hero.";
                chooseHeroUI.gameObject.SetActive(true);
            }

            else
                instructionsText.text = "Your opponent has won the coin toss! They are selecting their hero...";
        }
    }

    public void StartCoinTossAnimation(bool choice)
    {
        hostChoice = choice;
        PhotonNetwork.RaiseEvent(SHOW_COIN_TOSS_EVENT, hostChoice, raiseEventOptions, SendOptions.SendReliable);
    }

    private void ShowSecondChooseHeroUI(int choice)
    {
        if (StartGameController.Instance.BottomHeroChosen == 0)
        {
            StartGameController.Instance.BottomHeroChosen = choice;
            if (choice == 1)
                bottomHeroImage.sprite = warriorImage;
            else if (choice == 2)
                bottomHeroImage.sprite = rogueImage;
            else
                bottomHeroImage.sprite = mageImage;
        }

        PhotonNetwork.RaiseEvent(SHOW_HERO_UI_EVENT, choice, raiseEventOptions, SendOptions.SendReliable);
    }
}
