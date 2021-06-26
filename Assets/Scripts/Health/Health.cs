using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    [SerializeField] private PlayerDeath player;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        GameManager.Instance.playerHealth = Mathf.Clamp(GameManager.Instance.playerHealth - damage, 0, GameManager.Instance.playerStartingHealth);
        if (GameManager.Instance.playerHealth > 0)
        {
            player.TakeDamage();
        }
        else
        {
            player.Die();
        }
    }

}
