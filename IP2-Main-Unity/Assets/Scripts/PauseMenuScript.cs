using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenuScript : MonoBehaviour
{
   
    public GameObject ui;
    public float speedBeforePause = 1f;
    public GameObject resumeButton;
    //public GameObject[] otherUIStuff; old code because I could't work out why controller wouldn't work on the pause menu

    public GameObject settingsUI;
    public bool wzrd;

    void Update()
    {
        
        if(Gamepad.current != null)
        {
            if ((Input.GetKeyDown(KeyCode.P) || Gamepad.current.startButton.wasPressedThisFrame) && wzrd) //Uses the settings button on ps controller
            {
                wzrd = false;
                Toggle();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P) && wzrd)
            {
                wzrd = false;
                Toggle();
            }
        }
    }

    public void Toggle()
    {
        wzrd = true;
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            /*
            Cursor.lockState = CursorLockMode.Locked; probably not needed anymore
            Cursor.visible = false;
            */
            if(Time.timeScale > 0)
            {
                speedBeforePause = Time.timeScale; //Used in case you're using the speed up thinghy
            }
            
            Time.timeScale = 0f;
            EventSystem.current.SetSelectedGameObject(null);
            /*
            foreach (GameObject sb2h in otherUIStuff)
            {
                if (sb2h != null)
                {
                    sb2h.SetActive(false);
                }
            }
            */
            //Above lines in commentary used to turn off all other UI, not needed anymore because I got it working properly
            StartCoroutine(PleaseWork());
        }
        else
        {
            /*
            foreach (GameObject sb2h in otherUIStuff)
            {
                if (sb2h != null)
                {
                    sb2h.SetActive(true);
                }
            }
            */
            //Above lines in commentary used to turn on all other UI, not needed anymore because I got it working properly
            Time.timeScale = speedBeforePause;
        }
    }
   

    public void Retry()
    {
        Toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

   
    public void OpenSettings()
    {

        settingsUI.SetActive(true);
        ui.SetActive(false);
    }
    private IEnumerator PleaseWork() 
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }
    public void CloseSettings()
    {
        settingsUI.SetActive(false);
        ui.SetActive(true);
    }
}


