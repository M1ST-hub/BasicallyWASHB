using UnityEngine;

public class Target : MonoBehaviour
{
    public int health;
    public HealthBar healthBar; // Reference to the HealthBar script
    public int maxHealth = 100; // Maximum health
    public GameManager gm;


    private void OnEnable()
    {
        // Ensure healthBar is assigned when player/healthbar exists
        if (healthBar == null)
        {
            healthBar = FindFirstObjectByType<HealthBar>();
        }

        // If healthBar is still null here, log an error (optional)
        if (healthBar == null)
        {
            Debug.LogError("HealthBar is not assigned or found!");
        }
    }

    public void Start()
    {
        // GameManager reference (assumes GameManager exists in the scene)
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Ensure health is initialized correctly
        health = maxHealth;

        // Make sure healthBar is found/assigned before using it
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(health);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth); // Clamp health between 0 and max
        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gm.Respawn();
    }
}
