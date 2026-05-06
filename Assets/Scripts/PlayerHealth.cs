using System.Collections;
using NUnit.Framework;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    [Header("Regeneration")]
    public float healAmount = 1f;      // hồi bao nhiêu máu
    public float healInterval = 1f;    // mỗi bao lâu hồi
    bool isDead = false;
    [Header("Health")   ]
    public int maxHealth = 100;
    public int currentHealth;
    public Healthbar healthbar;

    public GameObject DeadMenu;
    
    void Start()
    {
        DeadMenu.SetActive(false);
        currentHealth = maxHealth;
        StartCoroutine(AutoHeal());
        healthbar.setMaxHealth(maxHealth);
        ApplyUpgrades();
    }
    void ApplyUpgrades()
    {
        healAmount += GameData.autoHealLevel * 1f;
        maxHealth += GameData.hpLevel * 10;
        currentHealth = maxHealth;
        healthbar.setMaxHealth(maxHealth);
        healthbar.setHealth(currentHealth);
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    
        Debug.Log("Player hit! HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        healthbar.setHealth(currentHealth);

    }
    public void AddMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        healthbar.setMaxHealth(maxHealth);
        healthbar.setHealth(currentHealth);
    }
    
    void Die()
    {
        isDead = true;
        Debug.Log("Player died");
        gameObject.SetActive(false);
        DeadMenu.SetActive(true);
    }
    IEnumerator AutoHeal()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(healInterval);

            if (currentHealth > 0 && currentHealth < maxHealth)
            {
                currentHealth += Mathf.RoundToInt(healAmount);

                if (currentHealth > maxHealth)
                {
                currentHealth = maxHealth;
                }
              
                healthbar.setHealth(currentHealth);
            }
        }
    }
    public void UpdateHealthBar()
    {
    healthbar.setMaxHealth(maxHealth);
    healthbar.setHealth(currentHealth);
    }
   
}
