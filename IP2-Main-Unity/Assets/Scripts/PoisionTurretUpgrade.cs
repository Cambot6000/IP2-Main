using UnityEngine;

public class PoisonTurretUpgrade : MonoBehaviour
{
    [Header("UI Settings")]
    public string turretName = "Poison Tower";

    [Header("Upgrade Option 1")]
    public string option1Label = "Increase Poison Damage";
    public int option1Cost = 100;
    public int damageIncrease = 1;

    [Header("Upgrade Option 2")]
    public string option2Label = "Increase Poison Duration";
    public int option2Cost = 120;
    public int timesIncrease = 1;

    private PoisonousScript poison;

    void Awake()
    {
        poison = GetComponent<PoisonousScript>();
    }

    public void ApplyOption1()
    {
        if (!MoneyManager.instance.SpendGold(option1Cost))
        {
            Debug.Log("Not enough gold");
            return;
        }

        if (poison != null)
        {
            poison.poisonDamage += damageIncrease;
            Debug.Log($"{turretName} poison damage upgraded to {poison.poisonDamage}");
        }
    }

    public void ApplyOption2()
    {
        if (!MoneyManager.instance.SpendGold(option2Cost))
        {
            Debug.Log("Not enough gold");
            return;
        }

        if (poison != null)
        {
            poison.poisonTimes += timesIncrease;
            Debug.Log($"{turretName} poison duration upgraded to {poison.poisonTimes} ticks");
        }
    }
}