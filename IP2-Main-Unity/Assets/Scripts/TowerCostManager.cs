using UnityEngine;

public class TowerCostManager : MonoBehaviour
{
    public int towerCost = 250;

    void Start()
    {
        if (MoneyManager.instance != null)
        {
            bool success = MoneyManager.instance.SpendGold(towerCost);

            if(!success)
            {
                Debug.LogWarning("not enough gold, tower cant be placed");
            }
        }
    }
}
