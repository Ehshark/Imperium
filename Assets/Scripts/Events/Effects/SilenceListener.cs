
using UnityEngine;
using UnityEngine.EventSystems;

public class SilenceListener : MonoBehaviour, IPointerDownHandler
{
    public void Start()
    {
        GameManager.Instance.ChangeCardColour(gameObject, Color.cyan);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject card = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        CardVisual cv = card.GetComponent<CardVisual>();
        Transform minions = GameManager.Instance.GetActiveMinionZone(false);

        cv.ActivateSilence(true);

        foreach (Transform t in minions)
        {
            SilenceListener sl = t.GetComponent<SilenceListener>();
            if (sl)
            {
                Destroy(sl);
            }
        }

        EffectCommand.Instance.inEffect = false;


    }

    public void OnDestroy()
    {
        CardVisual cv = gameObject.GetComponent<CardVisual>();
        if (cv.Md != null)
        {
            GameManager.Instance.ChangeCardColour(gameObject, cv.Md.Color);
        }
        else if (cv.Sd != null)
        {
            GameManager.Instance.ChangeCardColour(gameObject, cv.Sd.Color);
        }
    }
}
