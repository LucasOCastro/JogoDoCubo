using UnityEngine;

public class PursuePlayerBehavior : MovementBehavior
{
    private Vector3 DirToPlayer => (Player.Instance.transform.position - transform.position).normalized;
    protected override Vector3 GetMoveDirection() => DirToPlayer;
    protected override Vector3 GetLookDirection() => DirToPlayer;
}