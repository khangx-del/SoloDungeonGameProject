using UnityEngine;

public class KeySpam : MonoBehaviour
{
    public GameObject key;

    private void Start()
    {
        key.SetActive(false);
    }
    public void OnDeath()
    {
       key.transform.parent = null; // tách khỏi enemy
       key.SetActive(true);
    } 
}
