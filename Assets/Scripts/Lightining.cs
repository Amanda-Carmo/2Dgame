using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightining : MonoBehaviour
{
    //private Transform player;
    public int damageLightning;
    
    private Enemy enemy;
    [SerializeField] private AudioSource itemCollectSoundEffect;
    private GruzMother gruzMother;

    // Start is called before the first frame update
    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
        gruzMother = FindObjectOfType<GruzMother>();
        itemCollectSoundEffect.Play();
    }

    // Update is called once per frame
    public void Use()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Goblin");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("GruzMother");

        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damageLightning);
            enemy.GetComponent<Enemy>().ApplyLightning(1);
        }

        foreach(GameObject boss in bosses)
        {
            boss.GetComponent<GruzMother>().TakeDamage(damageLightning);
            boss.GetComponent<GruzMother>().ApplyLightning(1);
        }
        
        Destroy(gameObject);
    }
}
