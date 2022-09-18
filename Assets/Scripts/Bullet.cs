using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed=10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    
    void OnCollisionEnter(Collision other)
    {   
        
        //Quando tivermos classe de vida, podemos apenas dar um getcomponent aqui ao inves de comparar tags.
        if (other.gameObject.CompareTag("Enemy"))//Verifica se o objeto possui a Tag "Enemy"
         {
            Debug.Log("Acertou inimigo");
            //Função de Dano
         }
        
        Destroy(gameObject);
    }
}
