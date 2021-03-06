﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayCreator : MonoBehaviour
{
    IEnumerator CreateDelayCoroutine(Transform t, DelayCommand dc, float time)
    {
        Image image = t.GetComponent<Image>();
        if (!image)
            image = t.GetComponentInChildren<Image>();
        //Store the color of the image
        Color color = image.color;

        //Store the color's original alpha value
        float originalAlpha = color.a;

        //Change the alpha to 0
        color.a = 0;

        image.color = color;

        while (image.color.a < 1) //use "< 1" when fading in
        {
            color.a += Time.deltaTime / time; //fades out over 1 second. change to += to fade in
            image.color = color;
            yield return null;
        }
        //Set the alpha back to it's original value
        color.a = originalAlpha;
        image.color = color;

        dc.CommandExecutionComplete();
    }

    public void CreateDelay(Transform t, DelayCommand dc, float time)
    {
        StartCoroutine(CreateDelayCoroutine(t, dc, time));
    }
}
