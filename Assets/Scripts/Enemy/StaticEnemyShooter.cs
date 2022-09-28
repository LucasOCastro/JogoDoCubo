using Unity.Mathematics;
using UnityEngine;

public class StaticEnemyShooter : TimedShooter
{
    [SerializeField] private Transform rotateTransform;
    [SerializeField] private float rotateSpeed;

    private Vector3 TargetDirection => (Player.Instance.transform.position - BulletOrigin.position).normalized;

    void RotateTowards(Vector3 dir)
    {
        if (rotateSpeed == 0)
        {
            rotateTransform.forward = dir;
            return;
        }

        Quaternion target = quaternion.LookRotation(dir, Vector3.up);
        rotateTransform.rotation = Quaternion.RotateTowards(rotateTransform.rotation, target, rotateSpeed * Time.deltaTime);
    }
    
    private void Update()
    {
        if (Player.Instance == null) return;
        
        RotateTowards(TargetDirection);
        Fire(BulletOrigin.forward);
    }
}
