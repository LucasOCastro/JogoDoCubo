using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    private Animator _animator;
    private TimedShooter _shooter;
    private static readonly int Cooldown = Animator.StringToHash("Cooldown");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _shooter = GetComponent<TimedShooter>();
        _shooter.OnReloadStart += () => _animator.SetBool(Cooldown, true);
        _shooter.OnReloadEnd += () => _animator.SetBool(Cooldown, false);
    }
}