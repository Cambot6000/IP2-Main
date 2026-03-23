using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int currentGold = 500;
    public static MoneyManager instance;
    public TextMeshProUGUI goldText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }
    
    void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + currentGold.ToString();
        }
    }
}
