using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMatchMove : MonoBehaviour
{
    private float _animationTime;

    private MatchTargetAnimationData _matchTargetAnimationData;
    private Transform _target;

    private void Update()
    {
        if (_animationTime > 0)
        {
            _animationTime -= Time.deltaTime;
            MoveToTarget();
        }
    }

    public void SetMatchTargetAnimationData(MatchTargetAnimationData matchTargetAnimationData, Transform target)
    {
        _matchTargetAnimationData = matchTargetAnimationData;
        _animationTime = matchTargetAnimationData.AnimationClip.length;
        _target = target;
    }

    private void MoveToTarget()
    {
        if (_matchTargetAnimationData == null || _target == null)
        {
            return;
        }
       
        transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime);
        
    }
}
