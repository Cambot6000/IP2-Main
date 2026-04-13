using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
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
