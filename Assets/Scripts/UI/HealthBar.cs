using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthManager healthManager;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        healthManager.OnDamaged += UpdateVal;
    }

    private void UpdateVal()
    {
        if (_slider == null) return;
        float curHealth = healthManager.CurrentHealth;
        float maxHealth = healthManager.MaxHealth;
        _slider.value = curHealth / maxHealth;
    }

    private void OnEnable() => UpdateVal();
    private void Start() => UpdateVal();
}