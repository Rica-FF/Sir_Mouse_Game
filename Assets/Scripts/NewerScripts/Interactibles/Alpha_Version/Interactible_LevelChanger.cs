using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactible_LevelChanger : Interactible_Base
{
    public int LevelIndexToLoad, PlayerSpawnIndexNextArea;
    public bool SpawnLookingRight;

    public Animator CrossFadeAnimator;

    [HideInInspector]
    private GameObject GameInstance;


    private void Start()
    {
        GameInstance = FindObjectOfType<InstanceOfGame>().gameObject;
    }



    // called from loading screen
    public void PlayGame()
    {
        SceneManager.LoadScene(LevelIndexToLoad);
    }


    public void LoadLevelSlow()
    {
        // set the player spawn location
        NextLevel(PlayerSpawnIndexNextArea);
        // actual scene loading
        StartCoroutine(DelayLoad(0.5f));
    }



    IEnumerator DelayLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        SceneManager.LoadScene(LevelIndexToLoad);
    }




    public void NextLevel(int playerSpawnIndex)
    {
        // set the parent for the player
        PlayerRefs.transform.SetParent(GameInstance.transform); // double check if it is indeed the player refs that i need to re-parent

        // check what direction the player needs
        if (SpawnLookingRight)
        {
            PlayerRefs.transform.localScale = new Vector3(-6, 6, 6);
        }
        else
        {
            PlayerRefs.transform.localScale = new Vector3(6, 6, 6);
        }

        // set the player spawn location
        GameInstance.GetComponent<InstanceOfGame>().playerStartIndex = playerSpawnIndex;
    }
}
