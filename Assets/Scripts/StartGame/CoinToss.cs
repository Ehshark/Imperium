using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinToss : MonoBehaviour
{
    [SerializeField]
    private GameObject StartGameController;
    public GameObject heads;
    public GameObject tails;

    public void HeadsOrTails ()
    {
        //Reset 
        heads.SetActive(false);
        tails.SetActive(false);

        int result = StartGameController.GetComponent<StartGameController>().GetCoinValue();

        if (result == 0)
        {
            tails.SetActive(true);  
        }
        else
        {
            heads.SetActive(true);
        }
    }
}
