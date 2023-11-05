using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour { 
    public float moveSpeed = 3f;
    public float chaseRange = 20f;
    public float attackRange = 1.5f;
    public int health = 100;
    public int maxHealth = 100;
    public int attackDamage = 10;
    public float attackCooldown = 2f;
    public float knockbackForce = 2f;
    public float patrolCircleRadius = 10f;
    public float stunDuration = 0.3f; // Stun duration after knockback
    public float fleeHealthThreshold = 0.2f;

    public Transform patrolPoint, npcToChase, npcToAttack;
    public Transform[] wayPoints;
    public Rigidbody2D rb;
    public GameObject mainCamera;
    public NPCSpawner spawner;
    public Vector2 randomWanderTarget;
    public float lastAttackTime = 0f;
    public float stunEndTime = 0f; // Time when the stun ends
    public Vector2 moveDirection;

    private void Start()
    { 

        rb = GetComponent<Rigidbody2D>();
        patrolPoint = GetRandomPatrolPoint();

        moveSpeed = Random.Range(5f, 8f);
        chaseRange = Random.Range(7f, 15f);
        attackRange = Random.Range(3f, 5f);
        health = Random.Range(50, 130);
        maxHealth = Random.Range(50, 130);
        attackDamage = Random.Range(10, 30);
        attackCooldown = Random.Range(1.5f, 3.5f);
        knockbackForce = Random.Range(3f, 7f);
        stunDuration = Random.Range(0.3f, 1f);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        spawner = GameObject.FindGameObjectWithTag("NPCspawner").GetComponent<NPCSpawner>();
    }

    private Transform GetRandomPatrolPoint()
    {
        GameObject[] patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");

        if (patrolPoints.Length == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, patrolPoints.Length);
        return patrolPoints[randomIndex].transform;
    }

    public Transform FindClosestNPC()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        Transform closestNPC = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject npc in npcs)
        {
            if (npc != this.gameObject)
            {
                float distance = Vector2.Distance(transform.position, npc.transform.position);

                if (distance <= chaseRange && distance < closestDistance)
                {
                    closestNPC = npc.transform;
                    closestDistance = distance;
                }
            }
        }

        return closestNPC;
    }

    public bool CanChase()
    {
        Transform closestNPC = FindClosestNPC();
        return closestNPC != null;
    }

    public bool CanAttack(Vector2 targetPosition)
    {
        Debug.Log("ataca");
        return Vector2.Distance(transform.position, targetPosition) <= attackRange && Time.time - lastAttackTime >= attackCooldown;
    }

    public bool IsStunned()
    {
        return Time.time < stunEndTime;
    }

}