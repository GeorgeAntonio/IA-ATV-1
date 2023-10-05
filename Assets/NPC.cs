using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class NPC : MonoBehaviour
{
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

    private Transform patrolPoint;
    private Rigidbody2D rb;
    private enum State { Patrol, Chase, Attack, Flee, Stunned }
    private State currentState = State.Patrol;
    private Vector2 randomWanderTarget;
    private float lastAttackTime = 0f;
    private float stunEndTime = 0f; // Time when the stun ends
    private GameObject mainCamera;
    private NPCSpawner spawner;
     public float fleeHealthThreshold = 0.2f;

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
            case State.Stunned:
                HandleStun();
                break;
        }
        Debug.Log(currentState.ToString());
    }

    private void Patrol()
    {
        if (patrolPoint != null)
        {
            Vector2 circleDirection = (patrolPoint.position - transform.position).normalized;
            Vector2 offset = new Vector2(-circleDirection.y, circleDirection.x) * patrolCircleRadius;
            Vector2 targetPosition = (Vector2)patrolPoint.position + offset;

            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

            if (!IsStunned())
            {
                rb.velocity = moveDirection * moveSpeed;
            }

            if (CanChase())
            {
                currentState = State.Chase;
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

        if (!IsStunned())
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    private void Chase()
    {
        Transform npcToChase = FindClosestNPC();

        if (npcToChase != null)
        {
            Vector2 moveDirection = (npcToChase.position - transform.position).normalized;

            if (!IsStunned())
            {
                rb.velocity = moveDirection * moveSpeed;
            }

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
                if (health / (float)maxHealth <= fleeHealthThreshold){
                    currentState = State.Flee;
                    Debug.Log("fugindo");
                }
                Vector2 knockbackDirection = (transform.position - npcToAttack.position).normalized;
                Rigidbody2D targetRB = npcToAttack.GetComponent<Rigidbody2D>();

                if (targetRB != null)
                {
                    targetRB.velocity = knockbackDirection * knockbackForce;
                }

                NPC targetNPC = npcToAttack.GetComponent<NPC>();
                targetNPC.health -= attackDamage;

                lastAttackTime = Time.time;

                if (targetNPC.health <= 0)
                {
                    if(npcToAttack.gameObject == mainCamera.GetComponent<CameraFollow>().target.gameObject) { spawner.isTargetDead = true; }
                    Destroy(npcToAttack.gameObject);
                }

                currentState = State.Chase;
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

            // Check for the nearest health potion
            GameObject[] healthPotions = GameObject.FindGameObjectsWithTag("HealthPotion");
            Transform nearestPotion = null;
            float nearestPotionDistance = float.MaxValue;

            foreach (GameObject potion in healthPotions)
            {
                float distanceToPotion = Vector2.Distance(transform.position, potion.transform.position);

                if (distanceToPotion < nearestPotionDistance)
                {
                    nearestPotion = potion.transform;
                    nearestPotionDistance = distanceToPotion;
                }
            }

            // If a nearest potion is found, move towards it
            if (nearestPotion != null)
            {
                Vector2 potionDirection = (nearestPotion.position - transform.position).normalized;

                // Compare distances to decide whether to flee or go for the potion
                if (nearestPotionDistance < Vector2.Distance(transform.position, npcToFleeFrom.position))
                {
                    fleeDirection = potionDirection;
                }
            }

            if (!IsStunned())
            {
                rb.velocity = fleeDirection * moveSpeed;
            }

            if (Vector2.Distance(transform.position, npcToFleeFrom.position) > chaseRange)
            {
                currentState = State.Patrol;
            }
        }
        else
        {
            currentState = State.Patrol;
        }
    }


    private void HandleStun()
    {
        // Check if the stun duration has elapsed
        if (Time.time >= stunEndTime)
        {
            // Stun has ended, transition back to the previous state
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

    private bool IsStunned()
    {
        return Time.time < stunEndTime;
    }

    public void Heal(int value, Collision2D potion)
    {
        if (health >= 100)
        {
            if (health + value > 100)
            {
                health = 100;
            }
            else health += value;
            Destroy(potion.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.velocity = knockbackDirection * knockbackForce;
            currentState = State.Stunned; // Enter the Stunned state
            stunEndTime = Time.time + stunDuration; // Set the stun end time
        }
        if (collision.gameObject.CompareTag("HealthPotion"))
        {            
            Heal(30, collision);
        }
    }


    public static event System.Action<Vector3> OnNPCDestroyed;

    private void OnDestroy()
    {
        // Raise the event to notify that this NPC is destroyed
        OnNPCDestroyed?.Invoke(transform.position);
    }
}
