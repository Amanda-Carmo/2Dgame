using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invencibilityPotion : MonoBehaviour
{
    //private Transform player;
    private PlayerController player;
    [SerializeField] private AudioSource itemCollectSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        itemCollectSoundEffect.Play();
    }

    // Update is called once per frame
    public void Use()
    {
        player.GetComponent<PlayerController>().ApplyInvulnerability(4);
        Destroy(gameObject);
    }
}
