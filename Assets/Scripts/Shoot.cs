using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetButtonDown("Fire1")){
            shoot();
        }
    }
   
   void shoot () {

    Vector3 screenPlayerPos = Camera.main.WorldToScreenPoint(transform.position);
     //Debug.Log("player" + screenPlayerPos);
     //Debug.Log("mouse" + Input.mousePosition);
    Vector2 direction2D = (Input.mousePosition - screenPlayerPos).normalized;
    
    Vector3 direction = new Vector3(direction2D.x, 0, direction2D.y);
    direction = Camera.main.transform.rotation * direction; 
    direction.y = 0;
    Quaternion rotation = Quaternion.LookRotation(direction,Vector3.up);

    Instantiate(bullet,transform.position,rotation);
   }

}