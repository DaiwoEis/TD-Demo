using UnityEngine;

public class DefenderStatus : MonoBehaviour
{
    public int attackDistance = 30;

    public float attackIntervial = 0.5f;

    public float scanIntervial = 0.1f;

    public int atk = 50;

    public int[] buyPrice = { 250, 350, 450 };

    public int level = 0;

    public int maxLevel = 2;

    public DefenderAnimationName animName;

    public bool IsMaxLevel { get { return level == maxLevel; } }

    public int UpgradePrice { get { return buyPrice[level + 1]; } }

    public float fireTime = 0f;

    public void Upgrade()
    {
        level++;        
        atk += 10;        
        attackIntervial /= 1.5f;      
        attackDistance += 10;
    }

    public int SalePrice()
    {
        int price = 0;

        for (int i = 0; i <= level; i++)
        {
            price += buyPrice[i];
        }
        price /= 2;
        return price;
    }
}
