using UnityEngine;

public class FindkeyGuide : MonoBehaviour
{
    public GameObject tutorialText;
    bool hasShown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasShown)
        {
            tutorialText.SetActive(true);
            hasShown = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.SetActive(false);
        }
    }
}
