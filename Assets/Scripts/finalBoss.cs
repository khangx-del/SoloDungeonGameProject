using UnityEngine;
using System.Collections;

public class FinalBoss : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource; // Kéo AudioSource của Boss vào đây
    public AudioClip dashSound;     // Kéo file nhạc lướt vào
    public AudioClip meleeSound;    // Kéo file nhạc chém vào
    public AudioClip deathSound;    // Kéo file nhạc khi chết vào
    [Header("Collider Settings")]
    public float normalColliderRadius = 0.5f;
    public float dashColliderRadius = 1.2f;
    private CircleCollider2D bossCollider;

    [Header("Movement Settings")]
    public float speed = 3f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.4f;
    public float attackCooldown = 3f;
    public float windUpTime = 0.6f;
    public int DashdamageAmount = 20;

    [Header("Melee Settings")]
    public float meleeRange = 2.0f;
    public float meleeCooldown = 1.0f;
    public int attackDamageAmount = 10;

    [Header("Phase Settings")]
    public bool isPhase2 = false;
    public bool isFinnalPhase = false;
    private bool phase2Triggered = false;
    private bool finalPhaseTriggered = false;

    [Header("References")]
    public AnimatedSpriteRenderer right;
    public AnimatedSpriteRenderer death;
    public Animation attack; 

    private Rigidbody2D rb;
    private Transform player;
    private FinalbossHealth bossHealth;
    private SpriteRenderer rightSR;
    private SpriteRenderer deathSR;
    private SpriteRenderer attackSR;

    public bool isDead = false;
    public bool isDashing = false;
    private bool isAttacking = false; 
    private float nextAttackTime;
    private float nextMeleeTime;
    private Vector2 lastDirection = Vector2.down;
    public AudioClip bossMusic; // Kéo file nhạc Dungeon Boss vào đây
    private bool musicChanged = false;
    
    [HideInInspector] public bool isChasing = false;

    void Awake()
    {
        bossCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        bossHealth = GetComponent<FinalbossHealth>();
        rightSR = right.GetComponent<SpriteRenderer>();
        deathSR = death.GetComponent<SpriteRenderer>();
        attackSR = attack.GetComponent<SpriteRenderer>(); 
        
        if (bossCollider != null) bossCollider.radius = normalColliderRadius;
        
        death.enabled = false;
        attack.enabled = false;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
        nextAttackTime = Time.time + attackCooldown;
    }

    void FixedUpdate()
    {
        if (isDead || isDashing || player == null) return;

        if (bossHealth != null && bossHealth.isStunned)
        {
            AllSpriteOff();
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!isChasing)
        {
            Idle();
            return;
        }

        UpdatePhases();

        // ƯU TIÊN 1: DASH (Hủy đòn đánh thường nếu có Dash)
        if (Time.time >= nextAttackTime)
        {
            if (isAttacking) {
                StopAllCoroutines(); // Dừng MeleeAttackRoutine
                isAttacking = false;
            }
            StartCoroutine(DashAttackRoutine());
            return;
        }

        // Nếu đang trong hoạt ảnh đánh thường thì không chạy tiếp logic dưới
        if (isAttacking) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ƯU TIÊN 2: MELEE
        if (distanceToPlayer <= meleeRange)
        {
            rb.linearVelocity = Vector2.zero;
            if (Time.time >= nextMeleeTime) StartCoroutine(MeleeAttackRoutine());
            else Idle();
            return;
        }

        // ƯU TIÊN 3: DI CHUYỂN
        Vector2 dir = (player.position - transform.position).normalized;
        lastDirection = dir;
        rb.linearVelocity = dir * speed;
        UpdateSprite(dir);

        if (isChasing && !musicChanged)
        {
            musicChanged = true;
            if (MusicManager.instance != null && bossMusic != null)
            {
            MusicManager.instance.SwitchTrack(bossMusic);
            }
        }
    }

    void UpdatePhases()
    {
        if (isPhase2 && !phase2Triggered)
        {
            phase2Triggered = true;
            speed = 4f;
            attackDamageAmount = 80;
            DashdamageAmount = 80;
            attackCooldown = 2.5f; 
            windUpTime = 0.3f;
            dashDuration = 0.4f;
            dashSpeed += 2f;
        }
        if (isFinnalPhase && !finalPhaseTriggered)
        {
            finalPhaseTriggered = true;
            speed = 3f;
            DashdamageAmount = 50;
            dashSpeed += 5f;
            windUpTime = 0.2f;
            attackCooldown = 1.5f;
            dashDuration = 0.5f;
            attackDamageAmount = 100;
        }
    }

    IEnumerator DashAttackRoutine()
    {
        isDashing = true;
        
        // Final Phase lướt 5 lần, Phase 2 lướt 3 lần, Phase 1 lướt 1 lần
        int dashCount = isFinnalPhase ? 5 : (isPhase2 ? 3 : 1);
        Collider2D playerCollider = player.GetComponent<Collider2D>();

        for (int i = 0; i < dashCount; i++)
        {
            rb.linearVelocity = Vector2.zero;
            AllSpriteOff();
            right.enabled = true;
            right.idle = true; 

            // Đổi màu để Player biết đang ở Final Phase (Màu Đen hoặc Trắng sáng)
            if (isFinnalPhase) rightSR.color = Color.black; 
            else if (isPhase2) rightSR.color = Color.magenta;
            else rightSR.color = Color.red;

            // Chốt vị trí Player trước khi lướt (Nerf Aim)
            Vector2 targetPos = player.position;
            Vector2 lookDir = (targetPos - (Vector2)transform.position).normalized;
            rightSR.flipX = lookDir.x < 0;

            yield return new WaitForSeconds(windUpTime);

            PlayBossSound(dashSound,0.8f);

            // BẮT ĐẦU DASH
            if (bossCollider != null) bossCollider.radius = dashColliderRadius;
            if (playerCollider != null) Physics2D.IgnoreCollision(bossCollider, playerCollider, true);

            AllSpriteOff();
            attack.enabled = true;
            attack.idle = false; 
            attackSR.flipX = lookDir.x < 0;
            attackSR.color = rightSR.color;

            Vector2 dashDir = (targetPos - (Vector2)transform.position).normalized;
            rb.linearVelocity = dashDir * dashSpeed;
            rb.WakeUp();

            float timer = 0;
            bool hitPlayerThisDash = false;
            while (timer < dashDuration)
            {
                timer += Time.deltaTime;
                if (!hitPlayerThisDash)
                {
                    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, dashColliderRadius);
                    foreach (var hit in hits)
                    {
                        if (hit.CompareTag("Player"))
                        {
                            hit.GetComponent<PlayerHealth>()?.TakeDamage(DashdamageAmount);
                            hitPlayerThisDash = true;
                            break;
                        }
                    }
                }
                yield return null;
            }

            // RESET SAU MỖI LẦN LƯỚT TRONG CHUỖI
            if (bossCollider != null) bossCollider.radius = normalColliderRadius;
            if (playerCollider != null) Physics2D.IgnoreCollision(bossCollider, playerCollider, false);

            rb.linearVelocity = Vector2.zero;
            // Nghỉ giữa các lần lướt ngắn hơn ở Final Phase
            float pauseTime = isFinnalPhase ? 0.1f : 0.2f;
            yield return new WaitForSeconds(pauseTime);
        }

        rightSR.color = Color.white;
        attackSR.color = Color.white;
        isDashing = false;
        nextAttackTime = Time.time + attackCooldown;
    }
    
    void PlayBossSound(AudioClip clip, float baseVolume = 1f)
{
    if (audioSource == null || clip == null) return;

    // Mặc định Phase 1
    float pitch = 1.0f;
    float volumeMultiplier = 1.0f;

    // Tăng uy lực theo Phase
    if (isFinnalPhase) 
    {
        pitch = 0.8f; // Trầm hơn nghe sẽ nặng nề và nguy hiểm hơn
        volumeMultiplier = 1.4f; // To hơn
    }
    else if (isPhase2)
    {
        pitch = 0.9f; 
        volumeMultiplier = 1.2f;
    }

    audioSource.pitch = pitch + Random.Range(-0.05f, 0.05f); // Thêm chút ngẫu nhiên để không bị chán
    audioSource.PlayOneShot(clip, baseVolume * volumeMultiplier);
}

    IEnumerator MeleeAttackRoutine()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        PlayBossSound(meleeSound,1.0f);
        
        AllSpriteOff();
        attack.enabled = true;
        attack.idle = false;
        attackSR.flipX = (player.position.x - transform.position.x) < 0;

        yield return new WaitForSeconds(0.1f); 

        if (player != null && Vector2.Distance(transform.position, player.position) <= meleeRange + 0.5f)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamageAmount);
        }

        yield return new WaitForSeconds(0.3f); 
        
        isAttacking = false;
        nextMeleeTime = Time.time + meleeCooldown;
    }

    void AllSpriteOff() 
    { 
        right.enabled = false; 
        death.enabled = false; 
        attack.enabled = false; 
        right.idle = false; 
    }

    void Idle() 
    { 
        rb.linearVelocity = Vector2.zero; 
        AllSpriteOff();
        right.enabled = true;
        right.idle = true; 
        rightSR.flipX = lastDirection.x < 0; 
    }

    void UpdateSprite(Vector2 dir) 
    { 
        AllSpriteOff(); 
        right.enabled = true; 
        right.idle = false; 
        if (Mathf.Abs(dir.x) > 0.1f) lastDirection.x = dir.x; 
        rightSR.flipX = lastDirection.x < 0; 
    }

    public void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        if (bossCollider != null) bossCollider.enabled = false; 
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; 
        StopAllCoroutines(); 
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        if (MusicManager.instance != null)
        {
            // Nhạc tắt dần trong 4 giây để tạo sự trang trọng
            MusicManager.instance.FadeOutToSilence(4.0f);
        }
           PlayBossSound(deathSound, 1.0f);
        AllSpriteOff();
        if (death != null)
        {
            deathSR.flipX = lastDirection.x < 0;
            death.enabled = true;
            death.idle = false;
        }
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}