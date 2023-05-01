using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GruzMother : MonoBehaviour
{
    public float speed;
    // public Transform playerTransform;
    private float distance;
    public float attackRange = 3.0f;
    public float damage = 0.50f;
    bool isAttacking = false;
    public float attackBufferDistance = 0.25f; // distância de buffer de ataque

    // Burn, Freeze and Lightning
    public List<int> burnTickTimes = new List<int>();
    public List<int> freezeTickTimes = new List<int>();
    public List<int> lightiningTickTimes = new List<int>();
    public GameObject fireHead; 
    public GameObject iceHead;
    public GameObject lightningHead; 

    public int maxHealth = 250;
    int currentHealth;

    private PlayerController playerController;


    [Header("Idel")]
    [SerializeField] float idelMovementSpeed;
    [SerializeField] Vector2 idelMovementDirection;

    [Header("AttackUpNDown")]
    [SerializeField] float attackMovementSpeed;
    [SerializeField] Vector2 attackMovementDirection;

    [Header("AttackPlayer")]
    [SerializeField] float attackPlayerSpeed;
    [SerializeField] Transform player;

    [Header("Other")]
    [SerializeField] Transform goundCheckUp;
    [SerializeField] Transform goundCheckDown;
    [SerializeField] Transform goundCheckWall;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;

    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWall;
    private bool hasPlayerPositon;

    private Vector2 playerPosition;

    private bool facingLeft = true;
    private bool goingUp = true;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;


    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        currentHealth = maxHealth;
        lightningHead.SetActive(false);
        fireHead.SetActive(false);  
        iceHead.SetActive(false);   

        idelMovementDirection.Normalize();
        attackMovementDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        isTouchingUp = Physics2D.OverlapCircle(goundCheckUp.position, groundCheckRadius, groundLayer); 
        isTouchingDown = Physics2D.OverlapCircle(goundCheckDown.position, groundCheckRadius, groundLayer); 
        isTouchingWall = Physics2D.OverlapCircle(goundCheckWall.position, groundCheckRadius, groundLayer);
        
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        Vector2 newPosition = transform.position;

        Debug.Log("Distance to player: " + distanceToPlayer);

        if (distanceToPlayer <= attackRange && !isAttacking && playerController.canTakeDamage)
        {
            Debug.Log("Attack");
            // enemyAnimation.SetBool("isWalking", false);
            Attack();
        }
    }

    void RandomStatePicker()
    {
        int randomState = Random.Range(0, 2);
        if (randomState == 0)
        {
            enemyAnim.SetTrigger("AttackUpNDown");
        }
        else if (randomState == 1)
        {
            enemyAnim.SetTrigger("AttackPlayer");
        }
    }

   public void IdelState()
    {
        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }
        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }
        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = idelMovementSpeed * idelMovementDirection;
    } 

    // _________________________________________________________________________________________________________
    
    // Boss atacado pelo player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Espada de fogo
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

    // Espada de gelo    
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
        enemyAnim.SetBool("Die", true);

        GetComponent<Collider2D>().enabled = false; // Desativa o componente Collider2D
        GetComponent<Rigidbody2D>().simulated = false; // Desativa a simulação do componente Rigidbody2D
        GetComponent<Enemy>().enabled = false;
        this.enabled = false;
    }

    // __________________________________________________________________________________________________________

   public void AttackUpNDownState()
    {

        if (isTouchingUp && goingUp)
        {
            ChangeDirection();
        }

        else if (isTouchingDown && !goingUp)
        {
            ChangeDirection();
        }

        if (isTouchingWall)
        {
            if (facingLeft)
            {
                Flip();
            }
            else if (!facingLeft)
            {
                Flip();
            }
        }
        enemyRB.velocity = attackMovementSpeed * attackMovementDirection;
    }

    public void AttackPlayerState()
    {
       
        if (!hasPlayerPositon)
        {
            FlipTowardsPlayer();
             playerPosition = player.position - transform.position;
            playerPosition.Normalize();
            hasPlayerPositon = true;
        }

        if (hasPlayerPositon)
        {
            enemyRB.velocity = attackPlayerSpeed * playerPosition;
        }
        
        if (isTouchingWall || isTouchingDown)
        {
            //play Slam animation
            enemyAnim.SetTrigger("Slamed");
            enemyRB.velocity = Vector2.zero;
            hasPlayerPositon = false;
        }
    }

    private void Attack()
    {
        isAttacking = true;
        Debug.Log("Atacou");

        // dano ataque corpo a corpo com player
        if (Vector2.Distance(transform.position, player.position) <= attackRange && playerController.canTakeDamage && !playerController.hasDefense)
        {
            playerController.Hurt(playerController.damageTaken);
        }
        else if (Vector2.Distance(transform.position, player.position) <= attackRange && playerController.canTakeDamage && playerController.hasDefense)
        {
            playerController.Hurt(playerController.damageTaken - playerController.defense);
        }

        isAttacking = false;
        
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if (playerDirection>0 && facingLeft)
        {
            Flip();
        }
        else if (playerDirection<0 && !facingLeft)
        {
            Flip();
        }
    }

    void ChangeDirection()
    {
        goingUp = !goingUp;
        idelMovementDirection.y *= -1;
        attackMovementDirection.y *= -1;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        idelMovementDirection.x *= -1;
        attackMovementDirection.x *= -1;
        transform.Rotate(0, 180, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(goundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(goundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(goundCheckWall.position, groundCheckRadius);
    }
}
