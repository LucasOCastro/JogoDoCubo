using System.Threading.Tasks;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private Transform effectSpawnPos;
    [SerializeField] private Transform fireEffectPrefab;
    [SerializeField] private float minTimeBetweenShots;
    [SerializeField] private float ammoCount;
    [SerializeField] private float reloadSeconds;
    [SerializeField] private LayerMask clipBlockMask;
    public AudioSource source;
    public AudioClip clip;

    private float _timer, _reloadTimer;
    private float _shotsFired;
    private void Update()
    {
        _timer += Time.deltaTime;

        if (_shotsFired >= ammoCount)
        {
            if (_reloadTimer <= reloadSeconds)
            {
                _reloadTimer += Time.deltaTime;
                return;
            }
            _reloadTimer = 0;
            _shotsFired = 0;
        }
        
        if (Input.GetButton("Fire1") && _timer >= minTimeBetweenShots)
        {
            Fire();
            _timer = 0;
            _reloadTimer = 0;
            _shotsFired++;
        }
    }

    private void Fire()
    {
        Vector3 position = (bulletSpawnPos != null) ? bulletSpawnPos.position : transform.position;
        Vector3 direction = CameraUtility.DirectionToMouse(position);
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        source.PlayOneShot(clip);
        var bulletInstance = Instantiate(bullet, position, rotation);
        if (fireEffectPrefab != null)
        {
            Vector3 effectPos = (effectSpawnPos != null) ? effectSpawnPos.position : position;
            Instantiate(fireEffectPrefab, effectPos, rotation);    
        }

        if (Physics.Linecast(transform.position, position, out RaycastHit hitInfo, clipBlockMask))
        {
            bulletInstance.CollideWith(hitInfo.collider);
        }
    }
}