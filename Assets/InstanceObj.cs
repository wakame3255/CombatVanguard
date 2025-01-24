using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceObj : MonoBehaviour
{
    [SerializeField]
    private GameObject _instanceObj;

    [SerializeField]
    private int _instanceCount;
    private void Start()
    {
        for (int i = 0; i < _instanceCount; i++)
        {
            Instantiate(_instanceObj, this.transform);
        }
    }
}
