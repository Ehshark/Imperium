using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCommand : Command
{
    Transform objToAnimate;
    float time;
    public DelayCommand(Transform t, float thyme)
    {
        objToAnimate = t;
        time = thyme;
    }

    public override void StartCommandExecution()
    {
        GameManager.Instance.GetComponent<DelayCreator>().CreateDelay(objToAnimate, this, time);
    }
}

