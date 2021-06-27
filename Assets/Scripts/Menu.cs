using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        LevelLoader.Instance.LoadScene("Level 1", LevelLoader.FADE);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
