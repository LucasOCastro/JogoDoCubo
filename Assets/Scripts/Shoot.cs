using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private Transform fireEffectPrefab;
    [SerializeField] private float minTimeBetweenShots;

    private float _timer;
    void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && _timer >= minTimeBetweenShots)
        {
            Fire();
            _timer = 0;
        }
    }
    
    private void Fire()
    {
        Vector3 position = (bulletSpawnPos != null) ? bulletSpawnPos.position : transform.position;
        Vector3 direction = CameraUtility.DirectionToMouse(position);
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        Instantiate(bullet, position, rotation);
        if (fireEffectPrefab != null)
        {
            Instantiate(fireEffectPrefab, position, rotation);    
        }
    }
}