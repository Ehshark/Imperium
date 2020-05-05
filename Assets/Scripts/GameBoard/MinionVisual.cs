

//public class MinionVisual : MonoBehaviour, IPointerClickHandler
//{
//    //bool isEnlarged = false;
//    public List<Transform> descriptions;

//    public GameObject summonPanel;
//    private bool isPanelEnabled = false;

//    public TMP_Text cost;
//    public TMP_Text health;
//    public TMP_Text damage;

//    public Image cardBackground;
//    public Image condition;
//    public Image allyClass;
//    public Image silenceIcon;
//    public Image effect1;
//    public Image effect2;

//    private GameObject minion;

//    void OnEnable()
//    {
//        PopulateCard();
//        UpdateCardDescriptions();
//    }

//    public void OnPointerClick(PointerEventData eventData)
//    {
//        minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
//        if (minion.transform.parent.name.Equals("Hand"))
//        {
//            if (!isPanelEnabled)
//            {
//                summonPanel.SetActive(true);
//                isPanelEnabled = true;
//            }

//            else
//            {
//                summonPanel.SetActive(false);
//                isPanelEnabled = false;
//            }

//        }

//        //if (minion.transform.parent.name != "AlliedMinionsPanel")
//        //    StartCoroutine(PlayMinion());

//    }



//    public void playCard()
//    {
//        Debug.Log("i've been clicked");
//        Debug.Log(minion.transform.name);
//        summonPanel.SetActive(false);

//        // Debug.Log(GameManager.Instance.alliedMinionZone.Find("Canvas").Find("BottomPlayer").Find("MinionArea").name);
//        minion.transform.SetParent(GameManager.Instance.alliedMinionZone);
//        //minion.transform.SetParent(GameManager.Instance.al
//        Debug.Log("i did it");
//    }



//}
//    //IEnumerator PlayMinion()
//    //{
//    //    Transform minionZone = GameManager.Instance.alliedMinionZone;
//    //    Image image = minionZone.GetComponent<Image>();
//    //    Color color = image.color;
//    //    while (image.color.a < 1) //use "< 1" when fading in
//    //    {
//    //        color.a += Time.deltaTime / 1; //fades out over 1 second. change to += to fade in
//    //        image.color = color;
//    //        yield return null;
//    //    }
//    //    color.a = .4f;
//    //    image.color = color;

//    //    PlayMinionCommand playMinionCUIManager.Instance.currentMinion = new PlayMinionCommand(minion);
//    //    playMinionCUIManager.Instance.currentMinion.AddToQueue();
//    //}

//    public void UpdateCardDescriptions()
//    {
//        TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
//        text.text = "This minion's gold and mana cost is " + UIManager.Instance.currentMinion.GoldAndManaCost;
//        text = descriptions[3].GetComponent<TMP_Text>();
//        text.text = "This minion's health is " + UIManager.Instance.currentMinion.Health;
//        text = descriptions[5].GetComponent<TMP_Text>();
//        text.text = "This minion's damage is " + UIManager.Instance.currentMinion.AttackDamage;
//        text = descriptions[7].GetComponent<TMP_Text>();
//        text.text = "When this minion attacks, if you control a " + UIManager.Instance.currentMinion.AllyClass + " minion, this minion's damage increases by 1";
//        text = descriptions[9].GetComponent<TMP_Text>();
//        text.text = UIManager.Instance.currentMinion.ConditionText;
//        text = descriptions[11].GetComponent<TMP_Text>();
//        text.text = UIManager.Instance.currentMinion.EffectText1;
//        text = descriptions[13].GetComponent<TMP_Text>();
//        text.text = UIManager.Instance.currentMinion.EffectText2;
//    }

//    void PopulateCard()
//    {
//        //set the cost
//        cost.text = UIManager.Instance.currentMinion.GoldAndManaCost.ToString();

//        //set the health
//        health.text = UIManager.Instance.currentMinion.Health.ToString();

//        //set the damage
//        damage.text = UIManager.Instance.currentMinion.AttackDamage.ToString();

//        //set the card's color
//        cardBackground.color = UIManager.Instance.currentMinion.Color;

//        //set the condition icon
//        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionConditions)
//            if (UIManager.Instance.currentMinion.ConditionID == entry.Key)
//                condition.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

//        //set the effect1 icons
//        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
//            if (UIManager.Instance.currentMinion.EffectId1 == entry.Key)
//                effect1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

