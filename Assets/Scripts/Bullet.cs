using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed=10;
    [SerializeField] private float impact = 10;
    [SerializeField] private int damage;

    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        _rb.velocity = transform.forward * speed;
    }

    public void CollideWith(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent<HealthManager>(out var health)) //Verifica se o objeto possui a Tag "Enemy"
        {
            health.Damage(damage, transform.position, transform.forward * impact);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {   
        CollideWith(other);
    }
}
