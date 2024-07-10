using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [Header("Allgemeine Variablen")]
    [SerializeField] private AudioSource FleshCutAS;
    [SerializeField] private float damage = 30f;
    [SerializeField] private GameObject bloodParticle;



    [Header("Wichtige Variablen Blocking")]
    [SerializeField] public Animator anim;
    [SerializeField] private BoxCollider SwordCol;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private float Range;
    [SerializeField] private float MaxRaycastRange = 5f;
    [SerializeField] private Transform RaycastPoint;

    [Header("Blocking")]
    [SerializeField] private float BlockingWaitTime = 2.5f;
    [SerializeField] private float BlockingChance = 60;

    public bool isBlocking = false;
    private bool isRunning = false;
    private float TimerBlocking = 0;


    [Header("Wichtige Variablen Attacking")]
    [SerializeField] public BoxCollider Sword;
    [SerializeField] private AudioSource AS;
    [SerializeField] private int AttackingChance = 60;


    //[SerializeField] private float Timerr = 0f;
    private float AttackTime = 3f;
    public int BlockingRandomNum = 0;
    public bool StrikeTop = false;
    public bool StrikeLeft = false;
    public bool StrikeRight = false;
    private float TimerAttacking = 0f;

    //Test-Variablen
    private int randomAttackingNum = 0;
    private float IfAttackTimer = 0f;
    private bool isReady = false;
    public bool whileBlocking = false;
    private Vector3 closestPoint;

    public MeleeCombat Player;
    public EnemyMelee Enemy;
    public EnemyAI ai;


    // Update is called once per frame
    void Update()
    {
        if(GetComponent<EnemyAI>().Player != null)
        {
            Player = GetComponent<EnemyAI>().Player;
        }

        if(GetComponent<EnemyAI>().Enemy != null)
        {
            Enemy = GetComponent<EnemyAI>().Enemy;
        }

        ai = GetComponent<EnemyAI>();

        TimerAttacking += Time.deltaTime;
        TimerBlocking += Time.deltaTime;
        IfAttackTimer += Time.deltaTime;

        AnimCheckBlocking();
        AnimCheckAttacking();


        isReady = TimerBlocking >= BlockingWaitTime;


  
        if(IfAttackTimer >= 1.5f)
        {
            randomAttackingNum = Random.Range(0, 100);
            IfAttackTimer = 0f;
        }

        
        if(Physics.CheckSphere(transform.position, 3f, PlayerLayer))
        {
            if (randomAttackingNum <= AttackingChance)
            {
                Attacking();
            }

            BlockingPlayer();
        }
        else if (Physics.CheckSphere(transform.position, 3f, EnemyLayer))
        {
            if (randomAttackingNum <= AttackingChance)
            {
                Attacking();
            }
            else
            {
                BlockingNPC();
            }
        }

    }


    private void BlockingNPC()
    {
          Enemy = GetComponent<EnemyAI>().Enemy;
          if (Enemy.StrikeRight == true && isRunning == false && TimerBlocking >= BlockingWaitTime && Enemy.BlockingRandomNum <= BlockingChance)
          {
             anim.SetTrigger("BlockRight");
             TimerBlocking = 0f;
          }
          else if (Enemy.StrikeLeft == true && isRunning == false && TimerBlocking >= BlockingWaitTime && Enemy.BlockingRandomNum <= BlockingChance)
          {
             anim.SetTrigger("BlockLeft");
             TimerBlocking = 0f;
          }
          else if (Enemy.StrikeTop == true && isRunning == false && TimerBlocking >= BlockingWaitTime && Enemy.BlockingRandomNum <= BlockingChance)
          {
              anim.SetTrigger("BlockTop");
              TimerBlocking = 0f;
          }
    }

    private void BlockingPlayer()
    {

        //MeleeCombat Player = hit.transform.GetComponent<MeleeCombat>();
        MeleeCombat Player = transform.GetComponent<EnemyAI>().Player;
        if (Player.UpperStrike == true && isRunning == false && isReady == true)
        {
            anim.SetTrigger("BlockTop");
            TimerBlocking = 0f;
        }
        else if (Player.SideStrike == true && isRunning == false && isReady == true)
        {
            anim.SetTrigger("Test");
            TimerBlocking = 0f;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sword" && other.GetComponentInParent<EnemyAI>().TeamNum != GetComponent<EnemyAI>().TeamNum)
        {
            anim.SetTrigger("Hurt");
            FleshCutAS.Play();
            other.GetComponentInParent<EnemyMelee>().anim.SetTrigger("Idle");
            GetComponent<HealthScript>().TakeDamage(damage);
            closestPoint = other.ClosestPoint(transform.position);
            Instantiate(bloodParticle, closestPoint, Quaternion.identity);
        }
        
        if(other.tag == "Sword" && other.GetComponent<MeleeCombat>())
        {
            anim.SetTrigger("Hurt");
            FleshCutAS.Play();
            other.GetComponentInParent<EnemyMelee>().anim.SetTrigger("Idle");
            closestPoint = other.ClosestPoint(transform.position);
            Instantiate(bloodParticle, closestPoint, Quaternion.identity);
        }

    }
   
    public void GetHit(Vector3 contactPoint)
    {
        anim.SetTrigger("Hurt");
        FleshCutAS.Play();
        Instantiate(bloodParticle, contactPoint, Quaternion.identity);
    }


    private void Attacking()
    {
        if (TimerAttacking >= AttackTime)
        {
            int randomNum;
            randomNum = Random.Range(0, 9);
            if (randomNum <= 3)
            {
                anim.SetTrigger("AttackTop");
                TimerAttacking = 0f;
                AS.Play();
            }
            else if (randomNum > 3 && randomNum <= 6)
            {
                anim.SetTrigger("AttackLeft");
                TimerAttacking = 0f;
                AS.Play();
            }
            else if (randomNum > 6)
            {
                anim.SetTrigger("AttackRight");
                TimerAttacking = 0f;
                AS.Play();
            }
        }
    }


    private void AnimCheckBlocking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("BlockTop"))
        {
            isRunning = true;
            isBlocking = true;
            Sword.enabled = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("BlockLeft"))
        {
            isRunning = true;
            isBlocking = true;
            Sword.enabled = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("BlockRight"))
        {
            isRunning = true;
            isBlocking = true;
            Sword.enabled = true;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("TestBlockk"))
        {
            isRunning = true;
            isBlocking = true;
            Sword.enabled = true;
        }
        else
        {
            isRunning = false;
            isBlocking = false;
            Sword.enabled = false;
        }
    }

    private void AnimCheckAttacking()
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
            isRunning = false;
            Sword.enabled = false;
            StrikeLeft = false;
            StrikeRight = false;
            StrikeTop = false;
        }
    }


}
