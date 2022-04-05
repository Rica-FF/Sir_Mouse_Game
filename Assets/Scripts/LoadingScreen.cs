using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadingScene());
    }

    IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(2);

        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Start");


        yield return new WaitForSeconds(2f);

        GetComponent<LoadLevel>().PlayGame();
    }
}
