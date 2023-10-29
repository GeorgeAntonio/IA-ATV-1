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

    public Transform patrolPoint, npcToChase;
    public Transform[] wayPoints;
    public Rigidbody2D rb;
    public GameObject mainCamera;
    public NPCSpawner spawner;
    public Vector2 randomWanderTarget;
    public float lastAttackTime = 0f;
    public float stunEndTime = 0f; // Time when the stun ends
    public Vector2 moveDirection;

    public static Controller Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    
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
        return Vector2.Distance(transform.position, targetPosition) <= attackRange && Time.time - lastAttackTime >= attackCooldown;
    }

    public bool IsStunned()
    {
        return Time.time < stunEndTime;
    }

}
