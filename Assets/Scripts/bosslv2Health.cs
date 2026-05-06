using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class bosslv2Health : MonoBehaviour
{
    [Header("Regeneration")]
    public float healAmount = 1f;      // hồi bao nhiêu máu
    public float healInterval = 1f;
    float lastDamageTime;
    bool isHealing;
     [Header("Health")]
    public int maxHealth = 450;
    public int currentHealth;
    public Healthbar healthbar;

    public bool isStunned;
    public float stunTime = 0.15f;
    
    private Rigidbody2D _rb;
    private BossLv2 bossLv2;
    public GameObject hitFlashPrefab;
    public bool phase2 = false;
    public bool finalPhase = false;
    public GameObject Phase2Menu;
    public GameObject FinalPhaseMenu;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        bossLv2 = GetComponent<BossLv2>();
        Phase2Menu.SetActive(false);
        FinalPhaseMenu.SetActive(false);
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthbar.setMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {  
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        healthbar.setHealth(currentHealth);

        lastDamageTime = Time.time;

        if (!isHealing)
        { 
            isHealing = true;
            StartCoroutine(AutoHeal());
       
        }

        if (hitFlashPrefab != null && currentHealth > 0)
        {
            Instantiate(hitFlashPrefab, transform.position, Quaternion.identity, transform);
        }
        if (currentHealth < 300 && !phase2)
        {
            ShowPhase2Menu();
            phase2 = true;
        }
        if (currentHealth < 100 && !finalPhase)
        {
            ShowFinalPhaseMenu();
            finalPhase = true;
        }
        
        if (currentHealth <= 0)
        {        
            Die();
        }
        
    }

    void Die()
    {
        if (bossLv2 != null)
        {
            isHealing = false; // Dừng việc hồi máu khi chết
            StopAllCoroutines(); // Dừng tất cả các Coroutine đang chạy
            bossLv2.OnDeath();
        }
    }
    IEnumerator AutoHeal()
    {
        while (currentHealth > 0)
        {
        yield return new WaitForSeconds(healInterval);
            if (Time.time - lastDamageTime >= 4f) // Nếu đã 4 giây kể từ lần cuối bị tấn công
            {
                if (currentHealth < maxHealth)
                {
                    currentHealth += (int)healAmount;

                    if (currentHealth > maxHealth)
                    {
                        currentHealth = maxHealth;
                    }
              
                healthbar.setHealth(currentHealth);
                }
            }     
        }
    }
    void ShowPhase2Menu()
    {
        StartCoroutine(ShowPhase2MenuCoroutine());
    }
     IEnumerator ShowPhase2MenuCoroutine()
    {
        Phase2Menu.SetActive(true); // Show the phase 2 menu
        
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        Phase2Menu.SetActive(false); // Hide the phase 2 menu after 3 seconds
    }
    
    void ShowFinalPhaseMenu()
    {
        StartCoroutine(ShowFinalPhaseMenuCoroutine());
    }
    IEnumerator ShowFinalPhaseMenuCoroutine()
    {
        FinalPhaseMenu.SetActive(true); // Show the final phase menu
        
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        FinalPhaseMenu.SetActive(false); // Hide the final phase menu after 3 seconds
    }
}
