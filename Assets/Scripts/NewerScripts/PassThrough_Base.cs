using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThrough_Base : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;



    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }



    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            //StartCoroutine(MakeTransparant());
            StartCoroutine(LerpColor(_spriteRenderer.color, new Color(1f, 1f, 1f, 0.5f), 0.2f));
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            StartCoroutine(LerpColor(_spriteRenderer.color, new Color(1f, 1f, 1f, 1f), 0.2f));
        }
    }




    IEnumerator LerpColor(Color start, Color end, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            _spriteRenderer.color = Color.Lerp(start, end, normalizedTime);
            yield return null;
        }
        _spriteRenderer.color = end; //without this, the value will end at something like 0.9992367
    }
}
