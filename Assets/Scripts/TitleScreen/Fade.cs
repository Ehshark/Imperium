using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public bool doFade = true;

    public void Update()
    {
        if (doFade)
        {
            FadeEffect();
        }
    }

    public void FadeEffect()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup.alpha == 1)
        {
            doFade = false;
            StartCoroutine(DoFadeOut());
        }
        else
        {
            doFade = false;
            StartCoroutine(DoFadeIn());
        }
    }

    IEnumerator DoFadeOut ()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 1.25f;
            yield return null;
        }

        doFade = true;
        yield return null;
    }

    IEnumerator DoFadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / 1.25f;
            yield return null;
        }

        doFade = true;
        yield return null;
    }

}