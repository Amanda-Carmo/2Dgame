using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.2f;
    public Transform playerTransform;
    private float distance;
    public float attackRange = 1.5f;
    public float damage = 0.25f;
    bool isAttacking = false;
    public float attackBufferDistance = 0.25f; // distância de buffer de ataque


    public Animator enemyAnimation;

    public int maxHealth = 100;
    int currentHealth;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        GetComponent<Rigidbody2D>().freezeRotation = true;
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // calcular a posição de movimento
        Vector2 newPosition = transform.position;

        if (distanceToPlayer <= attackRange && !isAttacking && playerController.canTakeDamage)
        {
            // parar antes da distância de buffer de ataque
            float distanceToStop = distanceToPlayer - attackBufferDistance;
            newPosition = Vector2.MoveTowards(transform.position, playerTransform.position, distanceToStop);

            enemyAnimation.SetBool("isWalking", false);
            Attack();
        }
        else
        {
            newPosition = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            enemyAnimation.SetBool("isWalking", true);

            // verificar a posição do playerTransform em relação ao Enemy
            if (playerTransform.position.x > transform.position.x)
            {
                // o playerTransform está à direita
                transform.localScale = new Vector3(3f, 2.5f, 1f); // flip horizontal
            }
            else
            {
                // o playerTransform está à esquerda
                transform.localScale = new Vector3(-3f, 2.5f, 1f); // flip horizontal
            }
        }

        // atualizar a posição
        transform.position = newPosition;
        
    }


    private void Attack()
    {
        isAttacking = true;

        enemyAnimation.SetTrigger("Attack");

        // dano ataque corpo a corpo com player
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange && playerController.canTakeDamage && !playerController.hasDefense)
        {
            playerController.Hurt(playerController.damageTaken);
        }
        else if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange && playerController.canTakeDamage && playerController.hasDefense)
        {
            playerController.Hurt(playerController.damageTaken - playerController.defense);
        }

        isAttacking = false;
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        enemyAnimation.SetTrigger("Hurt");

        if(currentHealth <=0) {
            Die();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Explosion") {
            TakeDamage(25);
        }
    }

    void Die() 
    {
        enemyAnimation.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false; // Desativa o componente Collider2D
        GetComponent<Rigidbody2D>().simulated = false; // Desativa a simulação do componente Rigidbody2D
        GetComponent<Enemy>().enabled = false;
        this.enabled = false;
    }
}
