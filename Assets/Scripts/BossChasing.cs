using UnityEngine;
using System.Collections;

public class BossChasing : MonoBehaviour
{
    public BossLv2 bossLv2;
    private logicManager logic;
    public GameObject bossHealth;
    public GameObject BossMenu;

    void Awake()
    {
        bossHealth.SetActive(false);
        BossMenu.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossHealth.SetActive(true);
            bossLv2.isChasing = true;
            StartCoroutine(ShowBossMenuCoroutine());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossHealth.SetActive(false);
            bossLv2.isChasing = false;
        }
    }
    IEnumerator ShowBossMenuCoroutine()
    {
        BossMenu.SetActive(true); // Show the boss menu
        
        yield return new WaitForSeconds(3f); // Wait for 3 seconds (this line won't actually pause the method, it's just a placeholder)
        BossMenu.SetActive(false); // Hide the boss menu after 3 seconds
    }
}
