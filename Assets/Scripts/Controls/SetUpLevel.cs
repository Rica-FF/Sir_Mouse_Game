using System;
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
    private bool righDirection = true;

    public GameObject[] referencedObjects = new GameObject[0];

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameObject gameManagerPrefab = Resources.Load<GameObject>("Prefabs/GameManager");
            if (gameManagerPrefab != null) Instantiate(gameManagerPrefab);
            else Debug.LogError("GameManager Prefab doesn't exist! Did you change its name or changed its path?");
        }
    }
    
    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var gameManager = GameManager.Instance;
  
        player = gameManager.Player;
        player.SetActive(true);
        player.transform.position = new Vector3(0, 0, 0);
        player.transform.parent = playerRigid.transform;
        playerRigid.transform.position = playerStarts[gameManager.playerStartIndex].transform.position;

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
        player.transform.parent = GameManager.Instance.transform;

        if(righDirection)
        {
            player.transform.localScale = new Vector3(-6, 6, 6);
        }
        else
        {
            player.transform.localScale = new Vector3(6, 6, 6);
        }
        GameManager.Instance.playerStartIndex = _playerStartIndex;
    }

    public void RightDirection(bool _rightDirection)
    {
        righDirection = _rightDirection;
    }
}
