using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float transitionTime;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneTransition(sceneName));
    }

    IEnumerator LoadSceneTransition(string sceneName)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
