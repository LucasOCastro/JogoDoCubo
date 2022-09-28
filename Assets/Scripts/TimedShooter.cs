using System;
using System.Collections;
using UnityEngine;

public abstract class TimedShooter : Shooter
{
    [SerializeField] private float minTimeBetweenShots;
    [SerializeField] private int ammoCount;
    [SerializeField] private float reloadSeconds;

    public Action OnReload;

    private bool HandlesReloading => ammoCount > 0 && reloadSeconds > 0;

    public int MaxAmmo => ammoCount;
    public int RemainingAmmo => MaxAmmo - _shotsFired;

    private int _shotsFired;
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
        _shotsFired = 0;
        OnReload?.Invoke();
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
        _shotsFired++;
        
        StartCoroutine(CooldownCoroutine());
    }
}