using UnityEngine;

public class bullet : MonoBehaviour
{
    public float lifetime = 2f; 
    public int damage = 10; 

    private void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
