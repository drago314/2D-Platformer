using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    private Animator anim;
    private Health health;

    private void Awake()
    {
        health = gameObject.GetComponent<Health>();
        anim = gameObject.GetComponent<Animator>();
        health.SetStartingHealth(startingHealth);
        health.SetCurrentHealth(startingHealth);
    }

    private void Update()
    {
        if (health.IsDead())
        {
            anim.SetTrigger("Die");
        }
        else if (health.IsHit())
        {
            anim.SetTrigger("Damage");
            health.SetIsHit(false);
        }
    }
}
