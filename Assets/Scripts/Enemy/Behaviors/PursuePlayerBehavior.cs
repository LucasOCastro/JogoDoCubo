using UnityEngine;

public class PursuePlayerBehavior : MovementBehavior
{
    private Vector3 DirToPlayer => (player.position - transform.position).normalized;
    protected override Vector3 GetMoveDirection() => DirToPlayer;
    protected override Vector3 GetLookDirection() => DirToPlayer;
}