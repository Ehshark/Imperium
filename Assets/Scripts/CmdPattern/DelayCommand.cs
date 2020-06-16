using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayCommand : Command
{
    Transform objToAnimate;
    public DelayCommand(Transform t)
    {
        objToAnimate = t;
    }

    public override void StartCommandExecution()
    {
        Debug.Log("DelayCommand Started");

        GameManager.Instance.GetComponent<DelayCreator>().CreateDelay(objToAnimate, this);
    }
}

