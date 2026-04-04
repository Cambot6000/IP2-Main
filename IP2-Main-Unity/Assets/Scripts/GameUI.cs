using System.Collections;
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

    public Image waveImage;
    public CanvasGroup waveCanvasGroup;

    public int waveNumber;
    public Sprite[] images;
    public int currentWaveNumber = 0;
    public float fadeSpeed;
    public float targetAlpha;


    public GameObject[] egg3D;

    public bool takingDamage;
    public Building funnyVariableNameHere; //Gets reference to the Building script
    public CameraShake evenFunnierVariableName; //Gets reference to the CameraShake script

    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = healthBar.maxValue;
        egg3D[1].SetActive(false);
        egg3D[2].SetActive(false);
        egg3D[0].SetActive(true);
        if (funnyVariableNameHere == null)
        {
            funnyVariableNameHere = GetComponent<Building>();
        }
        
        if(evenFunnierVariableName == null)
        {
            evenFunnierVariableName = GetComponent<CameraShake>();
        }
    }
    void Update()
    {
        waveNumber = enemiesSpawner.waveNumber;
        if (waveNumber >= 6)
        {
            waveImage.sprite = images[2];
        }
        if (waveNumber >= 3)
        {
            waveImage.sprite = images[1];
        }
        if (currentWaveNumber != waveNumber) // only show when new wave begins
        {
            FadeIn();
            if (waveNumber == 1)
            {
                enemiesSpawner.LandedRocket();
                funnyVariableNameHere.StartCoroutine(funnyVariableNameHere.ControllerRumble(1.0f, 1.0f, 0.3f)); //Starts a controller rumble
                evenFunnierVariableName.StartCoroutine(evenFunnierVariableName.Shakermaker(0.3f,0.4f)); //Starts a screen shake ^ both when the rocket lands
            }

        }
        if (Mathf.Abs(waveCanvasGroup.alpha - targetAlpha) < 0.01f)// if the diffrence is between the values is less than 0.01d then fades out
        {
            FadeOut();
        }


        waveCanvasGroup.alpha = Mathf.Lerp(waveCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);// smooths the fade in and out
        if(waveNumber != 0)
        {
            waveText.text = "Wave " + waveNumber.ToString();// sets text to wave and the number
        }
        else if(waveNumber == 0) 
        {
            waveText.text = "Prep Time";
            funnyVariableNameHere.StartCoroutine(funnyVariableNameHere.ControllerRumble(0.4f, 0.2f, enemiesSpawner.rocketSpeed)); //Starts rumble when rocket is coming down
            evenFunnierVariableName.StartCoroutine(evenFunnierVariableName.Shakermaker(enemiesSpawner.rocketSpeed,0.1f)); //Screeen shakes when the rocket is coming down
        }
        
        healthBar.value = health;// sets health bar value for ui
                                 // check value against max value * value 
                                 // sets sprite to egg array image based on value
        if (health <= healthBar.maxValue * 0.25f)
        {
            eggImage.sprite = egg[0];
            FillBox.color = new Color32(174, 47, 49, 255);
            egg3D[1].SetActive(false);
            egg3D[2].SetActive(true);
            egg3D[0].SetActive(false);
        }
        else if (health <= healthBar.maxValue * 0.50f)
        {
            eggImage.sprite = egg[1];
            FillBox.color = new Color32(255, 153, 86, 255);
            egg3D[1].SetActive(true);
            egg3D[2].SetActive(false);
            egg3D[0].SetActive(false);
        }
        else if (health <= healthBar.maxValue * 0.75f)
        {

            eggImage.sprite = egg[2];
            FillBox.color = new Color32(255, 225, 121, 255);


        }
        if (health <= 0)// ends game

        {
            funnyVariableNameHere.EmergencyStop(); //Stops the controller from infinitely rumbling when you die
            SceneManager.LoadScene("GameOver");
        }

    }



    public void TakeDamage(int damage)// take damage minus it from Health
    {
        StartCoroutine(DamageWait());
        evenFunnierVariableName.StartCoroutine(evenFunnierVariableName.Shakermaker(0.2f, 0.3f));
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

    private IEnumerator DamageWait()
    {
        takingDamage = true;
        yield return new WaitForSeconds(1f);
        takingDamage = false;
        //Used to show the damage taken flash on a PS controller lightbar
    }
}
