using UnityEngine.UI;

public class UIBuyDefender : UIDefender 
{
    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_BuyMachineGunDefender").onClick.AddListener(() => OnBuy("DefenderMachineGun"));
        transform.FindChildComponentByName<Button>("Btn_BuyRocketDefender").onClick.AddListener(() => OnBuy("DefenderRocket"));
    }

    public void OnBuy(string defenderName)
    {
        if (currentSelectedTower.CanBuy(defenderName))
        {
            currentSelectedTower.BuyDefender(defenderName);
            Hide();
        }
    }
}
