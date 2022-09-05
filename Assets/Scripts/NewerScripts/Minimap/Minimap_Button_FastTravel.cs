using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minimap_Button_FastTravel : MonoBehaviour
{
    public bool SpawnLookingRight;

    public Animator CrossFadeAnimator;

    [HideInInspector]
    private GameObject GameInstance, PlayerReferenceObject;

    


    private void Start()
    {
        GameInstance = FindObjectOfType<GameManager>().gameObject;

        PlayerReferenceObject = FindObjectOfType<PlayerReferences>().gameObject;
    }


    public void LoadLevelSlow(int indexToLoad)
    {
        // set the player spawn location
        NextLevel(0);
        // actual scene loading
        StartCoroutine(DelayLoad(0.5f, indexToLoad));
    }



    IEnumerator DelayLoad(float seconds, int levelToLoad)
    {
        CrossFadeAnimator.SetTrigger("Fast");

        yield return new WaitForSeconds(seconds);

        // load the scene index that's on the button
        SceneManager.LoadScene(levelToLoad);
    }




    public void NextLevel(int playerSpawnIndex)
    {
        // set the parent for the player
        PlayerReferenceObject.transform.SetParent(GameInstance.transform); // double check if it is indeed the player refs that i need to re-parent

        // check what direction the player needs
        if (SpawnLookingRight)
        {
            PlayerReferenceObject.transform.localScale = new Vector3(-6, 6, 6);
        }
        else
        {
            PlayerReferenceObject.transform.localScale = new Vector3(6, 6, 6);
        }

        // set the player spawn location
        GameInstance.GetComponent<GameManager>().playerStartIndex = playerSpawnIndex;
    }
}
