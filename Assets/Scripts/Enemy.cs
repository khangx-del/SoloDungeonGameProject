using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
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
    public AnimatedSpriteRenderer up;
    public AnimatedSpriteRenderer down;
    public AnimatedSpriteRenderer right;
    public AnimatedSpriteRenderer death;
    Vector2 lastDirection = Vector2.down;
    public DropExp dropExp;

    Rigidbody2D rb;
    bool isDead = false;

    [HideInInspector] public bool isChasing = false;
    private EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DisableAllSprites();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    
    void FixedUpdate()
    {
        if (enemyHealth != null && enemyHealth.isStunned)
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

        if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
        {
            right.enabled = true;
            right.idle = true;
            right.GetComponent<SpriteRenderer>().flipX = lastDirection.x < 0;
        }
        else
        {
            if (lastDirection.y > 0)
            {
            up.enabled = true;
            up.idle = true;
            }
            else
            {
            down.enabled = true;
            down.idle = true;
            }
        };
    }

    void UpdateSprite(Vector2 dir)
    {
        DisableAllSprites();

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            right.enabled = true;
            right.idle = false;
            right.GetComponent<SpriteRenderer>().flipX = dir.x < 0;
        }
        else
        {
            if (dir.y > 0)
            {
                up.enabled = true;
                up.idle = false;
            }
            else
            {
                down.enabled = true;
                down.idle = false;
            }
        }
    }

    void DisableAllSprites()
    {
        up.enabled = down.enabled = right.enabled = death.enabled = false;

        up.idle = down.idle = right.idle = false;;
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
        
        rb.linearVelocity = Vector2.zero;

        dropExp.DropExperience();

        yield return new WaitForSeconds(1.5f);
        
        Destroy(gameObject);
    }
}