using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlyAway3D : Interactible_Base
{
    public GameObject Object1;
    public Vector3 direction1;
    public float liveTime1;

    public GameObject Object2;
    public Vector3 direction2;
    public float liveTime2;

    public GameObject Object3;
    public Vector3 direction3;
    public float liveTime3;

    public GameObject Object4;
    public Vector3 direction4;
    public float liveTime4;

    private bool activate1 = false;
    private bool activate2 = false;
    private bool activate3 = false;
    private bool activate4 = false;
    private int queue = 0;

    private bool Frog = false;

    public AudioClip[] sounds = new AudioClip[0];
    public AudioSource animalSounds1;
    public AudioSource animalSounds2;



    void FixedUpdate()
    {
        if (activate1)
        {
            Object1.transform.position += direction1;
        }
        if (activate2)
        {
            Object2.transform.position += direction2;
        }
        if (activate3)
        {
            Object3.transform.position += direction3;
        }
        if (activate4 && Frog)
        {
            Object4.transform.position += direction4;
        }
    }



    public override void ExtraBehaviour()
    {
        base.ExtraBehaviour();

        if (queue == 0)
        {
            animalSounds1.clip = sounds[0];
            animalSounds1.Play();
            StartCoroutine(FirstQueue());
        }
        else if (queue == 1)
        {
            StartCoroutine(SecondQueue());
        }
        else if (queue == 2)
        {
            animalSounds1.clip = sounds[1];
            animalSounds1.Play();
            StartCoroutine(ThirdQueue());
        }
        else if (queue == 3)
        {
            animalSounds2.clip = sounds[2];
            animalSounds2.Play();
            StartCoroutine(FourthQueue());
        }
    }






    IEnumerator FirstQueue()
    {
        if (activate1 == false)
        {
            Object1.SetActive(true);
            activate1 = true;
            queue = 1;

            for (float t = 0f; t < liveTime1; t += Time.deltaTime)
            {
                float normalizedTime = t / liveTime1;
                yield return null;
            }

            //yield return new WaitForSeconds(liveTime1);
            activate1 = false;
            Object1.SetActive(false);
            Object1.transform.position = gameObject.transform.position;
        }
    }
    IEnumerator SecondQueue()
    {
        if (queue == 1 && activate2 == false)
        {
            Object2.SetActive(true);
            activate2 = true;
            queue = 2;
            yield return new WaitForSeconds(liveTime2);
            activate2 = false;
            Object2.SetActive(false);
            Object2.transform.position = gameObject.transform.position;
        }
    }
    IEnumerator ThirdQueue()
    {
        if (queue == 2 && activate3 == false)
        {
            Object3.SetActive(true);
            activate3 = true;
            queue = 3;
            yield return new WaitForSeconds(liveTime3);
            activate3 = false;
            Object3.SetActive(false);
            Object3.transform.position = gameObject.transform.position;
        }
    }
    IEnumerator FourthQueue()
    {
        if (queue == 3 && activate4 == false)
        {
            float jumpTime = 0.5f;
            float waitTime = 0.2f;
            Object4.SetActive(true);
            activate4 = true;
            queue = 0;
            Frog = true;
            yield return new WaitForSeconds(jumpTime);
            Frog = false;
            yield return new WaitForSeconds(waitTime);
            Frog = true;
            yield return new WaitForSeconds(jumpTime);
            Frog = false;
            yield return new WaitForSeconds(waitTime);
            Frog = true;
            yield return new WaitForSeconds(jumpTime);
            Frog = false;
            yield return new WaitForSeconds(waitTime);
            Frog = true;
            yield return new WaitForSeconds(jumpTime);
            Frog = false;
            yield return new WaitForSeconds(waitTime);
            Frog = true;
            yield return new WaitForSeconds(jumpTime);
            Frog = false;
            yield return new WaitForSeconds(waitTime);
            Frog = true;
            yield return new WaitForSeconds(jumpTime);
            Frog = false;
            yield return new WaitForSeconds(waitTime);
            //yield return new WaitForSeconds(liveTime4);
            activate4 = false;
            Object4.SetActive(false);
            Object4.transform.position = gameObject.transform.position;
        }
    }
}

