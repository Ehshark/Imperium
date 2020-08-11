using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipController : MonoBehaviour
{
    [SerializeField]
    private GameObject inputField;
    [SerializeField]
    private GameObject skipMenu;

    [SerializeField]
    private GameObject userName;
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (Music.Instance.skipToMain)
        {
            inputField.SetActive(true);
            skipMenu.SetActive(true);
            userName.SetActive(false);
            mainMenu.SetActive(false);
        }
        else
        {
            inputField.SetActive(false);
            skipMenu.SetActive(false);
            userName.SetActive(true);
            mainMenu.SetActive(true);
        }
    }

    public void OptionsBackClicked()
    {
        optionsMenu.SetActive(false);

        if (Music.Instance.skipToMain)
        {
            skipMenu.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);
        }
    }
}
