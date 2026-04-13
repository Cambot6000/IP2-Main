using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public string sceneToLoad;
    public bool playRocketSound;
    public bool isLoadingNewScene = false;

    public void LoadScene()
    {
        if (isLoadingNewScene)
        {
            return;
        }
        isLoadingNewScene = true;
        if (playRocketSound)
        {
            AudioManager.Instance.PlayRocketLaunch(); 
            AudioManager.Instance.StopMusic();
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

