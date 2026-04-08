using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyPickerButtons : MonoBehaviour
{
    public void PickDifficulty(int i)
    {
        if (i == 0)
        {


            GameSettings.chosenDifficulty = GameSettings.Difficulty.Easy;
        }
        else if (i == 1)
        {
            GameSettings.chosenDifficulty = GameSettings.Difficulty.Medium;
            
        }
        else if (i == 2)
        {
            GameSettings.chosenDifficulty = GameSettings.Difficulty.Hard;
        }
        SceneManager.LoadScene("newPath");
    }
   
}
