using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCorrector_Spawnable : MonoBehaviour
{
    private Rigidbody _rigid;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();

        this.enabled = false;
    }

    void FixedUpdate()
    {
        transform.localEulerAngles = new Vector3(30, 0, transform.localEulerAngles.z);
    }




    // called from the override event on pointer_x
    public IEnumerator StopPhysicsUpdate(float timeActive)
    {
        yield return new WaitForSeconds(timeActive);

        _rigid.isKinematic = true;
        _rigid.useGravity = false;

        this.enabled = false;
    }
}
