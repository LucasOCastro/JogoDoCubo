using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private Transform fireEffectPrefab;
    [SerializeField] private float minTimeBetweenShots;

    private float _timer;
    void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetButton("Fire1") && _timer >= minTimeBetweenShots)
        {
            Fire();
            _timer = 0;
        }
    }
    
    private void Fire()
    {
        Vector3 position = (bulletSpawnPos != null) ? bulletSpawnPos.position : transform.position;
        Vector3 direction = DirectionToMouse();
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        Instantiate(bullet, position, rotation);
        if (fireEffectPrefab != null)
        {
            Instantiate(fireEffectPrefab, position, rotation);    
        }
    }

    private Vector3 DirectionToMouse()
    {
        //Partindo da posição do mouse na tela, calculo o ponto de interseção da linha Camera-Mouse com o plano do player.
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up ,transform.position);
        playerPlane.Raycast(mouseRay, out float planeDistance);
        Vector3 worldMousePos = mouseRay.GetPoint(planeDistance);

        return (worldMousePos - transform.position).normalized;
    }
}