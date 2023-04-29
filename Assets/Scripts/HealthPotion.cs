using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{

    //private Transform player;
    private PlayerController player;
    public float heal = 1.0f;
    [SerializeField] private AudioSource itemCollectSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        itemCollectSoundEffect.Play();
    }

    // Update is called once per frame
    void Use()
    {
        player.Heal(heal);
        Destroy(gameObject);
    }
}
