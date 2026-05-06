using UnityEngine;

public class EnemyChasing : MonoBehaviour
{
   public Enemy enemy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.isChasing = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.isChasing = false;
        }
    }
}
