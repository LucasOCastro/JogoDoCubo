using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musica : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
               source.loop = true;
               source.clip = clip;
               source.volume = 0.3f;
               source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
