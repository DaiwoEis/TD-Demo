using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIWidget : MonoBehaviour, IPointerClickHandler
{
    public event Action<UIWidget, PointerEventData> OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
            OnClick(this, eventData);
    }
}
