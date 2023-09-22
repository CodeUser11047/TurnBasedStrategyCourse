using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler<Transform> OnDead;
    public event EventHandler OnDamaged;
    [SerializeField] private int health;
    [SerializeField] private int helathMax = 100;

    private void Awake()
    {
        health = helathMax;
    }
    public void Damage(int damageAmount, Transform damageTransform = null)
    {
        health -= damageAmount;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            Die(damageTransform);
        }
        // Debug.Log(health);
    }

    private void Die(Transform damageTransform = null)
    {
        OnDead?.Invoke(this, damageTransform);
    }

    public float GetHealthNormalized()
    {
        return (float)health / helathMax;
    }

    public int GetHealth()
    {
        return health;
    }
}
