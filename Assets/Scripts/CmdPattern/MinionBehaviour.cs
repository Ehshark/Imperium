using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinionBehaviour : MonoBehaviour, IPointerDownHandler
{
    GameObject minion;

    public void OnPointerDown(PointerEventData eventData)
    {
        minion = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        if (minion.transform.parent.name != "AlliedMinionsPanel")
            StartCoroutine(PlayMinion());
    }

    IEnumerator PlayMinion()
    {
        Transform minionZone = GameManager.alliedMinionZone;
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

        PlayMinionCommand playMinionCmd = new PlayMinionCommand(minion);
        playMinionCmd.AddToQueue();
    }
}
