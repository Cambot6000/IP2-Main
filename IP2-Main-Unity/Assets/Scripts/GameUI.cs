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

    public Image waveImage;
    public CanvasGroup waveCanvasGroup;
    public Sprite[] egg; 

    public int waveNumber;
    public Sprite[] images;
    public int currentWaveNumber= 0;
    public float fadeSpeed;
    public float targetAlpha;
   

    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value= healthBar.maxValue;

    }


    void Update()
    {
        waveNumber = enemiesSpawner.waveNumber;
        if (waveNumber >= 6)
        {
            waveImage.sprite = images[2];
        }
        if (waveNumber>= 3)
        {
            waveImage.sprite = images[1];
        }
        if (currentWaveNumber != waveNumber) // only show when new wave begins
        {
            FadeIn();

        }
        if (Mathf.Abs(waveCanvasGroup.alpha - targetAlpha)< 0.01f)// if the diffrence is between the values is less than 0.01d then fades out
        {
            FadeOut();
        } 


        waveText.text ="Wave "+ waveNumber.ToString();
        healthBar.value = health;

        if(health <= 0)

        waveCanvasGroup.alpha = Mathf.Lerp(waveCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);// smooths the fade in and out
        waveText.text ="Wave "+ waveNumber.ToString();// sets text to wave and the number
        healthBar.value = health;// sets health bar value for ui
        // check value against max value * value 
        // sets sprite to egg array image based on value
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
        if(health <= 0)// ends game
>>>>>>> Stashed changes
        {
            SceneManager.LoadScene("GameOver");
        }
       
    }


    public void TakeDamage(int damage)// take damage minus it from Health
    {
        health -= damage;
    }

    public void FadeIn()// fade in 
    {
        targetAlpha = 1;
        
    }

   public void FadeOut()// fade out
    {
        targetAlpha = 0;
        currentWaveNumber = waveNumber;
    }
}
