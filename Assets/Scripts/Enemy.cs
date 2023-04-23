using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator enemyAnimation; 

    public int maxHealth = 100;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        enemyAnimation.SetTrigger("Hurt");

        if(currentHealth <=0) {
            Die();
        }
    }

    void Die() 
    {
        Debug.Log("Enemy died!");

        enemyAnimation.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false; // Desativa o componente Collider2D
        GetComponent<Rigidbody2D>().simulated = false; // Desativa a simulação do componente Rigidbody2D
        GetComponent<Enemy>().enabled = false;
        this.enabled = false;

    }

}
