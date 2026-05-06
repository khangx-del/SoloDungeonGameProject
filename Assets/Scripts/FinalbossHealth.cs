using UnityEngine;
using System.Collections;
using TMPro;
using NUnit.Framework.Constraints;

public class FinalbossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1500;
    public int currentHealth;
    public Healthbar healthbar;

    [Header("Heal Settings")]
    public float healAmount = 10f;
    public float healInterval = 1f;
    private bool isHealing = false;

    [Header("Status")]
    public bool isStunned;
    public float stunTime = 0.15f;

    private Rigidbody2D _rb;
    private FinalBoss finalboss;
    public TMP_Text finalPhase;
    public TMP_Text Phase2;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        finalboss = GetComponent<FinalBoss>();
        Phase2.enabled = false;
        finalPhase.enabled = false;
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (healthbar != null) healthbar.setMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Don't take damage if already dead

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (healthbar != null) healthbar.setHealth(currentHealth);

        // --- PHASE LOGIC ---
        // Use the cached 'finalboss' reference instead of FindAnyObjectByType
        if (finalboss != null)
        {
            if (currentHealth <= maxHealth * 0.8f && !finalboss.isPhase2)
            {
                ShowPhase2();
                finalboss.isPhase2 = true;
                Debug.Log("Entered Phase 2!");
                healAmount = 1.5f;
                healInterval = 0.05f;
            }

            if (currentHealth <= maxHealth * 0.2f && !finalboss.isFinnalPhase)
            {
                ShowFinalPhase();
                finalboss.isFinnalPhase = true;
                healAmount = 1f;
                healInterval = 0.05f;
                currentHealth = maxHealth -= 900;
                maxHealth = currentHealth;
                Debug.Log("Entered Final Phase!");
                healthbar.setMaxHealth(maxHealth);
                healthbar.setHealth(currentHealth);
            }
        }

        // --- AUTO HEAL TRIGGER ---
        if (!isHealing && currentHealth > 0 && currentHealth < maxHealth)
        {
            StartCoroutine(AutoHeal());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isHealing = false;
        StopAllCoroutines();
        if (finalboss != null)
        {
            finalboss.OnDeath();
        }
    }

    IEnumerator AutoHeal()
    {
        if (finalboss.isDashing == true)
        {
            yield break;
        }
        isHealing = true;
        
        while (currentHealth > 0 && currentHealth < maxHealth)
        {
            yield return new WaitForSeconds(healInterval);

            currentHealth = Mathf.Min(currentHealth + (int)healAmount, maxHealth);
            
            if (healthbar != null) healthbar.setHealth(currentHealth);

            // If we reach max health, stop the loop
            if (currentHealth >= maxHealth) break;
        }

        isHealing = false;
    }
    void ShowPhase2()
    {
        StartCoroutine(Phase2ShowCoroutine());
    }
    IEnumerator Phase2ShowCoroutine()
    {
        Phase2.enabled = true;

        yield return new WaitForSeconds(3f);

        Phase2.enabled = false;
    }
    void ShowFinalPhase()
    {
        StartCoroutine(ShowFinalPhaseCoroutine());
    }
    IEnumerator ShowFinalPhaseCoroutine()
    {
        finalPhase.enabled = true;

        yield return new WaitForSeconds(3f);

        finalPhase. enabled = false;
    }
}