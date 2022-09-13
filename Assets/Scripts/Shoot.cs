using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }
    
    private void Fire()
    {
        Vector3 direction = DirectionToMouse();
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        Instantiate(bullet, transform.position, rotation);
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