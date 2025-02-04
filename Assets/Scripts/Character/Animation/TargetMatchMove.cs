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

            // �A�j���[�V�����̐i�s�x���v�Z�i0.0 �` 1.0�j
            float normalizedTime = (_matchTargetAnimationData.AnimationClip.length - _animationTime) / _matchTargetAnimationData.AnimationClip.length;

            CheckAnimMatchTiming(normalizedTime);
        }
    }

    public void SetMatchTargetAnimationData(MatchTargetAnimationData matchTargetAnimationData, Transform target)
    {
        _matchTargetAnimationData = matchTargetAnimationData;
        _animationTime = matchTargetAnimationData.AnimationClip.length;
        _target = target;
    }

    /// <summary>
    /// �A�j���[�V�����̃^�C�~���O�ɍ��킹�Ĉړ����郁�\�b�h
    /// </summary>
    private void CheckAnimMatchTiming(float normalizedTime)
    {
        if (_matchTargetAnimationData == null)
        {
            return;
        }
        foreach (MatchTargetAnimationData.StartAnimationTimeList timeList in _matchTargetAnimationData.AnimationTimeList)
        {
            if (normalizedTime >= timeList.StartNormalizedTime && normalizedTime <= timeList.EndNormalizedTime)
            {
                MoveToTarget(timeList.MovePositionWeight);
            }
        }
    }

    /// <summary>
    /// �^�[�Q�b�g�Ɍ������Ĉړ����郁�\�b�h
    /// </summary>
    private void MoveToTarget(float weight)
    {
        if (_matchTargetAnimationData == null || _target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _target.position);
        float step = distance * weight * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
    }
}
