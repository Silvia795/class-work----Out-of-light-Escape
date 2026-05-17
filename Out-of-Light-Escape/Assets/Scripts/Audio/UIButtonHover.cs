using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler
{
    bool hasHovered;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UISoundManager.instance != null)
        {
            UISoundManager.instance.PlayHover();
        }
    }
}