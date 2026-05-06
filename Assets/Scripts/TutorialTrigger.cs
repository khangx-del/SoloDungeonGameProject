using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject tutorialText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.SetActive(true); // Show the tutorial text when the player enters the trigger
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.SetActive(false); // Hide the tutorial text when the player exits the trigger
        }
    }
}
