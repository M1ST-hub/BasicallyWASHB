using Unity.Netcode;
using UnityEngine;
using static GameManager;

public class Target : NetworkBehaviour
{
    public int health;
    public HealthBar healthBar; // Reference to the HealthBar script
    public int maxHealth = 100; // Maximum health
    public GameManager gm;
    public GameMode currentGameMode;
    public TeamColor currentTeamColor;


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

    [Rpc(SendTo.Everyone)]
    public void TakeDamageRpc(int amount)
    {

        if (Timer.gameStart == false)
        {
            return; // Ignore damage if the game hasn't started
        }

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


    
    public void Die()
    {
         Respawn(this.gameObject);
    }
}
