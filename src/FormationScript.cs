using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class FormationScript : MonoBehaviour
{
    [Header("Allgemeine Variablen")]
    [SerializeField] private List<GameObject> NPCs = new List<GameObject>();

    [Header("Formation")]
    [SerializeField] private Transform General;
    [SerializeField] private float Spread = 2f;
    [SerializeField] private int width = 2;
    [SerializeField] private int length = 5;
    [SerializeField] private bool Follow = false;
    [SerializeField] private bool inFormation = true;
    

    //private int LastUnit = 0;
    //private int lastX = 0;
    private Vector3 MiddleOffset;



    void Start()
    {
        foreach (Transform child in transform)
        {
            NPCs.Add(child.gameObject);
        }
    }

    void Update()
    {
        MiddleOffset = new Vector3(width / 2 , 0, length / 2);

        if(inFormation)
        {
            FormationSetter();
        }
        else
        {
            foreach(GameObject npc in NPCs)
            {
                EnemyAI enemy = npc.GetComponent<EnemyAI>();
                enemy.Formation = false;
            }
        }
        
    }

    private void FormationSetter()
    {
        /*
        length = NPCs.Count / width;
        for (int z = 0; z < width; z++)
        {
            for (int x = 0; x < length; x++)
            {
                var SpreadDistance = x * Spread;
                EnemyAI enemyAI = NPCs[x + LastUnit].GetComponent<EnemyAI>();
                Vector3 NpcPos = transform.position - new Vector3(SpreadDistance, 0, z * Spread);
                enemyAI.SetTarget(NpcPos);
                enemyAI.FaceTargetToGeneral(General);
                lastX = x;
            }
            LastUnit = z * length;
        }
        LastUnit = 0;
        */

        length = Mathf.CeilToInt(NPCs.Count / (float)width);
        for (int z = 0; z < width; z++)
        {
            float xPos = 0f;
            for (int x = 0; x < length && x + z * length < NPCs.Count; x++)
            {
                EnemyAI enemyAI = NPCs[x + z * length].GetComponent<EnemyAI>();
                if(Follow)
                {
                    Vector3 npcPos = new Vector3(General.position.x - 5, 0, General.position.z - 5) + new Vector3(xPos, 0f, (z * Spread));
                    enemyAI.SetTarget(npcPos);
                }
                else
                {
                    Vector3 npcPos = transform.position + new Vector3(xPos, 0f, z * Spread);
                    enemyAI.SetTarget(npcPos);
                }
                enemyAI.FaceTargetToGeneral(General);
                xPos += Spread;
            }
        }

    }
}
