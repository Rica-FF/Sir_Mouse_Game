using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(0, 10)]
    public float smoothFactor;
    public Vector3 minValue, maxValue;

    public bool follow = false;

    private float resOffset;

    private void Awake()
    {
        resOffset = Mathf.RoundToInt((GetComponent<Camera>().aspect - 1.3f) * 10) * 0.4f + 0.2f;
        minValue.x += resOffset;
        maxValue.x -= resOffset;
    }

    private void FixedUpdate()
    {
        StartCoroutine(Delay());
        if (follow)
        {
            Follow();
        }
    }

    void Follow()
    {
        // Player position
        Vector3 targetPosition = target.position + offset;

        // Level Boundries
        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x),
            Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y),
            Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z)
            );

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }

    IEnumerator Delay()
    {
        float seconds = 1f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        follow = true;
    }
}
