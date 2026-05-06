using UnityEngine;

public class ZombieChasing : MonoBehaviour
{
    public Zombie zombie;
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            zombie.isChasing = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            zombie.isChasing = false;
        }
    }
}

