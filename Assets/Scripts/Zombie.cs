using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    [Header("Attack")]
    public float attackRange = 1.2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.2f;
    float nextAttackTime;

    [Header("Movement")]
    private Transform player;
    public float speed = 3f;

    [Header("Sprites")]

    public AnimatedSpriteRenderer right;
    public AnimatedSpriteRenderer death;
    Vector2 lastDirection = Vector2.down;

    Rigidbody2D rb;
    bool isDead = false;
     public DropExp dropExp;

    [HideInInspector] public bool isChasing = false;
    private ZombieHealth zombieHealth;
    SpriteRenderer rightSR;
    SpriteRenderer deathSR;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        DisableAllSprites();

        zombieHealth = GetComponent<ZombieHealth>();

        rightSR = right.GetComponent<SpriteRenderer>();
        deathSR = death.GetComponent<SpriteRenderer>();

        SetDark(0.2f);
    }

    
    void FixedUpdate()
    {
        if (zombieHealth != null && zombieHealth.isStunned)
        {
            rb.linearVelocity = Vector2.zero;
            Idle();
            return; 
        }      
      
        if (isDead) return;

        if (!isChasing || player == null)
        {
            Idle();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 dir = (player.position - transform.position).normalized;

        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            Idle(); 
            TryAttack();
            return;
        }
   
        lastDirection = dir;
        rb.linearVelocity = dir * speed;

        UpdateSprite(dir);
    }
    

    void Idle()
    {
        rb.linearVelocity = Vector2.zero;

        DisableAllSprites();
       
        right.enabled = true;
        right.idle = true;
        right.GetComponent<SpriteRenderer>().flipX = lastDirection.x < 0;
        
    }
     
    void UpdateSprite(Vector2 dir)
    {
        DisableAllSprites();

        right.enabled = true;
        right.idle = false;

    
        if (Mathf.Abs(dir.x) > 0.1f)
        {
            lastDirection.x = dir.x;
        }

        right.GetComponent<SpriteRenderer>().flipX = lastDirection.x < 0;
    }
       
    

    void DisableAllSprites()
    {
        right.enabled = death.enabled = false;

        right.idle = false;;
    }

    void TryAttack()
    {
        if (player == null || Time.time < nextAttackTime) return;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            var ph = player.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(attackDamage);

            nextAttackTime = Time.time + attackCooldown;
        }
    }
    void SetDark(float value)
    {
        Color c = new Color(value, value, value);

        rightSR.color = c;
        deathSR.color = c;
    }

    public void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(Death());
        var keySpawn = GetComponent<KeySpam>();
        if (keySpawn != null)
        {
            keySpawn.OnDeath();
        }
    }

    IEnumerator Death()
    {
        DisableAllSprites();
        death.enabled = true;

        if (Mathf.Abs(lastDirection.x) > 0)
        {
            death.GetComponent<SpriteRenderer>().flipX = lastDirection.x < 0;
        }
     
        rb.linearVelocity = Vector2.zero;
        dropExp.DropExperience();

        yield return new WaitForSeconds(1.5f);
        
        Destroy(gameObject);
    }
}


