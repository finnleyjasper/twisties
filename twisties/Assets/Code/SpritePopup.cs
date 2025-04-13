using UnityEngine;

public class SpritePopup : MonoBehaviour
{
    private Vector3 originalScale;
    private Vector3 targetScale;

    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float pulseSpeed = 2f;

    private float pulseTimer = 0f;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale * pulseScale;
    }

    void Update()
    {
        pulseTimer += Time.deltaTime * pulseSpeed;
        float scaleFactor = Mathf.Lerp(1f, pulseScale, (Mathf.Sin(pulseTimer) + 1f) / 2f);
        transform.localScale = originalScale * scaleFactor;
    }
}
