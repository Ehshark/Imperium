using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HighlightCard : MonoBehaviour
{


    //MinionVisual mv = gameObject.GetComponent<MinionVisual>();
    //GameObject glowPanel;

    //public Text text;
    //public Hero hero;
    //public int cardCost;
    
    //// Start is called before the first frame update
    //void Start()
    //{
    //    text = GetComponent<Text>();
    //    StartBlinking();
    //}
    //IEnumerator Blink()
    //{
    //    while (true)
    //    {
    //        text.text = "Your Turn!";
    //        yield return new WaitForSeconds(1f);
    //        text.text = "swag";
    //        yield return new WaitForSeconds(1f);
    //    }
        
//            while (false){
//            text.text = "enemy turn!";
//            yield return new waitforseconds(1f);
//}

//}

//void StartBlinking()
//{
//    StartCoroutine(Blink());
//}
//void StopBlinking()
//{
//    StopCoroutine(Blink());
//}



// Update is called once per frame
void start()
    {

        if (GameManager.Instance.ActiveHero().CanPlayCards)
        {

        }

        Debug.Log("yyyyyyyyyyyyyyy");

        //Debug.Log(mv);
        Debug.Log(transform.parent.name.Equals("Hand"));

        CardVisual mv = gameObject.GetComponent<CardVisual>();
        Debug.Log(mv);

        //loop the hand and 
        //if hero gold is same or less than.
        //then activate the panel. highlight cards
        for (int i = 0; i < GameManager.Instance.bottomHero.HandSize; i++)
        {
            // if (GameManager.Instance.ActiveHero().CanPlayCards)
            // {
            //    string mystr = tmp_text.getparsedtext(mv.getcomponentinchildren[i].cost);
            //    int mvcost = system.int32.parse(mystr);
            //    if (gamemanager.instance.bottomhero.currenthealth > mvcost)
            //    {

            //        mv.getcomponentinchildren[i].setactive;
            //    }  

            //}
            Debug.Log("yoo");

        }
       


        // if (GameManager.Instance.ActiveHero())

      //  summonPanel.SetActive(false);
        // mv.cardBackground.color = Color.cyan;
    }
}

