using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minimap_Manager : MonoBehaviour
{
    // assign the buttons in inspector
    public List<GameObject> Minimap_Buttons;



    // call this method whenever the map inventory gets updated 
    public void UpdateMinimapButtons()
    {
        for (int i = 0; i < Minimap_Inventory.ItemsInMinimap.Length; i++)
        {
            if (Backpack_Inventory.ItemsInBackpack[i] == true)
            {
                Minimap_Buttons[i].SetActive(true);
            }
            else
            {
                Minimap_Buttons[i].SetActive(false);
            }
        }
    }


    // call this whenever an area is entered...
    // check if the scene you're in is part of the button...
    // if so, disable that button in map
    public void DisableCurrentAreaForFastTravel()
    {
        // check all the map buttons their indexes
        foreach (var gameobject in Minimap_Buttons)
        {
            // if the map index corresponds with the index of the current scene (means I need to give the map buttons a script that assigns a build index)
            //if (SceneManager.GetActiveScene().buildIndex == )
            //{

            //}
        }


    }
}
