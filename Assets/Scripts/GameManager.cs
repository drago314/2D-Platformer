using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Variables to be set by editor
    public int playerStartingHealth;

    public string DONT_SET_VARIABLES_BELOW_THIS;

    //Variables to be set by other classes
    public int playerHealth;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //YOU ARE THE CHOSEN ONEEEE
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        playerHealth = playerStartingHealth;
    }

    private void OnGUI()
    {
        if (Application.isEditor)
        {
            GUI.Label(new Rect(1200, 10, 100, 20), "Starting Health: " + playerStartingHealth);
            GUI.Label(new Rect(1200, 20, 100, 20), "Health: " + playerHealth);
        }
    }
}
