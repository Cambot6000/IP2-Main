using UnityEngine;

public class AoeTowerUpgrade : MonoBehaviour
{
    [Header("UI Settings")]
    public string turretName = "Aoe Tower";

    [Header("Upgrade 1: Supercharged Blast")]
    public string option1Label = "Supercharge";
    public int option1Cost = 250;
    public int damageBonus = 25;
    public GameObject superchargedParticlePrefab; 

    [Header("Upgrade 2: Rapid Pulse")]
    public string option2Label = "Rapid Pulse";
    public int option2Cost = 200;
    public float fireRateReduction = 0.25f; // Reduces the interval between shots

    private AoeTowerScript aoeTower;

    void Awake()
    {
        aoeTower = GetComponent<AoeTowerScript>();
    }

    public void ApplyOption1()
    {
        if (MoneyManager.instance == null)
        {
            Debug.LogWarning("No MoneyManager instance found");
            return;
        }

        if (!MoneyManager.instance.SpendGold(option1Cost))
        {
            Debug.Log("Not enough gold for Supercharge");
            return;
        }

        // Increase damage
        aoeTower.damage += damageBonus;

        // Swap the particle prefab used for firing
        if (superchargedParticlePrefab != null)
        {
            aoeTower.aoeParticlePrefab = superchargedParticlePrefab;
        }

        Debug.Log($"{turretName} Supercharged! Damage: {aoeTower.damage}");
    }

    public void ApplyOption2()
    {
        if (MoneyManager.instance == null)
        {
            Debug.LogWarning("No MoneyManager instance found");
            return;
        }

        if (!MoneyManager.instance.SpendGold(option2Cost))
        {
            Debug.Log("Not enough gold for Rapid Pulse");
            return;
        }

        // Increase fire rate by reducing the interval
        aoeTower.fireRate = Mathf.Max(0.1f, aoeTower.fireRate - fireRateReduction);

        Debug.Log($"{turretName} fire rate upgraded! New interval: {aoeTower.fireRate}");
    }
}
