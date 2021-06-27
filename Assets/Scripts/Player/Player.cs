using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealthCallback
{
    private Animator anim;
    private Health health;

    private void Awake()
    {
        health = gameObject.GetComponent<Health>();
        health.SetCallbackListener(this);
        anim = gameObject.GetComponent<Animator>();
    }

    public void OnDeath()
    {
        anim.SetTrigger("Die");
    }

    public void OnHeal()
    {
    }

    public void OnHit()
    {
        anim.SetTrigger("Damage");
    }

    public void OnHealthChanged(int currentHealth, int MaxHealth)
    {
    }
}
