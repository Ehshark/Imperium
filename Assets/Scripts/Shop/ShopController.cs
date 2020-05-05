using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    //Components 
    [SerializeField]
    private GameObject rightShopUI;
    [SerializeField]
    private GameObject shopCard;
    [SerializeField]
    private TextMeshProUGUI herosGold;
    private GameObject selectedCard = null;

    public GameObject minionPrefab;


    public void Start()
    {
        UIManager.Instance.currentMinion = Resources.Load("Minions/45") as MinionData;

        GameObject tmp = Instantiate(minionPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        tmp.transform.SetParent(GameObject.Find("DealtMinions").transform);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        tmp.transform.localPosition = new Vector3(0,0,0);

        tmp.AddComponent<ShowShopCard>();
        tmp.GetComponent<ShowShopCard>().rightShopUI = rightShopUI;
        tmp.GetComponent<ShowShopCard>().shop = this.gameObject;

        GameManager.Instance.heroes.Add(new Hero{ 
            MyTurn = true,
            Gold = 4
        });

        GameManager.Instance.heroes.Add(new Hero
        {
            MyTurn = false,
            Gold = 2
        });
    }

    public void UpdateShopCard(GameObject selectedMinionObject)
    {
        selectedCard = selectedMinionObject;

        MinionVisual selectedMinion = selectedMinionObject.GetComponent<MinionVisual>();

        //Update Card
        shopCard.GetComponent<MinionVisual>().cost.text = selectedMinion.cost.text;
        shopCard.GetComponent<MinionVisual>().health.text = selectedMinion.health.text;
        shopCard.GetComponent<MinionVisual>().damage.text = selectedMinion.damage.text;
        shopCard.GetComponent<MinionVisual>().cardBackground.sprite = selectedMinion.cardBackground.sprite;
        shopCard.GetComponent<MinionVisual>().condition.sprite = selectedMinion.condition.sprite;
        shopCard.GetComponent<MinionVisual>().allyClass.sprite = selectedMinion.allyClass.sprite;
        shopCard.GetComponent<MinionVisual>().silenceIcon.sprite = selectedMinion.silenceIcon.sprite;
        shopCard.GetComponent<MinionVisual>().effect1.sprite = selectedMinion.effect1.sprite;
        shopCard.GetComponent<MinionVisual>().effect2.sprite = selectedMinion.effect2.sprite;

        //Update Hero's Current Gold in Shop
        int currentPlayer = GetPlayer();
        herosGold.text = GameManager.Instance.heroes[currentPlayer].Gold.ToString();
    }

    public void BuyCard()
    {
        int costForCard = int.Parse(shopCard.GetComponent<MinionVisual>().cost.text);
        int currentPlayer = GetPlayer();


        //Compare the cost of the Card
        if (GameManager.Instance.heroes[currentPlayer].Gold >= costForCard)
        {
            Debug.Log("Can Buy");
            //Get the Purchased Minion
            MinionVisual minion = shopCard.GetComponent<MinionVisual>() as MinionVisual;
            //Subtract the Hero's current Gold
            GameManager.Instance.heroes[currentPlayer].Gold -= costForCard;
            //Update the Gold UI
            herosGold.text = GameManager.Instance.heroes[currentPlayer].Gold.ToString();
            //Destroy the Object
            Destroy(selectedCard.gameObject);
            selectedCard = null;
            //Close the Right Shop GUI
            rightShopUI.SetActive(false);
        }
        else
        {
            Debug.Log("Cannot buy");
        }
    }

    private int GetPlayer()
    {
        int player = 0;

        if (GameManager.Instance.heroes[0].MyTurn)
        {
            player = 0;
        }
        else
        {
            player = 1;
        }

        return player;
    }

    public void CloseShop()
    {
        rightShopUI.SetActive(false);
    }
}
