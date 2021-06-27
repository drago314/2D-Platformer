using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    private bool isHit;
    private bool isDead;


    private void Awake()
    {
        GameManager.Instance.playerHealth = this;
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
        }
        GameManager.Instance.playerHealth = this;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }

    public int GetStartingHealth()
    {
        return maxHealth;
    }

    public void SetStartingHealth(int health)
    {
        maxHealth = health;
    }

    public bool IsHit()
    {
        return isHit;
    }

    public void SetIsHit(bool isHit)
    {
        this.isHit = isHit;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth > 0)
        {
            isHit = true;
        }
    }

    public void HealDamage(int healing)
    {
        currentHealth = Mathf.Clamp(currentHealth + healing, 0, startingHealth);
    }
}
