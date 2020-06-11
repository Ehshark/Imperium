using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnlargedCardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Transform cardComponent;

    private void Start()
    {
        foreach (Transform t in transform)
        {
            if (t.GetComponent<Image>() != null)
            {
                if (t.name.Equals("CardPanel") || t.name.Equals("BorderPanel"))
                {
                    t.GetComponent<Image>().raycastTarget = false;
                }
                else
                {
                    t.GetComponent<Image>().raycastTarget = true;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardComponent = eventData.pointerCurrentRaycast.gameObject.transform.parent;
        Debug.Log(cardComponent.name);
        if (cardComponent.name.Equals("Cost") || cardComponent.name.Equals("Health") || cardComponent.name.Equals("Damage") ||
            cardComponent.name.Equals("Ally") || cardComponent.name.Equals("Condition") || cardComponent.name.Equals("Effect1Panel") ||
            cardComponent.name.Equals("Effect2Panel"))
        {
            foreach (Transform t in cardComponent)
            {
                if (t.name.Equals("TextBack") || t.name.Equals("Description"))
                {
                    t.gameObject.SetActive(true);
                }
                else if (t.name.Equals("Effect1") || t.name.Equals("Effect2"))
                {
                    foreach (Transform child in t)
                    {
                        if (child.name.Equals("TextBack") || child.name.Equals("Description"))
                        {
                            child.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Transform t in cardComponent)
        {
            if (t.name.Equals("TextBack") || t.name.Equals("Description"))
            {
                t.gameObject.SetActive(false);
            }
            else if (t.name.Equals("Effect1") || t.name.Equals("Effect2"))
            {
                foreach (Transform child in t)
                {
                    if (child.name.Equals("TextBack") || child.name.Equals("Description"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
