using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void TakeDamage()
    {
       anim.SetTrigger("Damage");
    }

    public void Die()
    {
        anim.SetTrigger("Die");
    }
}
