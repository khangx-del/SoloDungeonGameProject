using UnityEngine;

public class Exp : MonoBehaviour
{
    public int expValue = 10;
    public float lifeTime = 20f;
    public float moveSpeed = 1f;
    public float detectRange = 3f;
    Transform player;

    void Start()
    {
        Destroy(gameObject, lifeTime);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)        
        {
            player = p.transform;
        }
    }
    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < detectRange)
        {
            transform.position = Vector2.Lerp(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerExperience playerExp = other.GetComponent<PlayerExperience>();

        if (playerExp != null)
        {
            playerExp.GainExperience(expValue);
            Destroy(gameObject);
        }
    }
}