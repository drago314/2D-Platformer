using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;

    public void PlayGame()
    {
        levelLoader.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
