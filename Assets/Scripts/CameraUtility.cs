
using UnityEngine;

public static class CameraUtility
{
    //Algumas coisas seriam bom de fazer cache

    public static Vector3 Flat(this Vector3 vec) => new Vector3(vec.x, 0, vec.z);
    
    public static Vector3 RotateFlatVector(Vector3 vec)
    {
        vec = Camera.main.transform.TransformDirection(vec);
        vec = new Vector3(vec.x, 0, vec.z).normalized;
        return vec;
    }

    public static Vector3 MouseWorldPos(float height)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));
        plane.Raycast(mouseRay, out float planeDistance);
        Vector3 worldMousePos = mouseRay.GetPoint(planeDistance);
        return worldMousePos;
    }

    public static Vector3 DirectionToMouse(Vector3 from)
    {
        Vector3 worldMousePos = MouseWorldPos(from.y);
        return (worldMousePos - from).normalized;

    }
}
