using UnityEngine;

public class FinalBossChasing : MonoBehaviour
{
    public FinalBoss boss;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {      
            boss.isChasing = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            boss.isChasing = false;
        }
    }
}
