using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        totalHealthBar.fillAmount = GameManager.Instance.playerStartingHealth / 5f;
    }

    private void Update()
    {
        currentHealthBar.fillAmount = GameManager.Instance.playerHealth / 5f;   
    }
}
