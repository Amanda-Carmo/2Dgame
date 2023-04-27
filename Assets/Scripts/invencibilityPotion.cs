using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invencibilityPotion : MonoBehaviour
{
    //private Transform player;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    public void Use()
    {
        player.hasInvencibility = true;
        Destroy(gameObject);
    }
}
