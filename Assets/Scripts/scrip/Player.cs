using Unity.Netcode;
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

    
    public void Respawn(GameObject player)
    {
        Rigidbody rb = player.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            // Stop momentum
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Safe spawn with a slight upward offset to ensure grounded check will work
        Vector3 restartPosition = GameManager.Instance.spawnPoints[Random.Range(0, GameManager.Instance.spawnPoints.Length)].transform.position + Vector3.up * 1f;
        player.transform.position = restartPosition;

        // Reset rotation if needed
        player.transform.rotation = Quaternion.identity;

        Debug.Log("Respawned");
    }
}