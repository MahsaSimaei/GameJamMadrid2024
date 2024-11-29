using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isShaking = false;

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (isShaking) // Check if the camera is already shaking
            yield break; // If so, don't start another shake

        isShaking = true;
        originalPosition = transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        transform.localPosition = originalPosition;
        isShaking = false;
    }
}
