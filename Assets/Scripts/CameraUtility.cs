
using UnityEngine;

public static class CameraUtility
{
    //Algumas coisas seriam bom de fazer cache
    
    public static Vector3 RotateFlatVector(Vector3 vec)
    {
        vec = Camera.main.transform.TransformDirection(vec);
        vec = new Vector3(vec.x, 0, vec.z).normalized;
        return vec;
    }

    public static Vector3 DirectionToMouse(Vector3 from)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane playerPlane = new Plane(Vector3.up ,from);
        playerPlane.Raycast(mouseRay, out float planeDistance);
        Vector3 worldMousePos = mouseRay.GetPoint(planeDistance);

        return (worldMousePos - from).normalized;

    }
}
