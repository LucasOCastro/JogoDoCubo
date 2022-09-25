
using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject ragdollPrefab;

    public System.Action OnDeath;

    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void Damage(int damage) => Damage(damage, Vector3.zero, Vector3.zero);
    public void Damage(int damage, Vector3 impactPoint, Vector3 impactForce)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth + 1);
        if (CurrentHealth == 0)
        {
            Die(impactPoint, impactForce);
        }
    }

    private void Die(Vector3 impactPoint, Vector3 impactForce)
    {
        if (ragdollPrefab != null)
        {
            SpawnRagdoll(impactPoint, impactForce);
        }
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
    
    private void SpawnRagdoll(Vector3 impactPoint, Vector3 impactForce)
    {
        var instance = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        if (impactForce != Vector3.zero)
        {
            var rb = instance.GetComponentInChildren<Rigidbody>();
            rb.AddForceAtPosition(impactForce, impactPoint, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
