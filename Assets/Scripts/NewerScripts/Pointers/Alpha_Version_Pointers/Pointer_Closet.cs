using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pointer_Closet : Pointer_Base
{
    [SerializeField]
    private GameObject _openDoor;
    [SerializeField]
    private GameObject _shieldInteractible;
    private Collider _shieldCollider;
    private Pointer_ShieldPickup _shieldScript;

    private GameObject _shieldSpriteParent;
    private List<SpriteRenderer> _shieldSprites;
    private List<GameObject> _shieldSpriteObjects = new List<GameObject>();

    public List<Sprite> ShieldSprites;

    private bool _closetIsOpen;

    [HideInInspector]
    public int ShieldIndex;
    [HideInInspector]
    public int ShieldIndexLimit;


    private void Start()
    {
        ShieldIndexLimit = 0;

        _shieldCollider = _shieldInteractible.GetComponent<Collider>();

        _shieldSpriteParent = _shieldInteractible.transform.GetChild(0).gameObject;
        _shieldScript = _shieldInteractible.GetComponentInChildren<Pointer_ShieldPickup>();
        _shieldSprites = _shieldSpriteParent.GetComponentsInChildren<SpriteRenderer>(true).ToList();

        foreach (var sprite in _shieldSprites)
        {
            _shieldSpriteObjects.Add(sprite.gameObject);

            ShieldIndexLimit += 1;
        }

        // making it so i start at the end of the list, cuzz counter resets on first opening
        ShieldIndex = ShieldIndexLimit;
    }



    public override void PlayEvent()
    {
        base.PlayEvent();

        _closetIsOpen = !_closetIsOpen;

        OpenOrCloseCloset();    
    }

    void OpenOrCloseCloset()
    {
        if (_closetIsOpen == true)
        {
            // enable sprite
            _openDoor.SetActive(true);

            // play sound open
            InteractibleScript.AudioSource.PlayOneShot(SoundEffects[0]);

            // update index
            Debug.Log(ShieldIndex - 1 + " index disabled");
            _shieldSpriteObjects[ShieldIndex - 1].SetActive(false);  // OUT OF BOUNDS

            ShieldIndex += 1;
            if (ShieldIndex > ShieldIndexLimit)
            {
                ShieldIndex = 1;
            }

            Debug.Log(ShieldIndex - 1 + " index enabled");
            // enable shield interactible
            _shieldCollider.enabled = true;
            _shieldSpriteObjects[ShieldIndex-1].transform.parent.gameObject.SetActive(true);
            // update shield sprite object
            _shieldSpriteObjects[ShieldIndex-1].SetActive(true);

        }
        else
        {
            // enable sprite
            _openDoor.SetActive(false);

            // play sound closed
            InteractibleScript.AudioSource.PlayOneShot(SoundEffects[1]);

            // disable shield interactible
            _shieldCollider.enabled = false;
            _shieldSpriteObjects[ShieldIndex - 1].transform.parent.gameObject.SetActive(false);
        }
        
    }
}
