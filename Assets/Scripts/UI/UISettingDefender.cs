using UnityEngine.UI;

public class UISettingDefender : UIDefender
{
    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_Sale").onClick.AddListener(OnClickSale);
        transform.FindChildComponentByName<Button>("Btn_Up").onClick.AddListener(OnClickUp);
    }

    public void OnClickSale()
    {
        currentSelectedTower.Sale();
        Hide();
    }
     
    public void OnClickUp()
    {

        if (currentSelectedTower.CanUpgrade())
        {
            currentSelectedTower.Upgrade();
            Hide();
        }
    }
}
