using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SkillTree : MonoBehaviour
{

    public GameObject skillTreePanel;
    public Text tex;
    public void Trash()
    {
        if (skillTreePanel != null)
        {
            tex = skillTreePanel.GetComponentInChildren<Text>();
            Debug.Log(tex);
            tex.text = "PLZ YOLO";
            skillTreePanel.SetActive(true);

        }

    }

    public void confirm ()
    {
        SceneManager.LoadScene("GameBoard");
        Debug.Log("yolo confirm");
    }

    public void cancel()
    {
        SceneManager.LoadScene("GameBoard");
        Debug.Log("yolo cancel");

    }
}
