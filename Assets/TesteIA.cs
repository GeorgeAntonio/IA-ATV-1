/*using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f; // Speed at which the enemy moves towards the player.
    public float patrolSpeed = 1.0f; // Speed at which the enemy patrols.
    public float chaseDistance = 10.0f; // Distance at which the enemy starts chasing the player.
    public float attackDistance = 2.0f; // Distance at which the enemy can attack the player.
    public float attackCooldown = 2.0f; // Cooldown between attacks.
    public float maxHealth = 100.0f; // Maximum health of the enemy.
    public float healthRegenerationRate = 5.0f; // Health regeneration rate per second when not chasing.
    public float searchRadius = 10.0f; // Radius within which the enemy searches for health potions.
    public LayerMask healthPotionLayer; // Layer mask for health potions.
    
    private Transform player; // Reference to the player's Transform.
    private Vector3 spawnPoint; // The enemy's spawn point.
    private bool isChasing = false; // Flag to determine if the enemy is chasing the player.
    private float lastAttackTime = 0.0f; // Time of the last attack.
    private float currentHealth; // Current health of the enemy.

    private void Start()
    {
        // Find the player's GameObject using the "Player" tag.
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player has the 'Player' tag.");
        }

        // Store the initial spawn point of the enemy.
        spawnPoint = transform.position;

        // Initialize the enemy's health to its maximum.
        currentHealth = maxHealth;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentHealth < maxHealth && !isChasing)
        {
            // Regenerate health when not chasing the player.
            currentHealth += healthRegenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        if (isChasing)
        {
            // Calculate the direction vector from the enemy to the player.
            Vector3 direction = (player.position - transform.position).normalized;

            // Move the enemy towards the player.
            transform.Translate(direction * speed * Time.deltaTime);

            // Check if the player is within attack distance.
            if (distanceToPlayer <= attackDistance && Time.time - lastAttackTime >= attackCooldown)
            {
                // Player is in attack range and enough time has passed since the last attack.
                Attack();
                lastAttackTime = Time.time;
            }

            // Check if the enemy has less health than the player.
            if (currentHealth < player.GetComponent<PlayerHealth>().GetCurrentHealth())
            {
                // Enemy has less health, run away from the player.
                RunAway();
            }

            // Check if the player is too far away to chase.
            if (distanceToPlayer > chaseDistance)
            {
                // Player is out of range, stop chasing and return to patrol.
                isChasing = false;
            }
        }
        else
        {
            // If not chasing, patrol back to the spawn point.
            Patrol();
            
            // Check if the player is within chase distance to resume chasing.
            if (distanceToPlayer <= chaseDistance)
            {
                isChasing = true;
            }
            
            // Check if the enemy's health is below a certain threshold and it's not chasing.
            if (currentHealth < maxHealth * 0.5f)
            {
                // Search for a health potion within the search radius.
                SearchForHealthPotion();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // When the player enters the enemy's trigger area, start chasing the player.
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // When the player exits the trigger area, stop chasing the player.
            isChasing = false;
        }
    }

    private void Patrol()
    {
        // Calculate the direction vector from the enemy to the spawn point.
        Vector3 direction = (spawnPoint - transform.position).normalized;

        // Move the enemy towards the spawn point using the patrol speed.
        transform.Translate(direction * patrolSpeed * Time.deltaTime);

        // If the enemy reaches or gets close to the spawn point, reset its position.
        if (Vector3.Distance(transform.position, spawnPoint) < 0.1f)
        {
            transform.position = spawnPoint;
        }
    }

    private void Attack()
    {
        // Implement your attack logic here.
        // This could include dealing damage to the player or triggering animations.
        Debug.Log("Enemy attacks!");

        // Example: You can deal damage to the player here if you have a health system.
        // player.GetComponent<PlayerHealth>().TakeDamage(10);
    }

    private void RunAway()
    {
        // Calculate the direction vector away from the player.
        Vector3 direction = (transform.position - player.position).normalized;

        // Move the enemy away from the player.
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void SearchForHealthPotion()
    {
        // Find all nearby objects with the healthPotionLayer within the search radius.
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, healthPotionLayer);

        // Iterate through the found objects.
        foreach (Collider2D hitCollider in hitColliders)
        {
            // You can check the object's tag or use a specific script to identify health potions.
            // For simplicity, we assume that health potions have a "HealthPotion" tag.

            if (hitCollider.CompareTag("HealthPotion"))
            {
                // Found a health potion. Implement logic to pick it up and regain health.
                // Example: healthRegenerationRate += healthPotion.GetComponent<HealthPotion>().GetHealthBoost();
                
                // Destroy the health potion to prevent multiple pickups.
                Destroy(hitCollider.gameObject);
                
                // Exit the search loop after finding a health potion.
                break;
            }
        }
    }
}
*/