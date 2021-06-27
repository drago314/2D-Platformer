using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealthCallback
{
    private Animator anim;
    private Health health;

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    public void OnHeal()
    {
        throw new System.NotImplementedException();
    }

    public void OnHit()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        health = gameObject.GetComponent<Health>();
        health.SetCallbackListener(this);
        anim = gameObject.GetComponent<Animator>();
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
