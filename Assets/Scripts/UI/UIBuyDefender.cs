public class UIBuyDefender : UIDefender 
{        
    public void OnBuy(string defenderName)
    {
        if (currentSelectedTower.CanBuy(defenderName))
        {
            currentSelectedTower.BuyDefender(defenderName);
            Hide();
        }
    }
}
