using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 6f;
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    private Animator playerAnimation;

    public bool canTakeDamage = true;
    public float damageCooldown = 1f;
    private float lastDamageTime;
    private bool isDead = false;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();

        //attackArea = transform.GetChild(0).gameObject;
    }
    

    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (direction > 0f)
        {
            player.velocity = new Vector2(direction*speed, player.velocity.y);
            transform.localScale = new Vector2(3.5544f, 3.5544f);
        }

        else if (direction < 0f)
        {
            player.velocity = new Vector2(direction*speed, player.velocity.y);
            transform.localScale = new Vector2(-3.5544f, 3.5544f);
        }

        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && isTouchingGround)
        {
            player.velocity = new Vector2(player.velocity.x, jumpSpeed);
        }

        playerAnimation.SetFloat("Speed", Mathf.Abs(player.velocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);
        playerAnimation.SetBool("canTakeDamage", canTakeDamage);

        if (PlayerStats.Instance.Health <= 0 && isDead == false) 
        {
            Dead();
        }

        if (!canTakeDamage && Time.time - lastDamageTime > damageCooldown)
        {
            canTakeDamage = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Attack();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PotionHealth") {
            Heal(1.0f);
            collision.gameObject.SetActive(false);
        }

        if (collision.tag == "PotionSpeed") {
            speed = 8f;
            collision.gameObject.SetActive(false);
        }

        if (collision.tag == "HeartItem") {
            AddHealth();
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Spike")
        {
            if (canTakeDamage && isDead == false)
            {
                Hurt(0.25f);
            }
        }
    }

    public void Dead() 
    {
        playerAnimation.SetTrigger("Death");
        isDead = true;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Collider2D>().enabled = false; // Desativa o componente Collider2D
        GetComponent<Rigidbody2D>().simulated = false; // Desativa a simulação do componente Rigidbody2D
        this.enabled = false;
    }

    public void Hurt(float dmg)
    {
        canTakeDamage = false;
        PlayerStats.Instance.TakeDamage(dmg);
        lastDamageTime = Time.time;
    }

    public void AddHealth()
    {
        PlayerStats.Instance.AddHealth();
    }

    public void Heal(float health)
    {
        PlayerStats.Instance.Heal(health);
    }

    private void Attack()
    {
        playerAnimation.SetTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) 
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}