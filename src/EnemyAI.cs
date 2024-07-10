using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using UnityEditor.UIElements;
using UnityEngine.SocialPlatforms;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public Transform Target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float Distance;
    [SerializeField] private float SightRange = 50f;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float radius = 15f;
    [SerializeField] private LayerMask Targetlayer;
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private Animator anim;

    [Header("TEAMS")]
    [SerializeField] public int TeamNum = 1;

    public bool Formation = false;
    public MeleeCombat Player;
    public EnemyMelee Enemy;
    public EnemyAI enemyAI;
    public bool TargetIsPlayer;
    public bool TargetIsEnemy;
    public bool isAlive = true;

    Vector3 previousPos;
    private float Speed = 0f;
    private float distance = 0f;
    private Transform closestTarget;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Formation == false)
        {
            agent.stoppingDistance = 1.5f;
            FindTarget();
            FaceTarget();
        }
        else
        {
            agent.stoppingDistance = 0f;
        }
        

        //Die Distanz berechnen
        Distance = Vector3.Distance(transform.position, Target.position);
        
        //So können wir den Radius für die Sichtung von targets codieren
        if(Distance <= SightRange)
        {
           agent.SetDestination(Target.position);
        }

        GetSpeed();
    }




    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, Targetlayer);
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            if(col.gameObject.GetComponent<EnemyAI>() != null)
            {
                enemyAI = col.GetComponent<EnemyAI>();
            }

            if (col.gameObject.name != transform.name && TeamNum != enemyAI.TeamNum && enemyAI.isAlive)
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = col.transform;
                    Enemy = col.GetComponent<EnemyMelee>();
                    TargetIsEnemy = true;
                    TargetIsPlayer = false;
                }
            }
            else if(col.gameObject.name != transform.name && col.gameObject.tag == "Player")
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = col.transform;
                    Player = col.GetComponent<MeleeCombat>();
                    TargetIsEnemy = false;
                    TargetIsPlayer = true;
                }
            }
            
        }

        if (closestTarget != null)
        {
            Target = closestTarget;
        }
    

    /*
     * 
    if (col.gameObject.name != transform.name && TeamNum != enemyAI.TeamNum && enemyAI.isAlive == true)
    {
        Enemy = col.GetComponent<EnemyMelee>();
        TargetIsEnemy = true;
        TargetIsPlayer = false;
        Target = Enemy.transform;      
    }


    else if (col.gameObject.name != transform.name && col.gameObject.layer == PlayerLayer)
    {
        Player = col.gameObject.GetComponent<MeleeCombat>();
        EnemyMelee EnemyMelee = transform.GetComponent<EnemyMelee>();
        TargetIsEnemy = false;
        TargetIsPlayer = true;
        if(EnemyMelee.isBlocking == true)
        {
            Target = Player.SwordEndPos;
        }
        else
        {
            Target = Player.Sword;
        }
    }
    */

    /*
    if (distance > closestDistance)
    {
        closestDistance = distance;
        Target = col.transform;
        Debug.Log(distance);
    } 
    */

    }

    private void FaceTarget()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);
        //Debug.Log(direction);
    }

    public void FaceTargetToGeneral(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);
        //Debug.Log(direction);
    } 

    public void SetTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }

    private void GetSpeed()
    {
        Vector3 currentMove = transform.position - previousPos;
        Speed = currentMove.magnitude / Time.deltaTime;
        anim.SetFloat("WalkingSpeed", Speed);
        previousPos = transform.position;
    }
}
