using UnityEngine;

public class PlayerShooter : TimedShooter
{
    [SerializeField] private Transform weapon;
    [SerializeField] private float noRotationDistance;

    private bool TooClose => (CameraUtility.MouseWorldPos(transform.position.y) - transform.position).sqrMagnitude < noRotationDistance * noRotationDistance;
    private void Update()
    {
        bool tooClose = TooClose;
        
        Vector3 weaponForward = tooClose ? weapon.forward : CameraUtility.DirectionToMouse(weapon.position).Flat().normalized;
        weaponForward = weaponForward.Flat().normalized;
        weapon.forward = weaponForward;

        
        if (Input.GetButton("Fire1"))
        {
            Vector3 dir = tooClose ? BulletOrigin.forward : CameraUtility.DirectionToMouse(BulletOrigin.position);
            dir = dir.Flat().normalized;
            Fire(dir);
        }
        else if (Input.GetButtonDown("Reload"))
        {
            Reload();
        }
    }
}