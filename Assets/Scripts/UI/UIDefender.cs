using UnityEngine;

public class UIDefender : MonoBehaviour
{

    [HideInInspector]
    public Tower currentSelectedTower;

    public virtual void Start()
    {
        Hide();
    }

    public void Show(Tower tower)
    {
        currentSelectedTower = tower;
        gameObject.SetActive(true);

        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, tower.transform.position);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    } 
}
