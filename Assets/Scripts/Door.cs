using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string loadType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LevelLoader.Instance.LoadScene(sceneName, loadType);
        }
    }
}
