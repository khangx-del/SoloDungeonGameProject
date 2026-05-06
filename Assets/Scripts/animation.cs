using UnityEngine;

public class Animation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite idleSprite;
    public Sprite[] animationSprites;

    public float animationTime = 0.25f;
    private int animationFrame;

    public bool loop = true;
    public bool idle = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
        
        // --- THE FIX ---
        animationFrame = -1; // Reset to start
        CancelInvoke(nameof(NextFrame)); // Stop any old timers
        InvokeRepeating(nameof(NextFrame), 0, animationTime); // Start fresh timer immediately
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        CancelInvoke(nameof(NextFrame)); // Stop timer when hidden
    }

    // REMOVE Start() entirely, we handle it in OnEnable now

    private void NextFrame()
    {
        animationFrame++;

        if (loop && animationFrame >= animationSprites.Length) {
            animationFrame = 0;
        }

        if (idle) {
            spriteRenderer.sprite = idleSprite;
        } else if (animationFrame >= 0 && animationFrame < animationSprites.Length) {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
        else if (!loop && animationFrame >= animationSprites.Length) {
            // Stay on the last frame if not looping
            animationFrame = animationSprites.Length - 1;
        }
    }
}
