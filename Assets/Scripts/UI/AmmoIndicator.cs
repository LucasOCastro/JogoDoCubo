using UnityEngine;
using UnityEngine.UI;

public class AmmoIndicator : MonoBehaviour
{
    [SerializeField] private TimedShooter shooter;

    private Image _image;

    private void Awake()
    {
        if (shooter.MaxAmmo == 0)
        {
            Destroy(gameObject);
            return;
        }
        _image = GetComponent<Image>();
        shooter.OnShoot += UpdateVal;
        shooter.OnReload += UpdateVal;
    }

    private void UpdateVal()
    {
        if (_image == null) return;
        int curAmmo = shooter.RemainingAmmo;
        int maxAmmo = shooter.MaxAmmo;
        _image.fillAmount = (float) curAmmo / maxAmmo;
    }
    
    private void OnEnable() => UpdateVal();
    private void Start() => UpdateVal();
}