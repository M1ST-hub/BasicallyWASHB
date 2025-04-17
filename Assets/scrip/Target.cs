using UnityEngine;

public class Target : MonoBehaviour
{
    public int health;
    public HealthBar healthBar; // Reference to the HealthBar script
    public int maxHealth = 100; // Maximum health
    public GameManager gm;

    public void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth); // Clamp health between 0 and max
        healthBar.SetHealth(health);
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
