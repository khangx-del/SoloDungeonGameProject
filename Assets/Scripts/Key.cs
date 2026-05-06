using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip openDoor;
    [SerializeField] Door door; 
    
   private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(openDoor, transform.position);
            door.UnlockDoor();
            Debug.Log("Key collected!");
            Destroy(gameObject); // Destroy the key after collection
        }
    }
}
