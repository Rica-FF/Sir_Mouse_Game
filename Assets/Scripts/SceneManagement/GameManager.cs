using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [FormerlySerializedAs("player")] 
    public GameObject Player;
    
    public int playerStartIndex = 0;

    public bool usesMouseControls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);

        Player.GetComponent<PlayerReferences>().mouseControls = usesMouseControls;
    }
}
