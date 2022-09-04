using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Leg : byte {left, right};
public class SillyMovement : MonoBehaviour
{
    // VARIÁVEIS SERIALIZADAS
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform mainTrans;

    [SerializeField]
    float acceleration;

    [SerializeField]
    float maxSpeed;

    // TRANSFORMS PARA O CONTROLE DE MOVIMENTO
    Transform 
        movementControlTrans, // Representa o meio do bob
            leftFootTrans, // Posição inicial do pé esquerdo
            rightFootTrans, // Posição inicial do pé direito
            middleTrans, // Posição inicial entre os pés
            rightTrans, // Referência útil para o calculo da direção da direita.
            forwardTrans, // Referência útil para o calculo da direção da frente.
            leftGoalTrans, // Posição avançada do pé esquerdo
            rightGoalTrans, // Posição avançada do pé direito
        rigTrans,
            leftLegTrans,
                leftTargetTrans,
            rightLegTrans,
                rightTargetTrans
    ;
    

    // VARIÁVEIS INTERNAS PRIVADAS
    Vector3 goal; // Guarda para onde moveremos o pé.
    Leg leg; // Guarda a informação de qual perna está se movimentando.
    bool stability;
    float height;


    // Start is called before the first frame update
    void Start()
    {
        // BUSCANDO E CONFIGURANDO TRANSFORMS NECESSÁRIOS
        setupTransforms ();

        // SETANDO VARIÁVEIS
        leg = Leg.right;
        stability = true;
        goal = new Vector3();
        height = 1.1f * (middleTrans.position - movementControlTrans.position).magnitude;
    }



    // Update is called once per amount of time (idk)
    void FixedUpdate () 
    {
        // DESENHANDO INFORMAÇÕES PARA DEBUG
        drawDebugAxis ();

        // BUSCANDO O CHÃO
        RaycastHit ground = getGround();

        // VERIFICANDO SE A ENTIDADE QUER IR PARA ALGUM LUGAR.
        Vector3 movementDecision = getMovementDecision(
            getMovementIntention(), 
            ground
        );
        
        
        applyBasicMovement (ground, movementDecision);
    }

    void applyBasicMovement (RaycastHit ground, Vector3 movementDecision)
    {
        // APLICANDO ESTABILIDADE
        if (stability)
        {
            // AJUSTANDO ALTURA 
            float correction = 1 - ground.distance / height;
            if (correction < 0) correction = 0;

            rb.AddForce(2 * ground.normal * correction * Physics.gravity.magnitude * Time.fixedDeltaTime);

            // AJUSTANDO ANGULO
            // Ps: O que caralhos é um quarternion!?
            Vector3 hitNormal = ground.normal;
            Vector3 vecDiff = (hitNormal - getDownDirection()).normalized;
            rb.rotation = Quaternion.LookRotation (vecDiff, new Vector3(0, 0, -1));
        }


        // APLICANDO MOVIMENTO DESEJADO
        Vector3 currentDesiredMovement = Vector3.Project(rb.velocity, movementDecision);

        if (currentDesiredMovement.magnitude < maxSpeed) 
            rb.velocity += acceleration * movementDecision * Time.fixedDeltaTime;

    }

    /**
     * @brief Código retirado de 
     *  <https://answers.unity.com/questions/50846/how-do-i-obtain-the-surface-normal-for-a-point-on.html>.
     */
    private Vector3 GetMeshColliderNormal(RaycastHit hit)
    {
        if (null == hit.collider) return new Vector3(0, 0, 0);

        MeshCollider collider = (MeshCollider)hit.collider;
        Mesh mesh = collider.sharedMesh;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;

        Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];
        Vector3 baryCenter = hit.barycentricCoordinate; 
        Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
        interpolatedNormal.Normalize();
        interpolatedNormal = hit.transform.TransformDirection(interpolatedNormal);
        return interpolatedNormal;


    }


    void setupTransforms () 
    {
        rigTrans = mainTrans.Find("Rig");
            leftLegTrans = rigTrans.Find("LeftLeg");
                    leftTargetTrans = leftLegTrans.Find("LeftTarget");
            rightLegTrans = rigTrans.Find("RightLeg");
                rightTargetTrans = rightLegTrans.Find("RightTarget");
        movementControlTrans = mainTrans.Find("MovementControl");
            leftFootTrans = movementControlTrans.Find("LeftFoot");
            rightFootTrans = movementControlTrans.Find("RightFoot");
            middleTrans = movementControlTrans.Find("Middle");
            rightTrans = movementControlTrans.Find("Right");
            forwardTrans = movementControlTrans.Find("Forward");
            leftGoalTrans = movementControlTrans.Find("LeftGoal");
            rightGoalTrans = movementControlTrans.Find("RightGoal");
    }

    Vector3 getDownDirection () 
    {
        return Vector3.Normalize(middleTrans.position - movementControlTrans.position);
    }

    Vector3 getForwardsDirection () 
    {
        return Vector3.Normalize(forwardTrans.position - movementControlTrans.position);
    }

    Vector3 getRightDirection () 
    {
        return Vector3.Normalize(rightTrans.position - movementControlTrans.position);
    }



    void drawDebugAxis () {
        Debug.DrawRay(movementControlTrans.position, getDownDirection(), Color.green, Time.fixedDeltaTime, false); 
        Debug.DrawRay(movementControlTrans.position, getForwardsDirection(), Color.blue, Time.fixedDeltaTime, false); 
        Debug.DrawRay(movementControlTrans.position, getRightDirection(), Color.red, Time.fixedDeltaTime, false); 
    }

    RaycastHit getGround ()
    {
        // CONFIGURANDO VARIÁVEIS
        Ray ray = new Ray(movementControlTrans.position, getDownDirection());
        RaycastHit hit;

        // TRAÇANDO O RAIO
        Physics.Raycast(ray, out hit, height);

        // RETORNANDO O QUE O RAIO ENCONTROU
        return hit;
    }

    /** 
     * @brief Essa função parte da intenção de movimento da entidade e,
     *   considerando a posição do chão, retorna um vetor de que indica para
     *   onde a entidade pode e quer ir.
     * @return Um vetor tridimensional que indica a para onde a entidade pode e
     *   quer ir.
     */
    Vector3 getMovementDecision(Vector3 movementIntention, RaycastHit ground)
    {
        Vector3 projectedIntention = Vector3.ProjectOnPlane(
            movementIntention, 
            ground.normal
        );
        return projectedIntention.normalized;
    }

    /** 
     * @return Um vetor tridimensional que indica a para onde a entidade quer ir.
     */
    Vector3 getMovementIntention ()
    {
        return getPlayerInputVector ();
    }

    /** 
     * @return Um vetor tridimensional que indica a para onde o jogador quer ir.
     */
    Vector3 getPlayerInputVector ()
    {
        Vector3 v = Vector3.right * Input.GetAxis("Horizontal") + 
                    Vector3.forward * Input.GetAxis("Vertical") ;
        
        return v.magnitude > 1 ? v.normalized : v;
    }
}
