using UnityEngine.EventSystems;

public class UIWindow_GameInit : UIWindow , IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.BackToLastWindow();
    }
}
