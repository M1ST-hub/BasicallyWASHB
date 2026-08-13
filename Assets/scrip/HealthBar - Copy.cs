using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health value
    public int currentHealth = 100; // Current health value
    public Image fillImage; // Optional: If you're using a filled image instead of a slider

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public void SetHealth(int health)
    {
        this.currentHealth = health;
        // Update the fill image, if using it
        if (fillImage != null)
        {
            fillImage.fillAmount = (float)currentHealth / (float)maxHealth;
        }
    }
}