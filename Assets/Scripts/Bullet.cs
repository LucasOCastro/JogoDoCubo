using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed=10;
    [SerializeField] private float impact = 10;
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
    
    void OnCollisionEnter(Collision other)
    {   
        
        //Quando tivermos classe de vida, devemos dar getcomponent nela.
        if (other.gameObject.TryGetComponent<Enemy>(out var enemy)) //Verifica se o objeto possui a Tag "Enemy"
        {
            Debug.Log("Acertou inimigo");
            //Função de Dano
            
            //Isso provavelmente deveria ir pra classe de vida
            enemy.SpawnRagdoll(other.GetContact(0).point, transform.forward * impact);
        }


        Destroy(gameObject);
    }
}
