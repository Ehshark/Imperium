using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GearHoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform textBack;

    public void OnPointerEnter(PointerEventData eventData)
    {
        textBack.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textBack.gameObject.SetActive(false);
    }
}
