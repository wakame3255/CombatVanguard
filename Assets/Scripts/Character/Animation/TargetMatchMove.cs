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
            // アニメーションの残り時間を減算
            _animationTime -= Time.deltaTime;

            // アニメーションの進行度を計算（0.0 ～ 1.0）
            float normalizedTime = (_matchTargetAnimationData.AnimationClip.length - _animationTime) / _matchTargetAnimationData.AnimationClip.length;

            // タイミングに応じた処理を実行
            CheckAnimMatchTiming(normalizedTime);
        }
    }

    public void SetMatchTargetAnimationData(MatchTargetAnimationData matchTargetAnimationData, Transform target)
    {
        // アニメーションデータとターゲットを設定
        _matchTargetAnimationData = matchTargetAnimationData;
        _animationTime = matchTargetAnimationData.AnimationClip.length;
        _target = target;
    }

    /// <summary>
    /// アニメーションのタイミングに合わせて移動するメソッド
    /// </summary>
    private void CheckAnimMatchTiming(float normalizedTime)
    {
        if (_matchTargetAnimationData == null)
        {
            return;
        }
        foreach (MatchTargetAnimationData.StartAnimationTimeList timeList in _matchTargetAnimationData.AnimationTimeList)
        {
            // 現在の進行度が指定範囲内か確認
            if (normalizedTime >= timeList.StartNormalizedTime && normalizedTime <= timeList.EndNormalizedTime)
            {
                // ターゲットへの移動を実行
                MoveToTarget(timeList.MovePositionWeight);
            }
        }
    }

    /// <summary>
    /// ターゲットに向かって移動するメソッド
    /// </summary>
    private void MoveToTarget(float weight)
    {
        if (_matchTargetAnimationData == null || _target == null)
        {
            return;
        }

        // 現在の位置とターゲット位置の距離を計算
        float distance = Vector3.Distance(transform.position, _target.position);
        // 移動するステップを計算
        float step = distance * weight * Time.deltaTime;
        // ターゲットに向かって移動
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
    }
}
