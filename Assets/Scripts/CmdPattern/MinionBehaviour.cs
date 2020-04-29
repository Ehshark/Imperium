using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MinionBehaviour : MonoBehaviour, IPointerDownHandler
{
    public bool hasBattlecry;
    GameObject minion;
    bool isEnlarged = false;
    public List<Transform> descriptions;

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (GameManager.Instance.canPlayCards)
        //{
        //    GameManager.Instance.canPlayCards = false;
        //    minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        //    if (minion.transform.parent.name != "AlliedMinionsPanel")
        //        StartCoroutine(PlayMinion());
        //}

        if (eventData.button == PointerEventData.InputButton.Right && isEnlarged)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isEnlarged = false;
            foreach (Transform t in descriptions)
                t.gameObject.SetActive(false);
        }

        else if (eventData.button == PointerEventData.InputButton.Right && !isEnlarged)
        {
            transform.localScale = new Vector3(4, 4, 4);
            isEnlarged = true;
            CardData cd = UIManager.Instance.cards[UIManager.Instance.cardIndex];
            TMP_Text text = descriptions[13].GetComponent<TMP_Text>();
            if (!text.text.Equals(""))
                foreach (Transform t in descriptions)
                    t.gameObject.SetActive(true);
            else { 
                for(int i = 0; i < descriptions.Count - 2; i++)
                    descriptions[i].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator PlayMinion()
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

        PlayMinionCommand playMinionCmd = new PlayMinionCommand(minion);
        playMinionCmd.AddToQueue();
    }

    void Start()
    {
        hasBattlecry = true;
    }

    public void UpdateCardDescriptions()
    {
        CardData cd = UIManager.Instance.cards[UIManager.Instance.cardIndex];
        TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
        text.text = "This minion's gold and mana cost is " + cd.GoldAndManaCost;
        text = descriptions[3].GetComponent<TMP_Text>();
        text.text = "This minion's health is " + cd.Health;
        text = descriptions[5].GetComponent<TMP_Text>();
        text.text = "This minion's damage is " + cd.AttackDamage;
        text = descriptions[7].GetComponent<TMP_Text>();
        text.text = "When this minion attacks, if you control a " + cd.AllyClass + " minion, this minion's damage increases by 1";
        text = descriptions[9].GetComponent<TMP_Text>();
        text.text = cd.ConditionText;
        text = descriptions[11].GetComponent<TMP_Text>();
        text.text = cd.EffectText1;
        text = descriptions[13].GetComponent<TMP_Text>();
        text.text = cd.EffectText2;
    }
}
