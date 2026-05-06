using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        } else {
            Destroy(gameObject);
        }
    }

    public void SwitchTrack(AudioClip newClip, float fadeDuration = 1.5f)
    {
        // Chạy Coroutine để đổi nhạc
        StartCoroutine(FadeTrack(newClip, fadeDuration));
    }

    IEnumerator FadeTrack(AudioClip newClip, float duration)
    {
        float startVolume = audioSource.volume;

        // 1. Giảm âm lượng nhạc nền cũ (Fade Out)
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        
        // --- THAY ĐỔI Ở ĐÂY ---
        audioSource.clip = newClip;
        audioSource.loop = true; // Bật lặp lại cho bài nhạc mới (nhạc Boss)
        // ----------------------
        
        audioSource.Play();

        // 2. Tăng âm lượng nhạc Boss mới (Fade In)
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / duration;
            yield return null;
        }
        
        audioSource.volume = startVolume;
    }

    public void FadeOutToSilence(float duration)
    {
        StartCoroutine(FadeOutRoutine(duration));
    }

    IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = audioSource.volume;
        
        // --- NÊN THÊM DÒNG NÀY ---
        audioSource.loop = false; // Tắt lặp lại khi muốn tắt hẳn nhạc (lúc Boss chết)
        // -------------------------

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        audioSource.Stop();
    }
}