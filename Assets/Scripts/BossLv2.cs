using UnityEngine;
using System.Collections;

public class BossLv2 : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource; // Kéo AudioSource của Boss vào đây
    public AudioClip dashSound;     // Kéo file nhạc lướt vào
    public AudioClip meleeSound;    // Kéo file nhạc chém vào
    public AudioClip deathSound;    
    [Header("Attack")]
    public float attackRange = 1.2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.2f;
    float nextAttackTime;
    [Header("Shoot")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float shootRange = 5f;
    public float fireInterval = 0.1f;   // tốc độ bắn trong 3 giây
    public float fireDuration = 3f;     // thời gian bắn
    public float waitTime = 3f;     
    bool isFiring;    
    bool isFireCycling = false;
    public float rotateSpeed = 120f;
    public float radius = 1f;
    public AudioClip bossMusic; // Kéo file nhạc Dungeon Boss vào đây
    private bool musicChanged = false;
    private float angle;
    [Header("FirePoint2")]
    public Transform firePoint2;

    [Header("Movement")]
    private Transform player;
    public float speed = 3f;

    [Header("Death")]
    public AnimatedSpriteRenderer death;
    public SpriteRenderer spriteRenderer;

    Rigidbody2D rb;
    bool isDead = false;
    public bosslv2Health health;
    bool phase2Applied = false;
    bool finalPhaseApplied = false;
    public DropExp dropExp;
    public AudioClip normalLevelMusic; // Kéo bản nhạc hầm ngục bình thường vào đây


    [HideInInspector] public bool isChasing = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer.enabled = true;

        if (death != null)
        {
            death.enabled = false;
        }
    }

    void FixedUpdate()
{
    if (isDead || player == null || !isChasing) return;

    float distance = Vector2.Distance(transform.position, player.position);
    Vector2 dir = (player.position - transform.position).normalized;

    // 1. Cập nhật vị trí FirePoint LUÔN LUÔN chạy
    angle += rotateSpeed * Time.fixedDeltaTime;
    float rad = angle * Mathf.Deg2Rad;
    float x = Mathf.Cos(rad) * radius * 1.5f;
    float y = Mathf.Sin(rad) * radius;

    Vector3 offset = new Vector3(x, y, 0);
    firePoint.position = transform.position + offset;
    firePoint2.position = transform.position - offset;

    // 2. Kiểm tra chu kỳ bắn súng
    if (distance <= shootRange && !isFireCycling)
    {
        isFireCycling = true;
        StartCoroutine(FireCycle());
    }

    // 3. Logic các Phase của Boss
    UpdateBossPhases();

    // 4. Xử lý di chuyển và Tấn công cận chiến
    if (distance <= attackRange)
    {
        rb.linearVelocity = Vector2.zero;
        TryAttack();
        // Không dùng return ở đây để các logic khác (nếu có) vẫn chạy
    }
    else
    {
        rb.linearVelocity = dir * speed;
    }

    if (isChasing && !musicChanged)
        {
            musicChanged = true;
            if (MusicManager.instance != null && bossMusic != null)
            {
            MusicManager.instance.SwitchTrack(bossMusic);
            }
        }
}

// Tách logic Phase ra cho sạch code
void UpdateBossPhases()
{
    if (health.phase2 && !phase2Applied)
    {
        phase2Applied = true;
        speed += 0.25f;
        fireInterval -= 0.3f;
        shootRange += 0.5f;
        attackDamage += 5;
    }
    if (health.finalPhase && !finalPhaseApplied)
    {
        finalPhaseApplied = true;
        speed += 0.25f;
        attackDamage += 20;
        health.healInterval -= 0.25f;
        health.currentHealth = health.maxHealth - 200;
        health.healAmount += 5;
        bulletSpeed -= 4f;
        fireInterval += 0.3f;
    }
}
void PlayBossSound(AudioClip clip, float baseVolume = 1f)
{
    if (audioSource == null || clip == null) return;

    // Mặc định Phase 1
    float pitch = 1.0f;
    float volumeMultiplier = 1.0f;

    audioSource.pitch = pitch + Random.Range(-0.05f, 0.05f); // Thêm chút ngẫu nhiên để không bị chán
    audioSource.PlayOneShot(clip, baseVolume * volumeMultiplier);
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
        isFiring = false;
        StopAllCoroutines(); 
        GetComponent<Collider2D>().enabled = false;
        spriteRenderer.enabled = false;
        StartCoroutine(Death());
    }

    public void Fire()
    {
        if (isDead || bulletPrefab == null || firePoint == null || !isFiring ) return;


        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
      

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    
        if (rb != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        }
       if(phase2Applied && firePoint2 != null)
        {  
            GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation); 
            Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
            if (rb2 != null)
            {
                Vector2 direction2 = (player.position - firePoint2.position).normalized;
                rb2.linearVelocity = direction2 * bulletSpeed;
            }
        }
        if (finalPhaseApplied)
        {
            ShootTriple(firePoint2);
            ShootTriple(firePoint);
        }
    }
    void ShootTriple(Transform fp)
{
    Vector2 baseDir = (player.position - fp.position).normalized;

    ShootBullet(fp.position, baseDir);                 // giữa
    ShootBullet(fp.position, RotateVector(baseDir, -30f)); // trái
    ShootBullet(fp.position, RotateVector(baseDir, 30f));  // phải
}
void ShootBullet(Vector2 pos, Vector2 direction)
{
    GameObject bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

    if (rb != null)
    {
        rb.linearVelocity = direction * bulletSpeed;
    }
    }
    Vector2 RotateVector(Vector2 v, float angle)
{
    float rad = angle * Mathf.Deg2Rad;
    float cos = Mathf.Cos(rad);
    float sin = Mathf.Sin(rad);

    return new Vector2(
        v.x * cos - v.y * sin,
        v.x * sin + v.y * cos
    );
}
    

    IEnumerator Death()
    {
        if (MusicManager.instance != null && normalLevelMusic != null)
    {
        // Nhạc Boss sẽ nhỏ dần và nhạc đi đường sẽ nổi lên trong 3 giây
        MusicManager.instance.SwitchTrack(normalLevelMusic, 3.0f);
    }
        PlayBossSound(deathSound, 1.0f);
        dropExp.DropExperience();
        rb.linearVelocity = Vector2.zero;

        if (death != null)
            death.enabled = true;

        yield return new WaitForSeconds(4f);
   
        var keySpawn = GetComponent<KeySpam>();
        if (keySpawn != null)
        {
            keySpawn.OnDeath();
        }
        Destroy(gameObject);
    }
    IEnumerator FireCycle()
    {
    while (!isDead && isChasing)
    {
        yield return new WaitForSeconds(waitTime);

        if (isDead) break;

        isFiring = true;

        float timer = 0f;
        while (timer < fireDuration)
        {
            Fire();
            yield return new WaitForSeconds(fireInterval);
            timer += fireInterval;
        }

        isFiring = false;
    }

    isFireCycling = false; // reset khi thoát
    }
}