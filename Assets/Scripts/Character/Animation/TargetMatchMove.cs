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
            // �A�j���[�V�����̎c�莞�Ԃ����Z
            _animationTime -= Time.deltaTime;

            // �A�j���[�V�����̐i�s�x���v�Z�i0.0 �` 1.0�j
            float normalizedTime = (_matchTargetAnimationData.AnimationClip.length - _animationTime) / _matchTargetAnimationData.AnimationClip.length;

            // �^�C�~���O�ɉ��������������s
            CheckAnimMatchTiming(normalizedTime);
        }
    }

    public void SetMatchTargetAnimationData(MatchTargetAnimationData matchTargetAnimationData, Transform target)
    {
        // �A�j���[�V�����f�[�^�ƃ^�[�Q�b�g��ݒ�
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
            // ���݂̐i�s�x���w��͈͓����m�F
            if (normalizedTime >= timeList.StartNormalizedTime && normalizedTime <= timeList.EndNormalizedTime)
            {
                // �^�[�Q�b�g�ւ̈ړ������s
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

        // ���݂̈ʒu�ƃ^�[�Q�b�g�ʒu�̋������v�Z
        float distance = Vector3.Distance(transform.position, _target.position);
        // �ړ�����X�e�b�v���v�Z
        float step = distance * weight * Time.deltaTime;
        // �^�[�Q�b�g�Ɍ������Ĉړ�
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
    }
}
