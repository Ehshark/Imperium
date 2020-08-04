using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawDiscardListener : MonoBehaviour, IPointerDownHandler
{
    private void Start()
    {
        transform.Find("ParticleGlow").gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        GameManager.Instance.DiscardCard(card);

        RemoveListeners();
    }

    public void RemoveListeners()
    {
        foreach(Transform t in GameManager.Instance.GetActiveHand(true))
        {
            DrawDiscardListener ddl = t.GetComponent<DrawDiscardListener>();

            if (ddl)
            {
                Destroy(ddl);
            }

            t.gameObject.AddComponent<PlayCard>();
        }

        GameManager.Instance.instructionsObj.GetComponent<TMP_Text>().text = "";
    }

    private void OnDestroy()
    {
        transform.Find("ParticleGlow").gameObject.SetActive(false);
    }
}
