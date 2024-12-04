using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Collision3D : MonoBehaviour
{
    [SerializeField]
    private LayerMask _collisionLayer;

    private CapsuleCollider _capsuleCollider;
    private RaycastHit[] _collisionRaycastHit;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public void CollisionCheck()
    {
        Vector3 capsuleTopPos = _capsuleCollider.center + (Vector3.up * _capsuleCollider.height / 2);
        Vector3 capsuleUnderPos = capsuleTopPos + (Vector3.down * _capsuleCollider.height);
        Vector3 topCycleCenterPos = capsuleTopPos + (Vector3.down * _capsuleCollider.radius);
        Vector3 underCycleCenterPos = capsuleTopPos + (Vector3.up * _capsuleCollider.radius);

        Physics.CapsuleCastNonAlloc(
            topCycleCenterPos, underCycleCenterPos, _capsuleCollider.radius, Vector3.up, _collisionRaycastHit, 0);
    } 
}
