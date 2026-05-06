using UnityEngine;

public class crystalAnim : MonoBehaviour
{
    public SpriteRenderer crystal;

    public void Start()
    {
        crystal = GetComponent<SpriteRenderer>();
    }
    public void Awake()
    {
        crystal.enabled = true;
    }
}
