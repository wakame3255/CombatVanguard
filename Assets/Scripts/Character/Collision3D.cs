using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Collision3D : MonoBehaviour
{
    [SerializeField]
    private LayerMask _collisionLayer;

    [SerializeField]
    private int _collisionCount;

    private CapsuleCollider _capsuleCollider;
    private RaycastHit[] _collisionRaycastHit;
    private Transform _cacheTransform;

    private Vector3 _topCycleCenterPos;
    private Vector3 _underCycleCenterPos;

    private bool _isDrowGizmo = default;

    private void Awake()
    {
        _collisionRaycastHit = new RaycastHit[_collisionCount];
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _cacheTransform = this.transform;
        _isDrowGizmo = true;
    }

    private void FixedUpdate()
    {
        CheckCollision();
    }

    /// <summary>
    /// �R���W�������胁�\�b�h
    /// </summary>
    public void CheckCollision()
    {
        Vector3 capsuleTopPos = _cacheTransform.position + (Vector3.up * _capsuleCollider.height) / 2 + _capsuleCollider.center;
        Vector3 capsuleUnderPos = capsuleTopPos + (Vector3.down * _capsuleCollider.height);
        _topCycleCenterPos = capsuleTopPos + (Vector3.down * _capsuleCollider.radius);
        _underCycleCenterPos = capsuleUnderPos + (Vector3.up * _capsuleCollider.radius);

        int collisionCount = Physics.CapsuleCastNonAlloc(
            _topCycleCenterPos, _underCycleCenterPos, _capsuleCollider.radius, Vector3.down, _collisionRaycastHit, 0, _collisionLayer);

        DoRepulsionCheck(collisionCount);
    } 
    
    /// <summary>
    /// �����𔻒f���郁�\�b�h
    /// </summary>
    /// <param name="collisionCount">�q�b�g��</param>
    private void DoRepulsionCheck(int collisionCount)
    {      
        for (int i = 0; i < collisionCount; i++)
        {
            Vector3 colliderDirection = default;
            float colliderDistance = default;

            Physics.ComputePenetration(_collisionRaycastHit[i].collider, _collisionRaycastHit[i].transform.position, _collisionRaycastHit[i].transform.rotation,
                _capsuleCollider, _cacheTransform.position, _cacheTransform.rotation, out colliderDirection, out colliderDistance);

            DoRepulsionMove(colliderDirection, colliderDistance);
        }
    }

    /// <summary>
    /// �������ړ������郁�\�b�h
    /// </summary>
    /// <param name="direction">�q�b�g�����p�x</param>
    /// <param name="distance">�q�b�g��������</param>
    private void DoRepulsionMove(Vector3 direction, float distance)
    {
        _cacheTransform.position += -direction * distance;
    }

    //private void OnDrawGizmos()
    //{
    //    if (_isDrowGizmo)
    //    {
    //        Gizmos.DrawSphere(_topCycleCenterPos, _capsuleCollider.radius);
    //        Gizmos.DrawSphere(_underCycleCenterPos, _capsuleCollider.radius);
    //    }
    //}
}
