using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Slider healthBar;
    public Image FillBox;
    public Image eggImage;
    public TMP_Text waveText;
    public int health = 100;
    public EnemiesSpawner enemiesSpawner;

    public Sprite[] egg; 
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
        if (health <= healthBar.maxValue * 0.25f)
        {
            eggImage.sprite = egg[0];
            FillBox.color = new Color32(174, 47, 49,255);
        }
        else if (health <= healthBar.maxValue * 0.50f)
        {
            eggImage.sprite = egg[1];
            FillBox.color = new Color32(255, 153, 86, 255);
        }
        else if (health <= healthBar.maxValue * 0.75f)
        {
            
                eggImage.sprite = egg[2];
            FillBox.color = new Color32(255, 225, 121,255);


        }
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
