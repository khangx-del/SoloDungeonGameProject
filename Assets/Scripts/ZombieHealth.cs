using UnityEngine;
using System.Collections;
public class ZombieHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public Healthbar healthbar;
    
    public bool isStunned;
    public float stunTime = 0.15f;
    
    private Rigidbody2D rb;
    private Zombie zombie;
    public GameObject hitFlashPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        zombie = GetComponent<Zombie>();
       
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
        
        if (zombie != null)
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
        if (zombie != null)
        {
             zombie.OnDeath();
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