//        //set the effect2 icons
//        if (UIManager.Instance.currentMinion.EffectText2.Equals(""))
//            effect2.gameObject.SetActive(false);
//        else
//        {
//            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
//                if (UIManager.Instance.currentMinion.EffectId2 == entry.Key)
//                    effect2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
//        }


//        //set the allied class icon
//        foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionClasses)
//            if (UIManager.Instance.currentMinion.AllyClassID == entry.Key)
//                allyClass.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
//    }

//    //void CollapseMinionCard()
//    //{
//    //    transform.localScale = new Vector3(1, 1, 1);
//    //    isEnlarged = false;
//    //    foreach (Transform t in descriptions)
//    //        t.gameObject.SetActive(false);
//    //}

//    //void EnlargeMinionCard()
//    //{
//    //    transform.localScale = new Vector3(4, 4, 4);
//    //    isEnlarged = true;
//    //    TMP_Text text = descriptions[13].GetComponent<TMP_Text>();
//    //    if (!text.text.Equals(""))
//    //        foreach (Transform t in descriptions)
//    //            t.gameObject.SetActive(true);
//    //    else
//    //        for (int i = 0; i < descriptions.Count - 2; i++)
//    //            descriptions[i].gameObject.SetActive(true);
//    //}
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class MinionVisual : MonoBehaviour, IPointerClickHandler
{
    public MinionData minionData { get; private set; }

    //bool isEnlarged = false;
    public List<Transform> descriptions;

    public GameObject summonPanel;
    private bool isPanelEnabled = false;

    public TMP_Text cost;
    public TMP_Text health;
    public TMP_Text damage;

    public Image cardBackground;
    public Image condition;
    public Image allyClass;
    public Image silenceIcon;
    public Image effect1;
    public Image effect2;

    private GameObject minion;

    void OnEnable()
    private int shopCounter = 0;

    void Start()
    {
        minionData = UIManager.Instance.currentMinion;

        PopulateCard();
        UpdateCardDescriptions();
    }


    public void playCard()
    {
        Debug.Log("i've been clicked");
        Debug.Log(minion.transform.name);
        summonPanel.SetActive(false);

        // Debug.Log(GameManager.Instance.alliedMinionZone.Find("Canvas").Find("BottomPlayer").Find("MinionArea").name);
        minion.transform.SetParent(GameManager.Instance.alliedMinionZone);
        //minion.transform.SetParent(GameManager.Instance.al
        Debug.Log("i did it");
    }

    public void promoteMinion(){

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        if (minion.transform.parent.name.Equals("Hand"))
        {
            if (!isPanelEnabled)
            {
                summonPanel.SetActive(true);
                isPanelEnabled = true;
            }

            else
            {
                summonPanel.SetActive(false);
                isPanelEnabled = false;
            }

        }
        //if (minion.transform.parent.name != "AlliedMinionsPanel")
        //    StartCoroutine(PlayMinion());
    }


    //IEnumerator PlayMinion()
    //{
    //    Transform minionZone = GameManager.Instance.alliedMinionZone;
    //    Image image = minionZone.GetComponent<Image>();
    //    Color color = image.color;
    //    while (image.color.a < 1) //use "< 1" when fading in
    //    {
    //        color.a += Time.deltaTime / 1; //fades out over 1 second. change to += to fade in
    //        image.color = color;
    //        yield return null;
    //    }
    //    color.a = .4f;
    //    image.color = color;

    //    PlayMinionCommand playMinionCUIManager.Instance.currentMinion = new PlayMinionCommand(minion);
    //    playMinionCUIManager.Instance.currentMinion.AddToQueue();
    //}

    public void UpdateCardDescriptions()
    {
        TMP_Text text = descriptions[1].GetComponent<TMP_Text>();
        text.text = "This minion's gold and mana cost is " + UIManager.Instance.currentMinion.GoldAndManaCost;
        text = descriptions[3].GetComponent<TMP_Text>();
        text.text = "This minion's health is " + UIManager.Instance.currentMinion.Health;
        text = descriptions[5].GetComponent<TMP_Text>();
        text.text = "This minion's damage is " + UIManager.Instance.currentMinion.AttackDamage;
        text = descriptions[7].GetComponent<TMP_Text>();
        text.text = "When this minion attacks, if you control a " + UIManager.Instance.currentMinion.AllyClass + " minion, this minion's damage increases by 1";
        text = descriptions[9].GetComponent<TMP_Text>();
        text.text = UIManager.Instance.currentMinion.ConditionText;
        text = descriptions[11].GetComponent<TMP_Text>();
        text.text = UIManager.Instance.currentMinion.EffectText1;
        text = descriptions[13].GetComponent<TMP_Text>();
        text.text = UIManager.Instance.currentMinion.EffectText2;
    }

    void PopulateCard()
    {
        if (UIManager.Instance.shopFull)
        {
            switch(this.name)
            {
                case "0":
                    shopCounter = 0;
                    break;
                case "1":
                    shopCounter = 1;
                    break;
                case "2":
                    shopCounter = 2;
                    break;
                case "3":
                    shopCounter = 3;
                    break;
                case "4":
                    shopCounter = 4;
                    break;
                case "5":
                    shopCounter = 5;
                    break;
                case "6":
                    shopCounter = 6;
                    break;
                case "7":
                    shopCounter = 7;
                    break;
                case "8":
                    shopCounter = 8;
                    break;
            }

            //set the cost
            cost.text = UIManager.Instance.currentShopCards[shopCounter].GoldAndManaCost.ToString();

            //set the health
            health.text = UIManager.Instance.currentShopCards[shopCounter].Health.ToString();

            //set the damage
            damage.text = UIManager.Instance.currentShopCards[shopCounter].AttackDamage.ToString();

            //set the card's color
            cardBackground.color = UIManager.Instance.currentShopCards[shopCounter].Color;
            //////////////
            //Debug.Log("Card type: " + UIManager.Instance.currentShopCards[shopCounter].CardClass + " and: " + shopCounter);
            //shopCounter++;
            //EACH CARD PREFAB IS CALLING A MINOINVISUAL.CS SCRIPT AT THE EXACT SAME TIME, SO COUNTER INCREMENTS WILL NOT WORK!!!
            //Debug.Log("Prefab name: " + this.name);

            //set the condition icon
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionConditions)
                if (UIManager.Instance.currentShopCards[shopCounter].ConditionID == entry.Key)
                    condition.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

            //set the effect1 icons
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                if (UIManager.Instance.currentShopCards[shopCounter].EffectId1 == entry.Key)
                    effect1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

            //set the effect2 icons
            if (UIManager.Instance.currentShopCards[shopCounter].EffectText2.Equals(""))
                effect2.gameObject.SetActive(false);
            else
            {
                foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                    if (UIManager.Instance.currentShopCards[shopCounter].EffectId2 == entry.Key)
                        effect2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
            }


            //set the allied class icon
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionClasses)
                if (UIManager.Instance.currentShopCards[shopCounter].AllyClassID == entry.Key)
                    allyClass.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        }
        else
        {

            Debug.Log("shop populate not working");
            //set the cost
            cost.text = UIManager.Instance.currentMinion.GoldAndManaCost.ToString();

            //set the health
            health.text = UIManager.Instance.currentMinion.Health.ToString();

            //set the damage
            damage.text = UIManager.Instance.currentMinion.AttackDamage.ToString();

            //set the card's color
            cardBackground.color = UIManager.Instance.currentMinion.Color;

            //set the condition icon
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionConditions)
                if (UIManager.Instance.currentMinion.ConditionID == entry.Key)
                    condition.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

            //set the effect1 icons
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                if (UIManager.Instance.currentMinion.EffectId1 == entry.Key)
                    effect1.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();

            //set the effect2 icons
            if (UIManager.Instance.currentMinion.EffectText2.Equals(""))
                effect2.gameObject.SetActive(false);
            else
            {
                foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionEffects)
                    if (UIManager.Instance.currentMinion.EffectId2 == entry.Key)
                        effect2.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
            }


            //set the allied class icon
            foreach (KeyValuePair<int, string> entry in UIManager.Instance.minionClasses)
                if (UIManager.Instance.currentMinion.AllyClassID == entry.Key)
                    allyClass.sprite = UIManager.Instance.allSprites.Where(x => x.name == entry.Value).SingleOrDefault();
        }
    }

    //void CollapseMinionCard()
    //{
    //    transform.localScale = new Vector3(1, 1, 1);
    //    isEnlarged = false;
    //    foreach (Transform t in descriptions)
    //        t.gameObject.SetActive(false);
    //}

    //void EnlargeMinionCard()
    //{
    //    transform.localScale = new Vector3(4, 4, 4);
    //    isEnlarged = true;
    //    TMP_Text text = descriptions[13].GetComponent<TMP_Text>();
    //    if (!text.text.Equals(""))
    //        foreach (Transform t in descriptions)
    //            t.gameObject.SetActive(true);
    //    else
    //        for (int i = 0; i < descriptions.Count - 2; i++)
    //            descriptions[i].gameObject.SetActive(true);
    //}
}
