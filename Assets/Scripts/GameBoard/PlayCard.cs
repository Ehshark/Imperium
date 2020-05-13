using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayCard : MonoBehaviour
{
    private GameObject card;
    private GameObject summonPanel;

    private void Start()
    {
        if (gameObject.CompareTag("Minion"))
        {
            foreach (Transform t in transform)
                if (t.name.Equals("SummonPanel"))
                    summonPanel = t.gameObject;

            foreach (Transform t in summonPanel.transform)
            {
                if (t.name.Equals("PlayMinionButton"))
                    t.GetComponent<Button>().onClick.AddListener(PlayMinion);

                else if (t.name.Equals("PromoteMinionButton"))
                    t.GetComponent<Button>().onClick.AddListener(StartPromoteMinion);
            }
        }

        else if (gameObject.CompareTag("Essential"))
        {
            foreach (Transform t in transform)
                if (t.name.Equals("UsePanel"))
                    summonPanel = t.gameObject;

            foreach (Transform t in summonPanel.transform)
                if (t.name.Equals("UseButton"))
                    t.GetComponent<Button>().onClick.AddListener(PlayItem);
        }

        else if (gameObject.CompareTag("Starter")) {
            StarterVisual sv = gameObject.GetComponent<StarterVisual>();
            if (sv.Sd.AttackDamage == 0)
            {
                foreach (Transform t in transform)
                    if (t.name.Equals("UsePanel"))
                        summonPanel = t.gameObject;

                foreach (Transform t in summonPanel.transform)
                    if (t.name.Equals("UseButton"))
                        t.GetComponent<Button>().onClick.AddListener(PlayItem);
            }

            else {
                foreach (Transform t in transform)
                    if (t.name.Equals("SummonPanel"))
                        summonPanel = t.gameObject;

                foreach (Transform t in summonPanel.transform)
                {
                    if (t.name.Equals("PlayMinionButton"))
                        t.GetComponent<Button>().onClick.AddListener(PlayMinion);

                    else if (t.name.Equals("PromoteMinionButton"))
                        t.GetComponent<Button>().onClick.AddListener(StartPromoteMinion);
                }
            }
        }

    }

    //Connected to the play button in summon panel
    public void PlayMinion()
    {
        summonPanel.SetActive(false);
        StartCoroutine(MoveCardFromHand(true));
    }

    //Connected to the promote button in summon panel
    public void StartPromoteMinion()
    {
        TMP_Text text = GameManager.Instance.instructionsObj.GetComponent<TMP_Text>();
        text.text = "Please select an enemy minion to sacrifice";
        GameManager.Instance.EnableOrDisablePlayerControl(false);
        GameManager.Instance.MinionToPromote = gameObject;
        GameManager.Instance.IsPromoting = true;
        EventManager.Instance.PostNotification(EVENT_TYPE.SACRIFICE_MINION);
        summonPanel.SetActive(false);
    }

    public void ShowSummonPanel()
    {
        //minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        if (gameObject != UIManager.Instance.LastSelectedCard && UIManager.Instance.LastSelectedCard != null)
        {
            foreach (Transform t in UIManager.Instance.LastSelectedCard.transform)
            {
                if (t.name.Equals("SummonPanel"))
                    t.gameObject.SetActive(false);
            }
        }

        if (summonPanel.activeSelf)
            summonPanel.SetActive(false);

        else
            summonPanel.SetActive(true);
    }

    public void StartPromotion()
    {
        StartCoroutine(PromoteMinionWithPlayback());
    }

    IEnumerator MoveCardFromHand(bool isMinion)
    {
        Transform minionZone = GameManager.Instance.alliedHand;
        Image image = minionZone.GetComponent<Image>();
        Color color = image.color;
        while (image.color.a < 1) //use "< 1" when fading in
        {
            color.a += Time.deltaTime / 1; //fades out over 1 second. change to += to fade in
            image.color = color;
            yield return null;
        }
        color.a = .4f;
        image.color = color;

        card = gameObject;

        if (isMinion)
        {
            if (GameManager.Instance.IsPromoting)
            {
                card = GameManager.Instance.MinionToPromote;
                GameManager.Instance.IsPromoting = false;
                GameManager.Instance.EnableOrDisablePlayerControl(true);
                TMP_Text text = GameManager.Instance.instructionsObj.GetComponent<TMP_Text>();
                text.text = "";
                EventManager.Instance.PostNotification(EVENT_TYPE.SACRIFICE_MINION);
            }

            MoveCardCommand mc = new MoveCardCommand(card, GameManager.Instance.alliedHand, GameManager.Instance.alliedMinionZone);
            mc.AddToQueue();
        }

        else
        {
            MoveCardCommand mc = new MoveCardCommand(card, GameManager.Instance.alliedHand, GameManager.Instance.alliedDiscardPile);
            mc.AddToQueue();
        }
    }

    IEnumerator PromoteMinionWithPlayback()
    {
        Transform minionZone = GameManager.Instance.alliedMinionZone;
        Image image = minionZone.GetComponent<Image>();
        Color color = image.color;
        while (image.color.a < 1) //use "< 1" when fading in
        {
            color.a += Time.deltaTime / 1; //fades out over 1 second. change to += to fade in
            image.color = color;
            yield return null;
        }
        color.a = .4f;
        image.color = color;

        MoveCardCommand mc = new MoveCardCommand(GameManager.Instance.MinionToSacrifice,
            GameManager.Instance.alliedMinionZone, GameManager.Instance.alliedDiscardPile);
        mc.AddToQueue();
        StartCoroutine(MoveCardFromHand(true));
    }

    public void PlayItem()
    {
        summonPanel.SetActive(false);
        StartCoroutine(MoveCardFromHand(false));
    }
}
