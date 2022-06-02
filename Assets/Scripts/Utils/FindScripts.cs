using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindScripts : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _objectsWithScripts = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _objectsWithAnimators = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _objectsOnLayerTap = new List<GameObject>();

    private MonoBehaviour[] _scripts;
    private Animator[] _animators;
    private GameObject[] _objects;

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

        if (Input.GetKeyDown(KeyCode.L))
        {
            _animators = FindObjectsOfType<Animator>();

            for (int i = 0; i <= _animators.Length - 1; i++)
            {
                _objectsWithAnimators.Add(_animators[i].gameObject);

                Debug.Log(_animators[i].gameObject.name + " is an object with a script, counting " + i + " script = " + _animators[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _objects = FindObjectsOfType<GameObject>();

            foreach (var ob in _objects)
            {
                if (ob.layer == 11)
                {
                    _objectsOnLayerTap.Add(ob);
                }
            }
        }
    }


}
