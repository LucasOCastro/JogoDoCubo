using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;
    private void OnTriggerEnter(Collider collision)
    {
        iDamageable damageable = collision.GetComponent<iDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}
