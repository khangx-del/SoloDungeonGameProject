using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public Healthbar healthbar;
    
    public bool isStunned;
    public float stunTime = 0.15f;
    
    private Rigidbody2D rb;
    private Enemy enemy;
    public GameObject hitFlashPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
       
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

        if (hitFlashPrefab != null)
        {
            Instantiate(hitFlashPrefab, transform.position, Quaternion.identity, transform);
        }
        
        if (enemy != null)
        {
            StartCoroutine(StunCoroutine());
        }
        
        if (currentHealth <= 0)
        {        
            Die();
        }
        
    }

    void Die()
    {
        if (enemy != null)
        {
             enemy.OnDeath();
        }
    }
    IEnumerator StunCoroutine()
    {
        if (currentHealth <= 0) yield break; // Nếu đã chết thì không cần stun nữa
        isStunned = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }
}