using System;
using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private Transform effectSpawnPos;
    [SerializeField] private Transform fireEffectPrefab;
    [SerializeField] private LayerMask clipBlockMask;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] public AudioClip audioClip;

    public Action OnShoot;
    
    protected Transform BulletOrigin => (bulletSpawnPos != null) ? bulletSpawnPos : transform;

    protected virtual void Fire(Vector3 direction)
    {
        OnShoot?.Invoke();
        
        Vector3 position = BulletOrigin.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        var bulletInstance = Instantiate(bullet, position, rotation);
        
        
        if (fireEffectPrefab != null)
        {
            Vector3 effectPos = (effectSpawnPos != null) ? effectSpawnPos.position : position;
            Instantiate(fireEffectPrefab, effectPos, rotation);    
        }
        
        if (audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }

        if (Physics.Linecast(transform.position, position, out var hitInfo, clipBlockMask))
        {
            bulletInstance.CollideWith(hitInfo.collider);
        }
    }
}