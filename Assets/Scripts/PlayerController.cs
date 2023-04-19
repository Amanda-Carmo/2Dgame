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

    private bool canTakeDamage = true;
    public float damageCooldown = 1f;
    private float lastDamageTime;
    

    //public HealthBar healthBar;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
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

        if (!canTakeDamage && Time.time - lastDamageTime > damageCooldown)
        {
            canTakeDamage = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Spike")
        {
            if (canTakeDamage)
            {
                Hurt(0.25f);
                lastDamageTime = Time.time;
                canTakeDamage = false;
            }
        }
    }

    public void Hurt(float dmg)
    {
        PlayerStats.Instance.TakeDamage(dmg);
    }

    public void AddHealth()
    {
        PlayerStats.Instance.AddHealth();
    }

}
