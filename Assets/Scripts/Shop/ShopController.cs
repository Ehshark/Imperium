using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private GameObject rightShopUI;
    [SerializeField]
    private TextMeshProUGUI goldCost;

    public GameObject minionPrefab;

    public void Start()
    {
        MinionData minion = Resources.Load("Minions/1") as MinionData;
        UIManager.Instance.currentMinion = minion;

        GameObject tmp = Instantiate(minionPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        tmp.transform.SetParent(GameObject.Find("DealtMinions").transform);
        tmp.transform.localScale = new Vector3(1f, 1f, 1f);
        tmp.transform.localPosition = new Vector3(0,0,0);

        tmp.AddComponent<ShowShopCard>();
        tmp.GetComponent<ShowShopCard>().rightShopUI = rightShopUI;
    }

    public void UpdateShop(MinionVisual selectedMinion)
    {
        goldCost.text = selectedMinion.cost.ToString();
    }

    public void CloseShop()
    {
        rightShopUI.SetActive(false);
    }
}
