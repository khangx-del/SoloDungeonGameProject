using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;

    public Sprite idleSprite;
    public Sprite[] animationSprites;

    public float animationTime = 0.1f;
    private int animationFrame;

    private bool isPlaying = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = idleSprite;
    }

    private void Update()
    {
        if (isPlaying) return;

        // luôn giữ idle khi không đánh
        spriteRenderer.sprite = idleSprite;
    }

    public void Play()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            animationFrame = 0;
            InvokeRepeating(nameof(NextFrame), 0f, animationTime);
        }
    }

    private void NextFrame()
    {
        if (animationFrame >= animationSprites.Length)
        {
            Stop();
            return;
        }

        spriteRenderer.sprite = animationSprites[animationFrame];
        animationFrame++;
    }

    private void Stop()
    {
        CancelInvoke(nameof(NextFrame));
        isPlaying = false;
        spriteRenderer.sprite = idleSprite;
    }
}

