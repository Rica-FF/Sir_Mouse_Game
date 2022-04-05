using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public int levelIndex;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            PlayGame();
        }
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadLevelSlow(int index)
    {
        StartCoroutine(DelayLoad(index, 0.5f));
    }

    public void LoadLevelInSeconds(float seconds)
    {
        StartCoroutine(DelayLoad(levelIndex, seconds));
    }

    IEnumerator DelayLoad(int _index, float _seconds)
    {
        for (float t = 0f; t < _seconds; t += Time.deltaTime)
        {
            float normalizedTime = t / _seconds;
            yield return null;
        }
        SceneManager.LoadScene(_index);
    }

    
}
