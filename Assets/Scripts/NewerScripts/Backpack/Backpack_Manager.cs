using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack_Manager : MonoBehaviour
{
    // assign the buttons in inspector
    public List<GameObject> Backpack_Pickups;
    public List<GameObject> Pickup_Prefabs;

    private GameObject _playerReferencesObject;

    [SerializeField]
    private BackPack_Minimap_Manager _bmManager;


    private void Start()
    {
        _playerReferencesObject = FindObjectOfType<PlayerReferences>().gameObject;
    }


    // ! call this whenever the backpack is opened !
    public void UpdateInventoryButtons()
    {
        // check for the count of pickups in backpack...
        // for each backpack pickup, if it is true (put in backpack)...
        // set the button to true, else set to false


        for (int i = 0; i < Backpack_Inventory.ItemsInBackpack.Length; i++)
        {
            Debug.Log(i + " is I");
            if (Backpack_Inventory.ItemsInBackpack[i] == true)
            {
                Backpack_Pickups[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Backpack_Pickups[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }




    // call this on the backpack buttons clicks
    public void DropPickup(int index)
    {
        // instantiate the pickup on player position
        Instantiate(Pickup_Prefabs[index], _playerReferencesObject.transform.position, Quaternion.identity);

        // reset the bool value of the inventory
        Backpack_Inventory.ItemsInBackpack[index] = false;

        // closes the inventory panel
        _bmManager.CloseBackpack();  // not working on bucket
    }
}
