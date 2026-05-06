using UnityEngine;
using System.Collections;
public class HitFlash : MonoBehaviour
{
    public float duration = 0.1f;

    void Start()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}

