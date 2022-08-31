using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SillyMovement : MonoBehaviour
{
    // VARIÁVEIS SERIALIZADAS
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform movCntrlTrans;

    [SerializeField]
    Transform fowardsTrans;

    [SerializeField]
    Transform lFootBoneEnd;

    [SerializeField]
    Transform rFootBoneEnd;


    // VARIÁVEIS INTERNAS PRIVADAS
    Vector3 movementGoal;


    // Start is called before the first frame update
    void Start()
    {
        movementGoal = new Vector3();
    }


    // Update is called once per amount of time (idk)
    void FixedUpdate () 
    {
        // Debug.log(Input.GetAxis("Horizontal"));
        movementGoal = getMovementGoal();

        Debug.Log(movementGoal);
        rb.velocity = 10*movementGoal;

        Vector3 forwards = Vector3.Normalize(movCntrlTrans.position - fowardsTrans.position);
        // Debug.Log(movementGoal);
    }//


    Vector3 getMovementGoal()
    {
        Vector3 v = Vector3.right * Input.GetAxis("Horizontal") + 
                    Vector3.forward * Input.GetAxis("Vertical") ;
        
        return v.magnitude > 1 ? v.normalized : v;
    }
}
