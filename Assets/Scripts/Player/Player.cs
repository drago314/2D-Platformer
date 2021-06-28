using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealthCallback
{
    private Animator anim;
    private Health health;
    private Rigidbody2D body;

    private void Awake()
    {
        health = gameObject.GetComponent<Health>();
        health.SetCallbackListener(this);
        anim = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
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

    public void OnSpawn(string currentScene, string newScene)
    {
        body.position = Vector2.zero;
    }
}
