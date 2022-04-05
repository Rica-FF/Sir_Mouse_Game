using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindScripts : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _objectsWithScripts = new List<GameObject>();

    private MonoBehaviour[] _scripts;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _scripts = FindObjectsOfType<MonoBehaviour>();

            for (int i = 0; i <= _scripts.Length-1; i++)
            {
                _objectsWithScripts.Add(_scripts[i].gameObject);

                Debug.Log(_scripts[i].gameObject.name + " is an object with a script, counting " + i + " script = " +_scripts[i]);
            }
        }
    }


}
