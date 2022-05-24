using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpLevel : MonoBehaviour
{
    public GameObject playerRigid;
    public GameObject[] playerStarts = new GameObject[1];

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    private GameObject gameInstance;
    private bool righDirection = true;

    public GameObject[] referencedObjects = new GameObject[0];

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        gameInstance = GameObject.Find("GameInstance");
        player = gameInstance.GetComponent<InstanceOfGame>().player;
        player.SetActive(true);
        player.transform.position = new Vector3(0, 0, 0);
        player.transform.parent = playerRigid.transform;
        playerRigid.transform.position = playerStarts[gameInstance.GetComponent<InstanceOfGame>().playerStartIndex].transform.position;

        playerRigid.GetComponent<PlayerTouchControls>().player = player;
        //playerRigid.GetComponent<PlayerTouchControls>().dropPointer = player.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
        playerRigid.GetComponent<PlayerTouchControls>().enabled = true;
        

        // adding attached objects if present
        if(player.GetComponent<PlayerReferences>().attachedObject)
        {
            if(player.GetComponent<PlayerReferences>().attachedObject.name == "Coin_C")
            {
                player.GetComponent<PlayerReferences>().attachedObject.GetComponent<Coin>().player = playerRigid;
                if(referencedObjects.Length > 0)
                {
                    player.GetComponent<PlayerReferences>().attachedObject.GetComponent<Coin>().WishingWell = referencedObjects[0];
                }
            }
            else if(player.GetComponent<PlayerReferences>().attachedObject.name == "Corn")
            {
                player.GetComponent<PlayerReferences>().attachedObject.GetComponent<Corn>().player = playerRigid;
            }
            else if (player.GetComponent<PlayerReferences>().attachedObject.name == "Bucket")
            {
                player.GetComponent<PlayerReferences>().attachedObject.GetComponent<Bucket>().player = playerRigid;
            }
            //player.GetComponent<PlayerReferences>().dropPointer.SetActive(true);
        }  
        
        // additionally activate the sparkles on other objects
        //---
    }

    public void NextLevel(int _playerStartIndex)
    {
        player.transform.parent = gameInstance.transform;

        if(righDirection)
        {
            player.transform.localScale = new Vector3(-6, 6, 6);
        }
        else
        {
            player.transform.localScale = new Vector3(6, 6, 6);
        }
        gameInstance.GetComponent<InstanceOfGame>().playerStartIndex = _playerStartIndex;
    }

    public void RightDirection(bool _rightDirection)
    {
        righDirection = _rightDirection;
    }
}
