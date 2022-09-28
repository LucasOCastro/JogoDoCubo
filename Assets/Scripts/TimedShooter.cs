using System.Collections;
using UnityEngine;

public abstract class TimedShooter : Shooter
{
    [SerializeField] private float minTimeBetweenShots;
    [SerializeField] private int ammoCount;
    [SerializeField] private float reloadSeconds;

    private bool HandlesReloading => ammoCount > 0 && reloadSeconds > 0;

    protected int RemainingAmmo { get; private set; }
    protected void Reload()
    {
        if (HandlesReloading && !_reloading)
        {
            StartCoroutine(ReloadCoroutine());    
        }
    }

    private bool _cooldown, _reloading;

    private IEnumerator CooldownCoroutine()
    {
        _cooldown = true;
        yield return new WaitForSeconds(minTimeBetweenShots);
        _cooldown = false;
    }

    private IEnumerator ReloadCoroutine()
    {
        _reloading = true;
        yield return new WaitForSeconds(reloadSeconds);
        _reloading = false;
        RemainingAmmo = ammoCount;
    }
    
    protected override void Fire(Vector3 direction)
    {
        if (HandlesReloading && RemainingAmmo == 0)
        {
            Reload();
            return;
        }
        if (_cooldown)
        {
            return;
        }
        base.Fire(direction);
        RemainingAmmo--;
        
        StartCoroutine(CooldownCoroutine());
    }
}