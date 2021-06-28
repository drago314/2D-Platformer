using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public const string FADE = "fade";

    [SerializeField] private static Animator anim;
    [SerializeField] private Player player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            anim = GetComponentInChildren<Animator>();
        }
    }

    public void LoadScene(string sceneName, string loadType)
    {
        if (loadType == FADE)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            StartCoroutine(LoadFadeTransition(currentScene, sceneName, 1));
        }
    }

    IEnumerator LoadFadeTransition(string currentScene, string newScene, float transitionTime)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(newScene);
        player.OnSpawn(currentScene, newScene);
        anim.SetTrigger("End");
    }
}
