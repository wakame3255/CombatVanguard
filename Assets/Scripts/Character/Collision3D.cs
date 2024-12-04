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

    public void CheckCollision()
    {
        Vector3 capsuleTopPos = _capsuleCollider.center + (Vector3.up * _capsuleCollider.height / 2);
        Vector3 capsuleUnderPos = capsuleTopPos + (Vector3.down * _capsuleCollider.height);
        Vector3 topCycleCenterPos = capsuleTopPos + (Vector3.down * _capsuleCollider.radius);
        Vector3 underCycleCenterPos = capsuleTopPos + (Vector3.up * _capsuleCollider.radius);

        int collisionCount = Physics.CapsuleCastNonAlloc(
            topCycleCenterPos, underCycleCenterPos, _capsuleCollider.radius, Vector3.up, _collisionRaycastHit, 0);

        DoRepulsionCheck(collisionCount);
    } 
    
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

    private void DoRepulsionMove(Vector3 direction, float distance)
    {
        _cacheTransform.position += -direction * distance;
    }
}
