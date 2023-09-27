using UnityEngine;

public class NPC : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float attackRange = 1.5f; // Range within which the NPC can attack
    public int health = 100;
    public int attackDamage = 10;
    public float attackCooldown = 2f; // Cooldown between attacks

    private Transform patrolPoint;
    private Rigidbody2D rb;
    private enum State { Patrol, Chase, Attack }
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
                // Perform the attack action here
                // For simplicity, we'll just reduce the health of the target NPC
                NPC targetNPC = npcToAttack.GetComponent<NPC>();
                targetNPC.health -= attackDamage;

                // Add attack cooldown to prevent continuous attacks
                lastAttackTime = Time.time;

                if (targetNPC.health <= 0)
                {
                    currentState = State.Patrol; // If the target NPC is defeated, go back to patrolling
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
