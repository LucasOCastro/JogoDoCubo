using System.Collections;
using UnityEngine;

public class ArcProjectile : MonoBehaviour
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float speed;
    [SerializeField] private FloatRange explodeTimeUponLand;
    [Header("Damage")] 
    [SerializeField] private int damage;
    [SerializeField] private float impact;
    [SerializeField] private float damageRadius;
    [SerializeField] private LayerMask damageMask;
    [Header("Effects")]
    [SerializeField] private ParticleSystem explodeEffect;
    [SerializeField] private AudioClip explodeAudio;

    private Vector3 _startPos, _target, _dir;
    private float _startDistance;
    private float _progress;

    private void SetTarget(Vector3 target)
    {
        _target = target;
        _startPos = transform.position;

        Vector3 offset = _target - _startPos;
        _startDistance = offset.magnitude;
        _dir = offset / _startDistance;
    }
    
    private void Update()
    {
        if (_progress >= 1)
        {
            return;
        }
        
        float progressDelta = speed * Time.deltaTime / _startDistance;
        _progress = Mathf.Clamp01(_progress + progressDelta);
        float height = Mathf.PingPong(_progress, .5f) * maxHeight;
        
        transform.position = _startPos + (_dir * _startDistance * _progress) + (Vector3.up * height);

        if (_progress >= 1)
        {
            StartCoroutine(OnHitTargetCoroutine());
        }
    }

    private IEnumerator OnHitTargetCoroutine()
    {
        float time = explodeTimeUponLand.Random;
        yield return new WaitForSeconds(time);
        Vector3 pos = transform.position;

        if (explodeEffect != null)
        {
            Instantiate(explodeEffect, pos, Quaternion.identity);    
        }

        if (explodeAudio != null && TryGetComponent<AudioSource>(out var audioSource))
        {
            audioSource.PlayOneShot(explodeAudio);
        }
        

        var collisions = Physics.OverlapSphere(pos, damageRadius, damageMask);
        foreach (var col in collisions)
        {
            HealthManager health = col.GetComponent<HealthManager>();
            if (!health) continue;
            Vector3 colPos = col.transform.position;
            Vector3 impactForce = (colPos - pos).normalized * this.impact;
            health.Damage(damage, colPos, impactForce);
        }
        
        Destroy(gameObject);
    }

    public ArcProjectile MakeInstance(Vector3 from, Vector3 to)
    {
        var instance = Instantiate(this, from, Quaternion.identity);
        instance.transform.position = from;
        instance.SetTarget(to);
        return instance;
    }
}