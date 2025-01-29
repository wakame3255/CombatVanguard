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

    private void Awake()
    {
        _collisionRaycastHit = new RaycastHit[_collisionCount];
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _cacheTransform = this.transform;
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

        if (collisionCount > 0)
        {
            DoRepulsionCheck(collisionCount);
        }
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
            Transform hitTransform = _collisionRaycastHit[i].transform;

            Physics.ComputePenetration(_collisionRaycastHit[i].collider, hitTransform.transform.position, hitTransform.transform.rotation,
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
}
