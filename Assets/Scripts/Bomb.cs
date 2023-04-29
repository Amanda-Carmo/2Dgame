using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //private Transform player;
    private Transform player;
    public GameObject item;
    public GameObject explosion;

    private bool used = false; // evitar bombas duplicadas
    public float attackRange = 5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;

    public float SplashRange = 1;
    [SerializeField] private AudioSource itemCollectSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        itemCollectSoundEffect.Play();
    }

    // Update is called once per frame
    public void Use()
    {
        if (used) return; // retorna imediatamente se o item já foi usado
        used = true; // marca o item como usado

        Vector2 playerPos = new Vector2(player.position.x, player.position.y);
        GameObject itemObject = Instantiate(item, playerPos, Quaternion.identity);
        Animator itemAnimator = itemObject.GetComponent<Animator>();
        Destroy(itemObject, itemAnimator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(ExplosionCoroutine(playerPos, itemAnimator.GetCurrentAnimatorStateInfo(0).length)); //Inicia a corrotina para a explosão
    }

    IEnumerator ExplosionCoroutine(Vector2 position, float delay)
    {
        yield return new WaitForSeconds(delay); //Espera o tempo necessário para o item ser destruído
        GameObject explosionObject = Instantiate(explosion, position, Quaternion.identity);
        Animator explosionAnimator = explosionObject.GetComponent<Animator>();
        Damage();
        Destroy(explosionObject, explosionAnimator.GetCurrentAnimatorStateInfo(0).length ); //Destrói a explosão após o fim da animação
        Destroy(gameObject);
    }


    public void Damage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);
        
        foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
    }



}
