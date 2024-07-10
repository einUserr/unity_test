using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDetect : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource AS;
    [SerializeField] private GameObject ThisNPC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Sword")
        {
            if(collision.gameObject.GetComponentInParent<EnemyMelee>() && collision.transform.GetComponentInParent<EnemyMelee>().isBlocking == true)
            {
                anim.Play("Idle");
                AS.Play();
            }
            else if(collision.gameObject.GetComponentInParent<MeleeCombat>())
            {
                anim.Play("Idle");
                AS.Play();  
                MeleeCombat Player = collision.GetComponent<MeleeCombat>();
                Player.gotBlocked = true;
            }
        }

    }

}
