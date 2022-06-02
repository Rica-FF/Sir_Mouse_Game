using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapable_Base : MonoBehaviour
{
    [HideInInspector]
    public Transform ParentTransform;

    public bool OneTimeUse;
    private bool _usedSuccesfully;

    public bool HasACooldown;
    private bool _onCooldown;
    [SerializeField]
    private float _cooldownLength;

    [SerializeField]
    private AudioClip[] _audioClipsToPlay;

    // components any tap-able would have
    private Animator _animator;
    private AudioSource _audioSource;
    private Collider _collider;

    // values for dragging objects
    public bool CanBeMoved;
    private bool _activatedFollowMouse;
    private Vector3 _mousePosition;
    private Vector3 _mouseWorldPosXY;
    private Vector3 _mouseWorldPositionXYZ;
    [SerializeField]
    private LayerMask _layersToCastOn;
    private RaycastHit _hit;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<Collider>();

        ParentTransform = transform.parent;

        // for update method de-activation
        this.enabled = false;
    }

    private void Update()
    {
        FollowMouseLogic();
    }



    // logic for having object follow the mouse
    private void FollowMouseLogic()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // set mouse movement off let go of mouse
            _activatedFollowMouse = false;
        }
        if (_activatedFollowMouse == true)
        {
            _mousePosition = Input.mousePosition;
            _mouseWorldPosXY = Camera.main.ScreenToWorldPoint(_mousePosition);

            ParentTransform.position = _mouseWorldPosXY;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ParentTransform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _layersToCastOn))
            {
                Debug.DrawRay(ParentTransform.position, Camera.main.transform.forward * _hit.distance, Color.yellow);

                _mouseWorldPositionXYZ = _hit.point;
                ParentTransform.transform.position = _mouseWorldPositionXYZ;
            }
        }
        else
        {
            // disable the update script
            this.enabled = false;
        }
    }




    // logic for what should happen when this gets tapped
    public void PlayTapEvent()
    {
        if (OneTimeUse == false || OneTimeUse == true && _usedSuccesfully == false)
        {
            if (_audioSource != null)
            {
                var randomSound = UnityEngine.Random.Range(0, _audioClipsToPlay.Length - 1);
                _audioSource.PlayOneShot(_audioClipsToPlay[randomSound]);
            }
            if (_animator != null)
            {
                _animator.SetTrigger("Activate");  // !!! create this trigger in every animator that will be made for tap-ables !!!
            }
            if (CanBeMoved == true) // if true, enables update
            {
                _activatedFollowMouse = true;
                this.enabled = true;
            }

            // extra logic
            ExtraBehaviour();


            // if cooldown is present
            if (HasACooldown == true)
            {
                StartCoroutine(ActivateCooldown());
            }
        }
    }



    // override this method for more logic
    public virtual void ExtraBehaviour()
    {

    }



    public IEnumerator ActivateCooldown()
    {
        _onCooldown = true;
        _collider.enabled = false;

        yield return new WaitForSeconds(_cooldownLength);

        _onCooldown = false;
        _collider.enabled = true;
    }
}
