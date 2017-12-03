public class UISettingDefender : UIDefender
{  
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
