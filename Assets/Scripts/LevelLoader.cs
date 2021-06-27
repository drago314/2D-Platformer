using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public const string FADE = "fade";

    [SerializeField] private static Animator anim;

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
            anim = gameObject.GetComponent<Animator>();
        }
    }

    public void LoadScene(string sceneName, string loadType)
    {
        if (loadType == FADE)
            StartCoroutine(LoadFadeTransition(sceneName, 1));
    }

    IEnumerator LoadFadeTransition(string sceneName, float transitionTime)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
