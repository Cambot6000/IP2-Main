using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public string sceneToLoad;
    public bool playRocketSound;  

    public void LoadScene()
    {
        if (playRocketSound)
        {
            AudioManager.Instance.PlayRocketLaunch(); 
        }
        TransitionManager.Instance.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        if (TransitionManager.Instance == null)
        {
            Application.Quit();
            return;
        }

        TransitionManager.Instance.QuitGame();
    }
}

