using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnDamage : MonoBehaviour, iDamageable
{
    public bool IsDead { get; private set; }

    public event Action DeathEvent;

    public void Awake()
    {
        IsDead = false;
    }

    public void TakeDamage(int damage)
    {
        IsDead = true;
        DeathEvent.Invoke();
    }
}
