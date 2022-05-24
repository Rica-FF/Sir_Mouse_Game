using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible_Fireplace : Interactible_Base
{
    [SerializeField]
    private GameObject _spriteFire, _spriteSmoke, _particlePopcorn;


    public void GetDoused()
    {
        StartCoroutine(GettingDoused());
    }

    public void MakePopcorn()
    {
        _particlePopcorn.SetActive(true);
        _particlePopcorn.GetComponent<ParticleSystem>().Play();
    }


    private IEnumerator GettingDoused()
    {
        yield return new WaitForSeconds(0.3f);

        _spriteFire.SetActive(false);
        _spriteSmoke.SetActive(true);
    }

}
