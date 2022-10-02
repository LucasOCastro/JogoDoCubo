using System;
using System.Collections;
using UnityEngine;

public abstract class TimedShooter : Shooter
{
    [SerializeField] private float minTimeBetweenShots;
    [SerializeField] private int ammoCount;
    [SerializeField] private float reloadSeconds;

    public Action OnReloadStart, OnReloadEnd;

    private bool HandlesReloading => ammoCount > 0 && reloadSeconds > 0;

    public int MaxAmmo => ammoCount;
    public int RemainingAmmo => MaxAmmo - _shotsFired;

    private int _shotsFired;
    protected void Reload()
    {
        if (HandlesReloading && !Reloading)
        {
            StartCoroutine(ReloadCoroutine());    
        }
    }


    public bool Reloading { get; private set; }

    private bool _cooldown;

    private IEnumerator CooldownCoroutine()
    {
        _cooldown = true;
        yield return new WaitForSeconds(minTimeBetweenShots);
        _cooldown = false;
    }

    private IEnumerator ReloadCoroutine()
    {
        Reloading = true;
        OnReloadStart?.Invoke();
        yield return new WaitForSeconds(reloadSeconds);
        Reloading = false;
        _shotsFired = 0;
        OnReloadEnd?.Invoke();
    }
    
    protected override void Fire(Vector3 direction)
    {
        if (HandlesReloading && RemainingAmmo == 0)
        {
            Reload();
            return;
        }
        if (_cooldown || Reloading)
        {
            return;
        }
        base.Fire(direction);
        _shotsFired++;
        
        StartCoroutine(CooldownCoroutine());
    }
}