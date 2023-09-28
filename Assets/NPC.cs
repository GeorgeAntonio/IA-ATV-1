using UnityEngine;

public class NPC : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float attackRange = 1.5f;
    public int health = 100;
    public int attackDamage = 10;
    public float attackCooldown = 2f;
    public float knockbackForce = 2f;

    private Transform patrolPoint;
    private Rigidbody2D rb;
    private enum State { Patrol, Chase, Attack, Flee }
    private State currentState = State.Patrol;
    private Vector2 randomWanderTarget;
    private float lastAttackTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolPoint = GetRandomPatrolPoint();
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Flee:
                Flee();
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoint != null)
        {
            Vector2 moveDirection = (patrolPoint.position - transform.position).normalized;
            rb.velocity = moveDirection * moveSpeed;

            if (CanChase())
            {
                currentState = State.Chase;
            }

            if (Vector2.Distance(transform.position, patrolPoint.position) < 0.1f)
            {
                patrolPoint = GetRandomPatrolPoint();
            }
        }
        else
        {
            WanderRandomly();
        }
    }

    private void WanderRandomly()
    {
        if (randomWanderTarget == Vector2.zero || Vector2.Distance(transform.position, randomWanderTarget) < 1f)
        {
            randomWanderTarget = (Vector2)transform.position + Random.insideUnitCircle * 5f;
        }

        Vector2 moveDirection = (randomWanderTarget - (Vector2)transform.position).normalized;
        rb.velocity = moveDirection * moveSpeed;
    }

    private void Chase()
    {
        Transform npcToChase = FindClosestNPC();

        if (npcToChase != null)
        {
            Vector2 moveDirection = (npcToChase.position - transform.position).normalized;
            rb.velocity = moveDirection * moveSpeed;

            if (CanAttack(npcToChase.position))
            {
                currentState = State.Attack;
            }
            else if (!CanChase())
            {
                currentState = State.Patrol;
            }
        }
        else
        {
            currentState = State.Patrol;
        }
    }

    private void Attack()
    {
        Transform npcToAttack = FindClosestNPC();

        if (npcToAttack != null)
        {
            if (CanAttack(npcToAttack.position))
            {
                Vector2 knockbackDirection = (npcToAttack.position - transform.position).normalized;
                Rigidbody2D targetRB = npcToAttack.GetComponent<Rigidbody2D>();
                
                // Apply knockback force to the target NPC
                if (targetRB != null)
                {
                    targetRB.velocity = knockbackDirection * knockbackForce;
                }

                NPC targetNPC = npcToAttack.GetComponent<NPC>();
                targetNPC.health -= attackDamage;

                lastAttackTime = Time.time;

                // Check if the target NPC's health is zero or below, and destroy it
                if (targetNPC.health <= 0)
                {
                    currentState = State.Patrol;
                }
                else if (health < targetNPC.health)
                {
                    currentState = State.Flee; // Flee if our health is lower than the target's
                }
                else
                {
                    currentState = State.Chase;
                }
            }
            else
            {
                currentState = State.Chase;
            }
        }
        else
        {
            currentState = State.Patrol;
        }
    }

    private void Flee()
    {
        Transform npcToFleeFrom = FindNPCToFleeFrom();

        if (npcToFleeFrom != null)
        {
            Vector2 fleeDirection = (transform.position - npcToFleeFrom.position).normalized;
            rb.velocity = fleeDirection * moveSpeed;

            if (Vector2.Distance(transform.position, npcToFleeFrom.position) > chaseRange)
            {
                currentState = State.Patrol; // Return to patrolling when out of chase range
            }
        }
        else
        {
            currentState = State.Patrol; // No NPC to flee from, return to patrolling
        }
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

    private Transform FindClosestNPC()
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

    private Transform FindNPCToFleeFrom()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");

        Transform npcToFleeFrom = null;
        float highestHealthDifference = 0f;

        foreach (GameObject npc in npcs)
        {
            if (npc != this.gameObject)
            {
                float distance = Vector2.Distance(transform.position, npc.transform.position);
                NPC otherNPC = npc.GetComponent<NPC>();

                if (distance <= attackRange && otherNPC.health > health)
                {
                    float healthDifference = otherNPC.health - health;

                    if (healthDifference > highestHealthDifference)
                    {
                        highestHealthDifference = healthDifference;
                        npcToFleeFrom = npc.transform;
                    }
                }
            }
        }

        return npcToFleeFrom;
    }

    private bool CanChase()
    {
        Transform closestNPC = FindClosestNPC();
        return closestNPC != null;
    }

    private bool CanAttack(Vector2 targetPosition)
    {
        return Vector2.Distance(transform.position, targetPosition) <= attackRange && Time.time - lastAttackTime >= attackCooldown;
    }
}
