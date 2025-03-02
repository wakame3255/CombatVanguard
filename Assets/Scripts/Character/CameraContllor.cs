using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContllor : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _mainCamera;

    [SerializeField]
    private CinemachineVirtualCamera _lockOnCamera;

    [SerializeField]
    private Transform _PlayerPos;

    [SerializeField]
    private LayerMask _hitLayerMask;

    [SerializeField]
    private int _raycastHitCount;

    [SerializeField]
    private float _distance; 

    private RaycastHit[] _raycastHits;

    private void Awake()
    {
        _raycastHits = new RaycastHit[_raycastHitCount];
    }

    private void Update()
    {
        Vector3 cameraDirection = (_PlayerPos.position - _mainCamera.transform.position);

        int hitCount = Physics.BoxCastNonAlloc
            (_mainCamera.transform.position, Vector3.one * _distance, cameraDirection.normalized, _raycastHits, Quaternion.identity, _distance, _hitLayerMask);

        for (int i = 0; i < hitCount; i++)
        {
          
        }
    }
}
