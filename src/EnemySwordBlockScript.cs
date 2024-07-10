using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordBlockScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource AS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("EnemyAttack") && other.tag == "Sword")
        {
            //Debug.Log("Attack Blocked");
            AS.Play();
            anim.Play("EnemyNormal");
        }
    }


}
