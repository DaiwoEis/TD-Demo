using UnityEngine;

public class Tower : MonoBehaviour
{ 
    [HideInInspector]
    public DefenderBase currDefender;

    public bool CanBuy(string defenderName)
    {        
        var configData = ResourceManager.Load<DefenderConfigData>(defenderName + "Data");    
        int price = configData.BuyPrices[1];

        return GameController.Instance.GetGold() > price;
    }

    public void BuyDefender(string defenderName)
    {
        var configData = ResourceManager.Load<DefenderConfigData>(defenderName + "Data");
        int price = configData.BuyPrices[1];

        GameObject prefab = ResourceManager.Load<GameObject>(defenderName);
        currDefender = Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<DefenderBase>();
        currDefender.transform.SetParent(transform);

        GameController.Instance.ChangeGold(-price);
    }

    public void Sale()
    {
        GameController.Instance.ChangeGold(currDefender.SalePrice());
        currDefender.Sale();
        currDefender = null;
    }

    public bool CanUpgrade()
    {        
        return currDefender.CanUpgrade() && GameController.Instance.GetGold() >= currDefender.UpgradePrice();
    }

    public void Upgrade()
    {         
        GameController.Instance.ChangeGold(-currDefender.BuyPrice(currDefender.Level));
        currDefender.Upgrade();
    }
}
