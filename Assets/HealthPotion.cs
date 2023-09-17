using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public float healthBoost = 30.0f; // Amount of health the potion restores.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // If the enemy touches the health potion, increase its health.
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.IncreaseHealth(healthBoost);
            }

            // Destroy the health potion after it's picked up.
            Destroy(gameObject);
        }
    }

    // You can add more functionality to the health potion if needed.
}
