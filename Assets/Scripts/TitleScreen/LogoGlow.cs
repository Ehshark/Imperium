using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoGlow : MonoBehaviour
{
    public ParticleSystem logoGlow;
    public GameObject panel;

    public void EnableGlow()
    {
        logoGlow.gameObject.SetActive(true);
        panel.SetActive(true);
    }
}
