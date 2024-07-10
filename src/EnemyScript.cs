using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float health = 100f;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    [Header("Other")]
    [SerializeField] private float Timer;
    [SerializeField] private AudioSource AS;

    // Start is called before the first frame update
    void Start()
    {
        Timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        //Debug.Log(Timer);
        if(Timer >= 5f)
        {
            anim.SetTrigger("Attack");
            Timer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword" && other.GetComponentInParent<MeleeCombat>().isStretched == true)
        {
            Debug.Log("Enemy Hit!!!");
            anim.SetTrigger("Hit");
            AS.Play();
        }
    }


}
