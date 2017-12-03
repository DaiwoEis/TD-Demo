using UnityEngine;

public class Tower : MonoBehaviour
{ 
    [HideInInspector]
    public DefenderBase currDefender;

    public bool CanBuy(string defenderName)
    {        
        GameObject targetObject = Resources.Load<GameObject>("Defenders/" + defenderName);    
        int price = targetObject.GetComponent<DefenderStatus>().buyPrice[0];

        return GameController.Instance.GetGold() > price;
    }

    public void BuyDefender(string defenderName)
    {
        GameObject targetObject = Resources.Load<GameObject>("Defenders/" + defenderName);
        int price = targetObject.GetComponent<DefenderStatus>().buyPrice[0];
        currDefender = Instantiate(targetObject, transform.position, Quaternion.identity).GetComponent<DefenderBase>();
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
