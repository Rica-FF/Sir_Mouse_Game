using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateMemory : MonoBehaviour
{
    // this script was made to remember whether specific objects (this ones 1st child) should be activated or not
    // currently only used for roses on the tower 

    public int BoolIndexToCheck;

    void Start()
    {
        if (GameStateData.FindCorrectArray(SceneManager.GetActiveScene().name)[BoolIndexToCheck] == true)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

    }


}
