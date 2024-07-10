using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.tag == "Sword")
        {
            MeleeCombat Player = transform.GetComponentInParent<MeleeCombat>();
            Player.gotBlocked = true;
            Debug.Log(other.gameObject);
        }
        */
        if(other.name != "FPS_Player")
        {
            MeleeCombat Player = transform.GetComponentInParent<MeleeCombat>();
            Player.gotBlocked = true;
            if(other.GetComponent<HealthScript>())
            {
                other.GetComponent<HealthScript>().TakeDamage(30f);
                Vector3 closestPoint = other.ClosestPoint(transform.position);
                other.GetComponent<EnemyMelee>().GetHit(closestPoint);
            }
          
        }
        
    }
}
