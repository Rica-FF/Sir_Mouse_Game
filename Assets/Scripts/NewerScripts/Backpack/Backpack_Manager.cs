using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack_Manager : MonoBehaviour
{
    // assign the buttons in inspector
    public List<GameObject> Backpack_Pickups;
    public List<GameObject> Pickup_Prefabs;

    private PlayerReferences _playerRefs;
    private GameObject _playerReferencesObject;

    private BackPack_Minimap_Manager _bmManager;
    private SoundsOfUI _soundEffectsUI;

    [SerializeField]
    private GameObject _cloudParticlePrefab;


    private void Start()
    {
        StartCoroutine(GetPlayerRefs());

        _bmManager = transform.parent.GetComponentInChildren<BackPack_Minimap_Manager>();
        _soundEffectsUI = transform.parent.GetComponentInChildren<SoundsOfUI>();
    }
    private IEnumerator GetPlayerRefs()
    {
        yield return new WaitForSeconds(1.5f);

        _playerRefs = FindObjectOfType<PlayerReferences>();
        _playerReferencesObject = _playerRefs.gameObject;
    }


    // ! call this whenever the backpack is opened !
    public void UpdateInventoryButtons()
    {
        // check for the count of pickups in backpack...
        // for each backpack pickup, if it is true (put in backpack)...
        // set the button to true, else set to false

        for (int i = 0; i < Backpack_Inventory.ItemsInBackpack.Length; i++)
        {
            GameObject button = Backpack_Pickups[i].transform.GetChild(0).gameObject;

            if (Backpack_Inventory.ItemsInBackpack[i] == true)
            {               
                button.SetActive(true);
                button.GetComponent<Animation>().Play("SineWave_Scaler");

                Backpack_Pickups[i].GetComponent<Image>().enabled = false;
            }
            else
            {
                button.SetActive(false);

                Backpack_Pickups[i].GetComponent<Image>().enabled = true;
            }
        }
    }




    // call this on the backpack buttons clicks
    public void DropPickup(int index)
    {
        // play pop animation
        GameObject button = Backpack_Pickups[index].transform.GetChild(0).gameObject;
        button.GetComponent<Animation>().Play("Backpack_Pickup_Popout");

        // play sound
        _soundEffectsUI.PlaySoundEffect(0);

        // closes the inventory panel
        StartCoroutine(CloseBackpackDelay(index)); 
    }
    private IEnumerator CloseBackpackDelay(int _index)
    {
        yield return new WaitForSeconds(0.5f);

        // close backpack panel
        _bmManager.CloseBackpack();  

        // reset the bool value of the inventory
        Backpack_Inventory.ItemsInBackpack[_index] = false;

        // play unequip animation 
        _playerRefs.GetComponent<Animator>().Play("Unequipe_0");

        yield return new WaitForSeconds(0.3f);

        _playerRefs.GetComponent<Animator>().SetLayerWeight(1, 1f);

        // instantiate particle effect
        Instantiate(_cloudParticlePrefab, _playerRefs.swordJoint.transform.position, Quaternion.identity);

        // instantiate the pickup on player position
        var spawnedObject = Instantiate(Pickup_Prefabs[_index], _playerRefs.swordJoint.transform.position, Quaternion.identity);
        spawnedObject.transform.SetParent(_playerRefs.playerHand.transform);

        spawnedObject.transform.localRotation = Quaternion.Euler(60, 0, -30);

        spawnedObject.transform.localPosition = new Vector3(-0.03f, 0.02f, 0.02f);
        spawnedObject.GetComponentInChildren<Rigidbody>().isKinematic = true;

        spawnedObject.GetComponentInChildren<Interactible_Base>().GetComponent<SphereCollider>().enabled = false;
        spawnedObject.GetComponentInChildren<Interactible_Base>().PlayerRefs = _playerRefs;

        _playerRefs.attachedObject = spawnedObject;

        var pointerBases = _playerRefs.attachedObject.GetComponentsInChildren<Pointer_Base>();
        foreach (var pBase in pointerBases)
        {
            if (pBase.TypeOfPointer == PointerType.Pickup)
            {
                _playerRefs.PickedUpObject = pBase.TypeOfPickup;
                StartCoroutine(pBase.GetSparkleRefs(false));

                // assign the _pointerInHands
                _playerRefs.PointerOfPickUpInHands = pBase;
            }
        }
    }
}
