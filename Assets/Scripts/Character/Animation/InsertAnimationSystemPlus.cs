using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

/// <summary>
/// アニメーションを動的に挿入するシステムの拡張版
/// Playable APIを使用してアニメーションを動的に再生する
/// </summary>
public class InsertAnimationSystemPlus : IDisposable
{
    /// <summary>
    /// 対象のAnimator
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// PlayableGraphインスタンス
    /// </summary>
    private PlayableGraph _playableGraph;

    /// <summary>
    /// 現在再生中のアニメーションクリップ
    /// </summary>
    private AnimationClip _currentAnimationClip;

    /// <summary>
    /// アニメーションクリップのPlayable
    /// </summary>
    private AnimationClipPlayable _clipPlayable;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="animator">対象のAnimator</param>
    public InsertAnimationSystemPlus(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// プレイヤー本体のワーク解放
    /// リソースを解放してPlayableGraphを破棄する
    /// </summary>
    public void Dispose()
    {
        DebugUtility.Log("プレイアブルの破棄");
        // プレイアブルグラフが生成されず残っているのか
        if (_playableGraph.IsValid())
        {
            // プレイアブルグラフを破棄
            _playableGraph.Destroy();
            // プレイアブルグラフを初期化
            _playableGraph = default;
        }

        _animator = null;
    }

    /// <summary>
    /// アニメーションを挿入して再生する
    /// 同じアニメーションの場合は最初から再生する
    /// </summary>
    /// <param name="animationClip">再生するアニメーションクリップ</param>
    public void InsertAnimation(AnimationClip animationClip)
    {
        // 同じアニメーションの場合の早期リターン
        if (_currentAnimationClip == animationClip)
        {
            // プレイヤー本体のAPIのローカルタイムのセット
            _clipPlayable.SetTime(0);
            // アニメーションが完了していないフラグを立てる
            _clipPlayable.SetDone(false);
            return;
        }

        // 前のグラフを削除
        Stop();

       AnimationClipPlayable clipPlayable = AnimationPlayableUtilities.PlayClip(_animator, animationClip, out _playableGraph);

        clipPlayable.SetTime(0);
        clipPlayable.SetDuration(animationClip.length);
        clipPlayable.SetDone(false);
    }

    /// <summary>
    /// 再生中のアニメーションを停止する
    /// </summary>
    public void Stop()
    {
        if (_clipPlayable.IsValid())
        {
            // プレイアブルグラフを破棄
            _playableGraph.Destroy();
            // プレイアブルグラフを初期化
            _playableGraph = default;
        }

        _currentAnimationClip = null;
    }
}
