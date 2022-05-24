using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Handle : Interactible_Base
{
    public AudioSource suspicious;

    private bool down = false;
    private bool suspiciousBool = true;



    public void ToggleHandle()
    {
        if (down)
        {
            GetComponent<Animator>().SetTrigger("Up");
            StartCoroutine(SetFalse());
        }
        if (!down)
        {
            if (suspiciousBool)
            {
                StartCoroutine(PlaySound());
                print("play suspicious");
                suspiciousBool = false;
            }
            GetComponent<Animator>().SetTrigger("Down");
            StartCoroutine(SetTrue());
        }
    }

    IEnumerator SetFalse()
    {
        float seconds = 2;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        down = false;
    }

    IEnumerator SetTrue()
    {
        float seconds = 2;
        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }

        down = true;
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(1.5f);
        if (suspicious)
        {
            suspicious.Play();
        }
    }
}
