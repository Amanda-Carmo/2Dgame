using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private Enemy enemy;
    public List<int> burnTickTimes = new List<int>();
    public int attackDamage = 5;
    // Start is called before the first frame update
    void Start()
    {
        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
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
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            burnTickTimes.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
