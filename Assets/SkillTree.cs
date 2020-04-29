using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SkillTree : MonoBehaviour
{

    public GameObject skillTreePanel;
    public Text skillTreeText;
    //maybe for confirm? 
    public bool TrashHeroPower = false, PeekHeroPower = false, ExpressedHeroPower = false, RecycleHeroPower = false, HealHeroPower = false, DeathTouchHeroPower = false, UntapHeroPower = false, SilenceHeroPower = false, BuffedAliiedMinionHeroPower = false;
    EventSystem variable;



    public void Trash()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you trash a card)?";
            skillTreePanel.SetActive(true);

        }
        else
        {
            Debug.Log(skillTreePanel);
        }

    }
    public void Peek()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you peek(scry) shop)?";
            skillTreePanel.SetActive(true);

        }

    }
    public void Express()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you do a express buy)?";
            skillTreePanel.SetActive(true);

        }

    }
    public void Recycle()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you recycle a card)?";
            skillTreePanel.SetActive(true);

        }

    }
    public void Heal()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you heal a minion)?";
            skillTreePanel.SetActive(true);

        }

    }
    public void DeathTouch()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you deal deathtouch damage)?";
            skillTreePanel.SetActive(true);

        }

    }
    public void UnTap()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you untap a minion)?";
            skillTreePanel.SetActive(true);

        }

    }

    public void Silence()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you silence an enemy minion)?";
            skillTreePanel.SetActive(true);

        }

    }


    public void BuffAlliedMinion()
    {
        if (skillTreePanel != null)
        {
            skillTreeText = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(skillTreeText);
            skillTreeText.text = "Would You like to acquire the condition (Whenever you buff an allied minion)?";
            skillTreePanel.SetActive(true);

        }

    }


    public void confirm ()
    {
        SceneManager.LoadScene("GameBoard");
        Debug.Log("yolo confirm");
    }


    //maybe a function to keep true and false for 2 items. or no.
}
