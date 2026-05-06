using System.Collections;
using UnityEngine;

public class DropExp : MonoBehaviour
{
    public GameObject expPrefab; // Prefab của EXP sẽ được rơi ra
    public int max = 3;
    public int min = 1;

    public void DropExperience()
    {
        if (expPrefab != null)
        {
            StartCoroutine(DropExpCoroutine());
        }
    }
    IEnumerator DropExpCoroutine()
    {
        if (expPrefab == null) yield break;
        int expCount = Random.Range(min, max); // Số lượng EXP sẽ rơi ra (3-5)
        for (int i = 0; i < expCount; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * 0.5f; // Vị trí rơi ngẫu nhiên quanh kẻ địch
            Instantiate(expPrefab, spawnPosition, Quaternion.identity); // Tạo EXP tại vị trí rơi
           
        }
        
    }
}
