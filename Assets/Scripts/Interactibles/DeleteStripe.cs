using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeleteStripe : MonoBehaviour
{
    public GameObject stripe;
    public bool turnOnPointers = false;
    public GameObject tutorial;
    public bool lastChapter = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if(stripe)
            {
                Destroy(stripe);
            }
            if (turnOnPointers)
            {
                collider.GetComponent<NavMeshAgent>().isStopped = true;
                collider.GetComponent<PlayerTouchControls>().walkingEnabled = false;
                StartCoroutine(Delay(collider));
                PopUpPointer.disableIrrelevantPointers = false;
                tutorial.GetComponent<Tutorial>().Walk_04();
            }

            if (lastChapter)
            {
                collider.GetComponent<NavMeshAgent>().speed = 0;
                tutorial.GetComponent<Animator>().SetTrigger("ZoomExitSign");
                tutorial.GetComponent<Tutorial>().ActivateExitPointer();
                StartCoroutine(DestroyDelay());
            }
        }
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(5f);
        Destroy(tutorial);
    }

    IEnumerator Delay(Collider _collider)
    {
        float seconds = 0.5f;

        for (float t = 0f; t < seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / seconds;
            yield return null;
        }
        _collider.GetComponent<NavMeshAgent>().isStopped = false;
        Destroy(gameObject);

    }
}
