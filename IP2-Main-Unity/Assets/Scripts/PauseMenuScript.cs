using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenuScript : MonoBehaviour
{
   
    public GameObject ui;
<<<<<<< Updated upstream
    public float speedBeforePause;
    public GameObject resumeButton;
    //public GameObject[] otherUIStuff; old code because I could't work out why controller wouldn't work on the pause menu
=======
    public GameObject settingsUI;

>>>>>>> Stashed changes

    void Update()
    {
        if(Gamepad.current != null)
        {
            if (Input.GetKeyDown(KeyCode.P) || Gamepad.current.startButton.wasPressedThisFrame) //Uses the settings button on ps controller
            {
                Toggle();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Toggle();
            }
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            /*
            Cursor.lockState = CursorLockMode.Locked; probably not needed anymore
            Cursor.visible = false;
            */
            speedBeforePause = Time.timeScale; //Used in case you're using the speed up thinghy
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
<<<<<<< Updated upstream
        Time.timeScale = 1f;
        SceneManager.LoadScene("Settings Menu");
    }
    private IEnumerator PleaseWork() //Delays the selection of the resume button until the end of the frame so that it actually gets picked now
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }
}
=======
        ui.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsUI.SetActive(false);
        ui.SetActive(true);
    }
    }

>>>>>>> Stashed changes
