using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
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

    // Burn, Freeze and Lightning
    public List<int> burnTickTimes = new List<int>();
    public List<int> freezeTickTimes = new List<int>();
    public List<int> lightiningTickTimes = new List<int>();
    public GameObject fireHead; 
    public GameObject iceHead;
    public GameObject lightningHead; 

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        GetComponent<Rigidbody2D>().freezeRotation = true;
        playerController = FindObjectOfType<PlayerController>();
        fireHead.SetActive(false);
        iceHead.SetActive(false);
        lightningHead.SetActive(false);
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
            if (gameObject.CompareTag("Goblin"))
            {
                if (playerTransform.position.x > transform.position.x)
                {
                    // o playerTransform está à direita
                    transform.localScale = new Vector3(8f, 8f, 1f); // flip horizontal

                }
                else
                {
                    transform.localScale = new Vector3(-8f, 8f, 1f); // flip horizontal

                }
            }
            if (gameObject.CompareTag("FlyingEye"))
            {
                if (playerTransform.position.x > transform.position.x)
                {
                    // o playerTransform está à direita
                    transform.localScale = new Vector3(24.5f, 23.5f, 1f); // flip horizontal
                }
                else
                {
                    transform.localScale = new Vector3(-24.5f, 23.5f, 1f); // flip horizontal

                }
                if (Mathf.Abs(transform.position.y - playerTransform.position.y) > 1.0f) // se a diferença de altura for maior que 1.5f
                {
                    newPosition = new Vector2(transform.position.x, playerTransform.position.y);
                }
                else
                {
                    newPosition = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
                    enemyAnimation.SetBool("isWalking", true);
                }
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

    public void ApplyBurn(int ticks)
    {
        if (burnTickTimes.Count <= 0)
        {
            burnTickTimes.Add(ticks);
            StartCoroutine(Burn());
        }
        else
        {
            burnTickTimes.Add(ticks);
        }
    }

    IEnumerator Burn()
    {
        while(burnTickTimes.Count > 0)
        {
            for(int i = 0; i < burnTickTimes.Count; i++)
            {
                burnTickTimes[i]--;
            }
            TakeDamage(5);
            fireHead.SetActive(true);
            burnTickTimes.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
        if (burnTickTimes.Count <= 0)
        {
            fireHead.SetActive(false);
        }
    }

    public void ApplyFreeze(int ticks)
    {
        if (freezeTickTimes.Count <= 0)
        {
            freezeTickTimes.Add(ticks);
            StartCoroutine(Freeze());
        }
        else
        {
            freezeTickTimes.Add(ticks);
        }
    }

    IEnumerator Freeze()
    {
        while(freezeTickTimes.Count > 0)
        {
            for(int i = 0; i < freezeTickTimes.Count; i++)
            {
                freezeTickTimes[i]--;
            }
            speed = 5;
            iceHead.SetActive(true);
            freezeTickTimes.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
        if (freezeTickTimes.Count <= 0)
        {
            iceHead.SetActive(false);
            speed = 10;
        }
    }

    public void ApplyLightning(int ticks)
    {
        if (lightiningTickTimes.Count <= 0)
        {
            lightiningTickTimes.Add(ticks);
            StartCoroutine(Lightining());
        }
        else
        {
            lightiningTickTimes.Add(ticks);
        }
    }

    IEnumerator Lightining()
    {
        while(lightiningTickTimes.Count > 0)
        {
            for(int i = 0; i < lightiningTickTimes.Count; i++)
            {
                lightiningTickTimes[i]--;
            }
            lightningHead.SetActive(true);
            lightiningTickTimes.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
        if (lightiningTickTimes.Count <= 0)
        {
            lightningHead.SetActive(false);
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
