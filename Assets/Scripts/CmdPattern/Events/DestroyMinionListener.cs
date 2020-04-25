using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DestroyMinionListener : MonoBehaviour, IListener, IPointerDownHandler
{
    GameObject minion;
    bool inputReceived = false;

    public void OnEvent(EVENT_TYPE Event_Type)
    {
        foreach (Transform obj in transform)
        {
            Image img = obj.GetComponentInChildren<Image>();
            img.color = Color.yellow;
        }

        StartCoroutine(SelectEnemyMinion());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        if (minion.CompareTag("EnemyMinion"))
        {
            inputReceived = true;
            StartCoroutine(DestroyMinionPlayback());
        }
    }

    IEnumerator SelectEnemyMinion()
    {
        DelayCommand dc = new DelayCommand();
        dc.AddToQueue();
        Text text = GameManager.Instance.instructionsObj.gameObject.GetComponent<Text>();
        text.text = "Please select an enemy minion to destroy";
        yield return new WaitUntil(() => inputReceived == true);
        dc.CommandExecutionComplete();
    }

    IEnumerator DestroyMinionPlayback()
    {
        Transform minionZone = GameManager.Instance.enemyMinionZone;
        Image image = minionZone.GetComponent<Image>();
        Color color = image.color;
        while (image.color.a < 1) //use "< 1" when fading in
        {
            color.a += Time.deltaTime / 1; //fades out over 1 second. change to += to fade in
            image.color = color;
            yield return null;
        }
        color.a = .4f;
        image.color = color;

        DestroyMinionCommand dmc = new DestroyMinionCommand(minion);
        Destroy(minion);
        dmc.AddToQueue();
    }
}
