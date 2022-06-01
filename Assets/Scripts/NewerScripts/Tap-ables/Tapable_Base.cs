using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapable_Base : MonoBehaviour
{
    public bool OneTimeUse;
    private bool _usedSuccesfully;

    public bool HasACooldown;
    [SerializeField]
    private float _cooldownLength;

    [SerializeField]
    private AudioClip[] _audioClipsToPlay;

    // components any tap-able would have
    private Animator _animator;
    private AudioSource _audioSource;
    private Collider _collider;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponentInChildren<Collider>();              
    }

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
        _collider.enabled = false;

        yield return new WaitForSeconds(_cooldownLength);
        
        _collider.enabled = true;
    }
}
