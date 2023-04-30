using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightningEnemy : MonoBehaviour
{
    [SerializeField] private AudioSource lightningSoundEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        lightningSoundEffect.Play();
    }

}
