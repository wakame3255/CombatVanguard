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
    private Vector3 _capsuleTopPos;
    private Vector3 _capsuleUnderPos;

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
    /// コリジョン判定メソッド
    /// </summary>
    public void CheckCollision()
    {
        _capsuleTopPos = _cacheTransform.position + (Vector3.up * _capsuleCollider.height) / 2 + _capsuleCollider.center;
        _capsuleUnderPos = _capsuleTopPos + (Vector3.down * _capsuleCollider.height);
        _topCycleCenterPos = _capsuleTopPos + (Vector3.down * _capsuleCollider.radius);
        _underCycleCenterPos = _capsuleUnderPos + (Vector3.up * _capsuleCollider.radius);

        int collisionCount = Physics.CapsuleCastNonAlloc(
            _topCycleCenterPos, _underCycleCenterPos, _capsuleCollider.radius, Vector3.down, _collisionRaycastHit, 0, _collisionLayer);

        if (collisionCount > 0)
        {
            DoRepulsionCheck(collisionCount);
        }
    }

    /// <summary>
    /// 反発を判断するメソッド
    /// </summary>
    /// <param name="collisionCount">ヒット個数</param>
    private void DoRepulsionCheck(int collisionCount)
    {
        for (int i = 0; i < collisionCount; i++)
        {
            Vector3 colliderDirection = default;
            float colliderDistance = default;
            Transform hitTransform = _collisionRaycastHit[i].transform;

            Physics.ComputePenetration(_collisionRaycastHit[i].collider, hitTransform.position, hitTransform.rotation,
                _capsuleCollider, _cacheTransform.position, _cacheTransform.rotation, out colliderDirection, out colliderDistance);

            DoRepulsionMove(colliderDirection, colliderDistance);
        }
    }

    /// <summary>
    /// 反発分移動させるメソッド
    /// </summary>
    /// <param name="direction">ヒットした角度</param>
    /// <param name="distance">ヒットした距離</param>
    private void DoRepulsionMove(Vector3 direction, float distance)
    {
        if (!float.IsNaN(direction.x) && !float.IsNaN(direction.y) && !float.IsNaN(direction.z) && !float.IsNaN(distance))
        {
            _cacheTransform.position += -direction * distance;
        }
    }
}
