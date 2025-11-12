using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションに合わせてターゲットに向かって移動するクラス
/// アニメーションの特定のタイミングでターゲットに近づく動作を実現する
/// </summary>
public class TargetMatchMove : MonoBehaviour
{
    /// <summary>
    /// アニメーションの残り時間
    /// </summary>
    private float _animationTime;

    /// <summary>
    /// マッチターゲットアニメーションデータ
    /// </summary>
    private MatchTargetAnimationData _matchTargetAnimationData;

    /// <summary>
    /// 移動先のターゲット
    /// </summary>
    private Transform _target;

    /// <summary>
    /// 毎フレーム呼ばれる更新処理
    /// アニメーション時間を更新し、タイミングをチェックする
    /// </summary>
    private void Update()
    {
        if (_animationTime > 0)
        {
            // アニメーションの残り時間を減算
            _animationTime -= Time.deltaTime;

            // アニメーションの進捗度を計算（0.0 ～ 1.0）
            float normalizedTime = (_matchTargetAnimationData.AnimationClip.length - _animationTime) / _matchTargetAnimationData.AnimationClip.length;

            // タイミングに応じた移動を行う
            CheckAnimMatchTiming(normalizedTime);
        }
    }

    /// <summary>
    /// マッチターゲットアニメーションデータを設定する
    /// </summary>
    /// <param name="matchTargetAnimationData">アニメーションデータ</param>
    /// <param name="target">移動先のターゲット</param>
    public void SetMatchTargetAnimationData(MatchTargetAnimationData matchTargetAnimationData, Transform target)
    {
        // アニメーションデータとターゲットを設定
        _matchTargetAnimationData = matchTargetAnimationData;
        _animationTime = matchTargetAnimationData.AnimationClip.length;
        _target = target;
    }

    /// <summary>
    /// アニメーションのタイミングに合わせて移動するメソッド
    /// 指定された時間範囲内でターゲットへの移動を実行する
    /// </summary>
    /// <param name="normalizedTime">アニメーションの正規化時間（0.0～1.0）</param>
    private void CheckAnimMatchTiming(float normalizedTime)
    {
        if (_matchTargetAnimationData == null)
        {
            return;
        }
        foreach (MatchTargetAnimationData.StartAnimationTimeList timeList in _matchTargetAnimationData.AnimationTimeList)
        {
            // 現在の進捗度が指定範囲内か確認
            if (normalizedTime >= timeList.StartNormalizedTime && normalizedTime <= timeList.EndNormalizedTime)
            {
                // ターゲットへの移動を実行
                MoveToTarget(timeList.MovePositionWeight);
            }
        }
    }

    /// <summary>
    /// ターゲットに向かって移動するメソッド
    /// 重み付けに応じて滑らかに接近する
    /// </summary>
    /// <param name="weight">移動の重み（速度に影響）</param>
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
