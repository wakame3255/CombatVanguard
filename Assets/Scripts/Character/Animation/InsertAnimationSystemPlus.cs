using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class InsertAnimationSystemPlus : IDisposable
{
    private Animator _animator;

    private PlayableGraph _playableGraph;

    private AnimationClip _currentAnimationClip;

    private AnimationClipPlayable _clipPlayable;

    public InsertAnimationSystemPlus(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// プレあぶるグラフのリークを防ぐ
    /// </summary>
    public void Dispose()
    {
        DebugUtility.Log("プレイアブルの破棄");
        //プレイアブルグラフが消されず残っているのか
        if (_playableGraph.IsValid())
        {
            //プレイアブルグラフを破棄
            _playableGraph.Destroy();
            //プレイアブルグラフを初期化
            _playableGraph = default;
        }

        _animator = null;
    }

    public void InsertAnimation(AnimationClip animationClip)
    {
        //同じアニメーションだった場合の巻き戻し
        if (_currentAnimationClip == animationClip)
        {
            //プレあぶるAPIのローカルタイムのセット
            _clipPlayable.SetTime(0);
            //アニメーションが完了していないフラグを立てる
            _clipPlayable.SetDone(false);
            return;
        }

        //元のグラフを削除
        Stop();

       AnimationClipPlayable clipPlayable = AnimationPlayableUtilities.PlayClip(_animator, animationClip, out _playableGraph);

        clipPlayable.SetTime(0);
        clipPlayable.SetDuration(animationClip.length);
        clipPlayable.SetDone(false);
    }

    /// <summary>
    /// 再生中のアニメーションを停止させる。
    /// </summary>
    public void Stop()
    {
        if (_clipPlayable.IsValid())
        {
            //プレイアブルグラフを破棄
            _playableGraph.Destroy();
            //プレイアブルグラフを初期化
            _playableGraph = default;
        }

        _currentAnimationClip = null;
    }
}
