using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawDiscardListener : MonoBehaviour, IPointerDownHandler
{
    const byte DRAW_DISCARD_SYNC_EVENT = 50;

    private void Start()
    {
        transform.Find("ParticleGlow").gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Multiplayer 
        int position = gameObject.transform.GetSiblingIndex();
        object[] data = new object[] { position };
        EffectCommandPun.Instance.SendData(DRAW_DISCARD_SYNC_EVENT, data);

        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        GameManager.Instance.DiscardCard(card);

        RemoveListeners();
    }

    public void RemoveListeners()
    {
        foreach (Transform t in GameManager.Instance.GetActiveHand(true))
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
