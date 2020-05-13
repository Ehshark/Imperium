using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayCard : MonoBehaviour
{
    private GameObject minion;
    private GameObject item;
    private GameObject summonPanel;

    private void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.name.Equals("SummonPanel"))
                summonPanel = t.gameObject;
        }

        foreach (Transform t in summonPanel.transform)
        {
            if (t.name.Equals("PlayMinionButton"))
            {
                t.GetComponent<Button>().onClick.AddListener(PlayMinion);
                t.gameObject.SetActive(true);
            }

            else if (t.name.Equals("PromoteMinionButton"))
            {
                t.GetComponent<Button>().onClick.AddListener(StartPromoteMinion);
                t.gameObject.SetActive(true);
            }

            else if (t.name.Equals("UseButton"))
            {
                t.GetComponent<Button>().onClick.AddListener(PlayItem);
                t.gameObject.SetActive(true);
            }

        }
    }

    //Connected to the play button in summon panel
    public void PlayMinion()
    {
        summonPanel.SetActive(false);
        StartCoroutine(MoveMinion());
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
        if (gameObject != UIManager.Instance.LastSelectedMinion && UIManager.Instance.LastSelectedMinion != null)
        {
            foreach (Transform t in UIManager.Instance.LastSelectedMinion.transform)
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

    IEnumerator MoveMinion()
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

        minion = gameObject;

        if (GameManager.Instance.IsPromoting)
        {
            minion = GameManager.Instance.MinionToPromote;
            GameManager.Instance.IsPromoting = false;
            GameManager.Instance.EnableOrDisablePlayerControl(true);
            EventManager.Instance.PostNotification(EVENT_TYPE.SACRIFICE_MINION);
        }

        PlayMinionCommand pmc = new PlayMinionCommand(minion, GameManager.Instance.alliedHand, GameManager.Instance.alliedMinionZone);
        pmc.AddToQueue();
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

        DestroyMinionCommand dmc = new DestroyMinionCommand(GameManager.Instance.MinionToSacrifice);
        dmc.AddToQueue();
        StartCoroutine(MoveMinion());
    }

    public void PlayItem()
    {
        summonPanel.SetActive(false);
        StartCoroutine(MoveItem());
    }

    IEnumerator MoveItem()
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
    }
}
