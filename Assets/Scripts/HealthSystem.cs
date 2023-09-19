using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler<Transform> OnDead;
    [SerializeField] private int health = 100;

    public void Damage(int damageAmount, Transform damageTransform = null)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            Die(damageTransform);
        }
        Debug.Log(health);
    }

    private void Die(Transform damageTransform = null)
    {
        OnDead?.Invoke(this, damageTransform);
    }
}
