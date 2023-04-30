using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    public float SplashRange = 1;
    public int attackDamage = 20;
    [SerializeField] private AudioSource explosionSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        explosionSoundEffect.Play();
    }

    private void onCollisionEnter2D(Collision2D collision)
    {
        if(SplashRange > 0)
        {
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, SplashRange);
            foreach(var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponent<Enemy>();
                if (enemy)
                {
                    var closestPoint = hitCollider.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercent = Mathf.InverseLerp(SplashRange, 0, distance);
                    enemy.TakeDamage(attackDamage);
                }

            }
        }
    }
}
