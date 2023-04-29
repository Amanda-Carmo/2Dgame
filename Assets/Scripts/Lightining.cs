using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightining : MonoBehaviour
{
    //private Transform player;
    public int damageLightning;
    
    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    public void Use()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Goblin");

        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damageLightning);
            enemy.GetComponent<Enemy>().ApplyLightning(1);
        }
        
        Destroy(gameObject);
    }
}
