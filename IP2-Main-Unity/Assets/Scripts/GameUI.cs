using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Slider healthBar;
    public TMP_Text waveText;
    public int health = 100;
    public EnemiesSpawner enemiesSpawner;

    public int waveNumber;

    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value= healthBar.maxValue;

    }

    
    void Update()
    {
        waveNumber= enemiesSpawner.waveNumber;


        waveText.text ="Wave "+ waveNumber.ToString();
        healthBar.value = health;

        if(health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
