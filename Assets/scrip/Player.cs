using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBar healthBar; // Reference to the HealthBar script
    public int maxHealth = 100; // Maximum health
    public int currentHealth; // Current health

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health between 0 and max
        healthBar.SetHealth(currentHealth);

        // Add death logic here if currentHealth reaches 0
    }
}