using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyAttackSriptTest : MonoBehaviour
{
    [Header("Wichtige Variablen Attacking")]
    [SerializeField] private BoxCollider Sword;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource AS;


    [SerializeField] private float Timer = 0f;
    private float AttackTime = 3f;
    public int BlockingRandomNum = 0;
    public bool StrikeTop = false;
    public bool StrikeLeft = false;
    public bool StrikeRight = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        AnimChecker();
        Attacking();

    }

    private void AnimChecker()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("AttackingGruntAnim"))
        {
            Sword.enabled = true;
            StrikeTop = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("AttackingGruntLeft"))
        {
            Sword.enabled = true;
            StrikeLeft = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("AttackingGruntRight"))
        {
            Sword.enabled = true;
            StrikeRight = true;
        }
        else
        {
            Sword.enabled = false;
            StrikeLeft = false;
            StrikeRight = false;
            StrikeTop = false;
        }
    }
            
    private void Attacking()
    {
        if (Timer >= AttackTime)
        {
            int randomNum;
            randomNum = Random.Range(0, 9);
            if (randomNum <= 3)
            {
                anim.SetTrigger("AttackTop");
                Timer = 0f;
                Sword.enabled = true;
                AS.Play();
            }
            else if (randomNum > 3 && randomNum <= 6)
            {
                anim.SetTrigger("AttackLeft");
                Timer = 0f;
                Sword.enabled = true;
                AS.Play();
            }
            else if (randomNum > 6)
            {
                anim.SetTrigger("AttackRight");
                Timer = 0f;
                Sword.enabled = true;
                AS.Play();
            }

            BlockingRandomNum = Random.Range(0, 100);
        }
    }


            
}
