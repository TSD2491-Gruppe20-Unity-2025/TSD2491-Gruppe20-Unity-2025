using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    //-----------------------------------------------------------------------------//
    // Inspector References

    [SerializeField] private Image healthBarFill;

    //-----------------------------------------------------------------------------//
    // Private Fields

    private int maxHealth;

    //-----------------------------------------------------------------------------//
    // Initialization

    public void Initialize(int max)
    {
        maxHealth = max;
        SetHealth(max); // Set full health initially
    }

    //-----------------------------------------------------------------------------//
    // UI Update

    public void SetHealth(int currentHealth)
    {
        if (healthBarFill == null || maxHealth <= 0)
        {
            Debug.LogWarning("UIScript: Health bar fill not set or maxHealth is invalid.");
            return;
        }

        float fill = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = fill;
        Debug.Log($"[UI] Health updated: {currentHealth}/{maxHealth} → fill: {fill}");
    }
}