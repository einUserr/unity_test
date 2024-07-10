using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider BoxColl;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die");
        GetComponent<EnemyMelee>().enabled = false;
        GetComponent<EnemyAI>().isAlive = false;
        GetComponent<EnemyAI>().enabled = false;
        BoxColl.enabled = false;
        gameObject.layer = 0;

    }

}
