using UnityEngine;

public class Door : MonoBehaviour
{
    bool isdoorlooked = true; // set the door to be locked by default
    private Collider2D doorCollider;
    public GameObject doorlocked; // Reference to the SpriteRenderer component of the door
    public GameObject doorunlocked; // Sprite to show when the door is locked

    public void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
       
        UpdateVisual();
    }

    private void Start()
    {
        
    }
    void UpdateVisual()
    {
    doorlocked.SetActive(isdoorlooked);
    doorunlocked.SetActive(!isdoorlooked);
    }

    public void UnlockDoor()
    {
        isdoorlooked = false;
        doorCollider.enabled = false; // mở đường cho player đi qua
        Debug.Log("Door unlocked!");
        UpdateVisual();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isdoorlooked)
        {
            Debug.Log("The door is locked. Find the key to unlock it.");
        }
    }
    
}
