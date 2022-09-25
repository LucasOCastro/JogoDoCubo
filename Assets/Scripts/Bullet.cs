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

    void FixedUpdate()
    {
        _rb.position += transform.forward * speed;
    }
    
    void OnCollisionEnter(Collision other)
    {   
        if (other.gameObject.TryGetComponent<HealthManager>(out var health)) //Verifica se o objeto possui a Tag "Enemy"
        {
            health.Damage(damage, other.GetContact(0).point, transform.forward * impact);
        }

        Destroy(gameObject);
    }
}
