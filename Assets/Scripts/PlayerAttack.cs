using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip swingSound;    // Tiếng vung kiếm (luôn kêu khi bấm chuột)
    public AudioClip hitSound;      // Tiếng "phập" (chỉ kêu khi trúng địch)

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 1f;
    public int attackDamage = 25;
    public LayerMask enemyLayers;
    public Transform player;

    public float attackRate = 1f;
    float nextAttackTime = 0f;
    public float attackDistance = 1f;
    public AttackAnimation anim;

    public void Start()
    {
        ApplyUpgrades();
        // Tự động lấy AudioSource nếu bạn quên kéo vào Inspector
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void ApplyUpgrades()
    {
        attackDamage += GameData.damageLevel * 2;
        attackRate += GameData.attackSpeedLevel * 0.1f;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - player.position).normalized;
        attackPoint.position = (Vector2)player.position + direction * attackDistance;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackPoint.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        if (Input.GetMouseButtonDown(1) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack()
    {
        anim.Play();

        // 1. Phát tiếng vung kiếm ngay khi bấm chuột
        if (audioSource != null && swingSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Giúp âm thanh bớt nhàm chán
            audioSource.PlayOneShot(swingSound);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        // Kiểm tra xem có đánh trúng ai không
        if (hitEnemies.Length > 0)
        {
            // 2. Phát tiếng đánh trúng (chỉ phát 1 lần cho mỗi cú chém trúng)
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
                enemy.GetComponent<bosslv2Health>()?.TakeDamage(attackDamage);    
                enemy.GetComponent<ZombieHealth>()?.TakeDamage(attackDamage);  
                enemy.GetComponent<FinalbossHealth>()?.TakeDamage(attackDamage); 
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}