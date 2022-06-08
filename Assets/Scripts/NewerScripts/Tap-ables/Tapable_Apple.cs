using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapable_Apple : Tapable_Base
{
    private Vector3 _previousScale;

    public override void ExtraBehaviour()
    {       
        base.ExtraBehaviour();

        if (SpawnsAPrefab == false)
        {
            _previousScale = transform.localScale;

            transform.localScale = new Vector3(_previousScale.x + 0.05f, _previousScale.y + 0.05f, _previousScale.z + 0.05f);
        }
    }
}
