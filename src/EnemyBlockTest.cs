using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyBlockTest : MonoBehaviour
{
    [Header("Wichtige Variablen")]
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider SwordCol;
    [SerializeField] private LayerMask EnemyLayer;
    [SerializeField] private float Range;
    [SerializeField] private float MaxRaycastRange = 5f;
    [SerializeField] private Transform RaycastPoint;

    [Header("Blocking")]
    [SerializeField] private float BlockingWaitTime = 2.5f;
    [SerializeField] private float BlockingChance = 60;

    public bool isBlocking = false;
    private bool isRunning = false;
    private float Timer = 0;
    private int randomBlockChance;
    private float ChanceTimer = 0f;




    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        ChanceTimer += Time.deltaTime;

        if(ChanceTimer >= 1.5f)
        {
            randomBlockChance = Random.Range(0, 100);
            ChanceTimer = 0f;
        }


        if(randomBlockChance <= BlockingChance)
        {
            Blocking();
        }

        AnimCheck();
    }


    private void AnimCheck()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("BlockTop"))
        {
            isRunning = true;
            isBlocking = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("BlockLeft"))
        {
            isRunning = true;
            isBlocking = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("BlockRight"))
        {
            isRunning = true;
            isBlocking = true;
        }
        else
        {
            isRunning = false;
            isBlocking = false;
        }
    }

    private void Blocking()
    {
        if (Physics.CheckSphere(transform.position, Range, EnemyLayer))
        {
            RaycastHit hit;
            if (Physics.Raycast(RaycastPoint.position, transform.forward, out hit, MaxRaycastRange, EnemyLayer))
            {
                if (hit.transform.GetComponent<EnemyAttackSriptTest>())
                {
                    EnemyAttackSriptTest enemy = hit.transform.GetComponent<EnemyAttackSriptTest>();
                    if (enemy.StrikeRight == true && isRunning == false && Timer >= BlockingWaitTime && enemy.BlockingRandomNum <= BlockingChance)
                    {
                        anim.SetTrigger("BlockRight");
                        Timer = 0f;
                    }
                    else if (enemy.StrikeLeft == true && isRunning == false && Timer >= BlockingWaitTime && enemy.BlockingRandomNum <= BlockingChance)
                    {
                        anim.SetTrigger("BlockLeft");
                        Timer = 0f;
                    }
                    else if (enemy.StrikeTop == true && isRunning == false && Timer >= BlockingWaitTime && enemy.BlockingRandomNum <= BlockingChance)
                    {
                        anim.SetTrigger("BlockTop");
                        Timer = 0f;
                    }

                }

            }

        }
    }
    
    

}
