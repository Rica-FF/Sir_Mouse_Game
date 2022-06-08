using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapable_Base : MonoBehaviour
{
    // usage values
    public bool OneTimeUse;
    private bool _usedSuccesfully;
    public bool HasACooldown;
    private bool _onCooldown;
    [SerializeField]
    private float _cooldownLength;

    // components any tap-able would have
    private Animator _animator;
    private AudioSource _audioSource;
    private Collider _collider;

    // values for if it spawns objects
    public bool SpawnsAPrefab;
    public bool CanBeMoved;
    public bool SpawnedObjectHasPhysics;

    public GameObject PrefabToSpawn;
    private GameObject _spawnedPrefab;

    // values for following mouse
    private bool _activatedFollowMouse;
    private Vector3 _mousePosition;
    private Vector3 _mouseWorldPosXY;
    private Vector3 _mouseWorldPositionXYZ;

    // etc
    [SerializeField]
    private LayerMask _layersToCastOn;
    private RaycastHit _hit;

    [HideInInspector]
    public Transform ParentTransform;

    [SerializeField]
    private AudioClip[] _audioClipsToPlay;

    [HideInInspector]
    public List<GameObject> SpawnedObjects = new List<GameObject>();
    private int SpawnLimit = 5;


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
            LetGoOfMouse();
        }
        if (_activatedFollowMouse == true)
        {
            FollowMouseCalculations();
        }
        else
        {
            // disable the update script
            this.enabled = false;
        }
    }






    private void LetGoOfMouse()
    {
        // set mouse movement off let go of mouse
        _activatedFollowMouse = false;

        // activate physics corrector
        PhysicsCorrector_Spawnable pxCorrector = null;
        Rigidbody rigid = null;
        if (_spawnedPrefab != null && SpawnedObjectHasPhysics == true)
        {
            // if there's physics present, calculate an offset so the object falls a litte bit
            Vector3 direction = -Camera.main.transform.forward;
            Vector3 calcualtedExtraDistance = direction * 5;
            _spawnedPrefab.transform.position = _spawnedPrefab.transform.position + calcualtedExtraDistance;

            pxCorrector = _spawnedPrefab.GetComponent<PhysicsCorrector_Spawnable>();
            pxCorrector.enabled = true;

            rigid = _spawnedPrefab.GetComponent<Rigidbody>();
            rigid.useGravity = true;
            rigid.isKinematic = false;
        }
    }

    private void FollowMouseCalculations()
    {
        _mousePosition = Input.mousePosition;
        _mouseWorldPosXY = Camera.main.ScreenToWorldPoint(_mousePosition);

        // check if it's the object itself or a spawned one
        if (SpawnsAPrefab == true)
        {
            _spawnedPrefab.transform.position = _mouseWorldPosXY;
            if (Physics.Raycast(_spawnedPrefab.transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _layersToCastOn))
            {
                //Debug.DrawRay(ParentTransform.position, Camera.main.transform.forward * _hit.distance, Color.yellow);
                _mouseWorldPositionXYZ = _hit.point;
                _spawnedPrefab.transform.position = _mouseWorldPositionXYZ;
            }
        }
        else
        {
            ParentTransform.position = _mouseWorldPosXY;
            if (Physics.Raycast(ParentTransform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _layersToCastOn))
            {
                //Debug.DrawRay(ParentTransform.position, Camera.main.transform.forward * _hit.distance, Color.yellow);
                _mouseWorldPositionXYZ = _hit.point;
                ParentTransform.transform.position = _mouseWorldPositionXYZ;
            }
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

            if (SpawnsAPrefab == true)
            {              
                SpawnedObjecLogic();
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


    private void SpawnedObjecLogic()
    {
        // might be better to use object pooling here...
        _spawnedPrefab = Instantiate(PrefabToSpawn, _mouseWorldPosXY, Quaternion.Euler(30, 0, 0));
                
        // list addition
        SpawnedObjects.Add(_spawnedPrefab);

        // remove the object (limited for performance/memory)
        if (SpawnedObjects.Count >= SpawnLimit)
        {
            Destroy(SpawnedObjects[0]);
            SpawnedObjects.RemoveAt(0);
        }
    }
}
