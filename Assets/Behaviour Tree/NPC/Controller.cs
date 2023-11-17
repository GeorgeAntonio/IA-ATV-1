using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour { 
    public float moveSpeed = 5f;
    public float chaseRange = 20f;
    public float attackRange = 0.5f;
    public int health = 100;
    public int maxHealth = 100;
    public int attackDamage = 10;
    public int healthPotionValue = 50;
    public float attackCooldown = 2f;
    public float knockbackForce = 2f;
    public float patrolCircleRadius = 10f;
    public float stunDuration = 0.3f; // Stun duration after knockback
    public float fleeHealthThreshold = 0.2f;
    public bool isHealing;

    public Transform patrolPoint, npcTarget, selectedPotion;    
    public Rigidbody2D rb;
    public GameObject mainCamera;
    public NPCSpawner spawner;
    public Vector2 randomWanderTarget;
    public float lastAttackTime = 0f;
    public float stunEndTime = 0f; // Time when the stun ends
    public Vector2 moveDirection;
    public List<PatrolPoint> patrolPoints;

    public struct PatrolPoint
    {
        public bool ocuppied;
        public Transform position;

        public PatrolPoint(Transform position, bool ocuppied)
        {
            this.position = position;
            this.ocuppied = ocuppied;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HealthPotion"))
        {
            isHealing = true;
            selectedPotion = collision.gameObject.transform;
        }
    }

    private void Start()
    {         
        patrolPoints = new List<PatrolPoint> ();
        SetPatrolPoints();
        rb = GetComponent<Rigidbody2D>();
        patrolPoint = GetRandomPatrolPoint();

        moveSpeed = Random.Range(5f, 8f);
        chaseRange = Random.Range(7f, 15f);
        attackRange = Random.Range(.5f, .7f);
        health = Random.Range(50, 130);
        maxHealth = Random.Range(50, 130);
        attackDamage = Random.Range(10, 30);
        attackCooldown = Random.Range(1.5f, 3.5f);
        knockbackForce = Random.Range(3f, 7f);
        stunDuration = Random.Range(0.3f, 1f);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        spawner = GameObject.FindGameObjectWithTag("NPCspawner").GetComponent<NPCSpawner>();
        isHealing = false;
    }

    private void SetPatrolPoints()
    {
       patrolPoints.Clear();
       GameObject[] patrolPointsG = GameObject.FindGameObjectsWithTag("PatrolPoint");
       foreach(GameObject patrolPoint in patrolPointsG)
        {
            patrolPoints.Add(new PatrolPoint(patrolPoint.transform, false));
        }         
    }

    private Transform GetRandomPatrolPoint()
    {        
        List<PatrolPoint> points = new List<PatrolPoint>();
        points.AddRange(patrolPoints);
        if (points.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, points.Count);
        while (points.Count > 0 && points[randomIndex].ocuppied == true) {
            randomIndex = Random.Range(0, points.Count);
            points.RemoveAt(randomIndex);
        }        
        for(int x = 0; x < patrolPoints.Count;x++)
        {
            if (patrolPoints[x].position == points[randomIndex].position)
            {
                patrolPoints[x] = new PatrolPoint(patrolPoints[x].position, true);
                break;
            }
        }
        return points[randomIndex].position;
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
            Debug.Log(npc.ToString());
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

    public void Heal(int value, Transform potion)
    {
        if (health >= 100)
        {
            if (health + value > 100)
            {
                health = 100;
            }
            else health += value;
            Destroy(potion.gameObject);
            isHealing = false;
        }
    }
    private void HandleStun()
    {
        // Check if the stun duration has elapsed
        if (Time.time >= stunEndTime)
        {
          
        }
    }
}