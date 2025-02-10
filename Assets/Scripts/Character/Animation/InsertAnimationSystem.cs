using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using R3;

[Serializable]
public class AttackAnimationInformation
{
    [SerializeField]
    private MatchTargetAnimationData _jabAnimation;

    [SerializeField]
    private MatchTargetAnimationData _hitAnimation;

    public MatchTargetAnimationData JabAnimation { get => _jabAnimation; }
    public MatchTargetAnimationData HitAnimation { get => _hitAnimation; }
}

[Serializable]
public class WalkTurnAnimationInformation
{
    [SerializeField]
    private MatchTargetAnimationData _forwardTurnAnimation;

    [SerializeField]
    private MatchTargetAnimationData _avoidanceAnimation;

    public MatchTargetAnimationData ForwardTurnAnimation { get => _forwardTurnAnimation; }
    public MatchTargetAnimationData AvoidanceAnimation { get => _avoidanceAnimation; }
}

public class InsertAnimationSystem : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private Transform _targetPos;

    private TargetMatchMove _targetMatchMove;
    private Animator _animator;
    private PlayableGraph _playableGraph;
    private Playable _playable;
    private AnimationPlayableOutput _playableOutput;

    private ReactiveProperty<bool> _reactivePropertyIsAnimation = new ReactiveProperty<bool>(false);

    public ReactiveProperty<bool> ReactivePropertyIsAnimation { get => _reactivePropertyIsAnimation; }

    private void Awake()
    {
        // TargetMatchMoveコンポーネントを追加
        _targetMatchMove = gameObject.AddComponent<TargetMatchMove>();
        // Animatorコンポーネントを取得
        _animator = GetComponent<Animator>();
        // Playableグラフを初期化
        InitializePlayableGraph();
    }

    private void OnDestroy()
    {
        // Playableリソースをクリーンアップ
        CleanupPlayable();
    }

    private void OnValidate()
    {
        // カーブが設定されていない場合、デフォルトのカーブを生成
        if (_curve == null)
        {
            _curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        }
    }

    private void InitializePlayableGraph()
    {
        // Playableグラフが無効な場合に新規作成
        if (!_playableGraph.IsValid())
        {
            _playableGraph = PlayableGraph.Create();
            _playableOutput = AnimationPlayableOutput.Create(_playableGraph, name, _animator);
            _playableOutput.SetWeight(0f);
        }
    }

    public IEnumerator AnimationPlay(MatchTargetAnimationData animationClip)
    {
        // アニメーションクリップのnullチェック
        MyExtensionClass.CheckArgumentNull(animationClip, nameof(animationClip));

        // アニメーション中の状態をtrueに設定
        _reactivePropertyIsAnimation.Value = true;

        // Playableグラフの初期化を確実に行う
        InitializePlayableGraph();

        // 既存のPlayableをクリーンアップ
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        // 新しいPlayableを設定
        SetupNewPlayable(animationClip.AnimationClip);
        // ターゲットマッチモーションのデータを設定
        _targetMatchMove.SetMatchTargetAnimationData(animationClip, _targetPos);

        // トランジション開始を待機
        yield return StartTransition(animationClip.AnimationClip.length / 2f, true, animationClip);

        // Playableグラフを停止
        _playableGraph.Stop();

        float playTime = animationClip.AnimationClip.length - (animationClip.AnimationClip.length);

        // 残りの再生時間があれば待機
        if (playTime > 0)
        {
            yield return new WaitForSeconds(playTime);
        }

        // Playableグラフを再生
        _playableGraph.Play();

        // トランジション終了を待機
        yield return EndTransition(animationClip.AnimationClip.length / 2f, false);

        // Playableのクリーンアップを行い、グラフは維持
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }
        // アニメーション中の状態をfalseに設定
        _reactivePropertyIsAnimation.Value = false;
    }

    /// <summary>
    /// 割り込ませるアニメーションの生成メソッド
    /// </summary>
    /// <param name="animClip">割り込ませたいアニメクリップ</param>
    private void SetupNewPlayable(AnimationClip animClip)
    {
        // 割り込みアニメをPlayableとして生成
        _playable = AnimationClipPlayable.Create(_playableGraph, animClip);

        // Playable出力にセット
        _playableOutput.SetSourcePlayable(_playable);
        // Playableグラフを再生
        _playableGraph.Play();
    }

    private IEnumerator StartTransition(float duration, bool isIn, MatchTargetAnimationData animationData)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;

        // トランジション期間中、カーブに基づいてウェイトを調整
        while (Time.timeSinceLevelLoad < endTime)
        {
            float nowTime = (Time.timeSinceLevelLoad - startTime) / duration;
            if (!isIn)
            {
                nowTime = 1 - nowTime;
            }
            _playableOutput.SetWeight(_curve.Evaluate(nowTime));
            yield return null;
        }

        // 最終的なウェイトを確実に設定
        _playableOutput.SetWeight(isIn ? 1f : 0f);
    }

    private IEnumerator EndTransition(float duration, bool isIn)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;

        // トランジション期間中、カーブに基づいてウェイトを調整
        while (Time.timeSinceLevelLoad < endTime)
        {
            float nowTime = (Time.timeSinceLevelLoad - startTime) / duration;
            if (!isIn)
            {
                nowTime = 1 - nowTime;
            }
            _playableOutput.SetWeight(_curve.Evaluate(nowTime));
            yield return null;
        }

        // 最終的なウェイトを確実に設定
        _playableOutput.SetWeight(isIn ? 1f : 0f);
    }

    private void CleanupPlayable()
    {
        // Playableが有効な場合は破棄
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        // Playableグラフが有効な場合は破棄
        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
        }
    }

#if UNITY_EDITOR
    private void OnDisable()
    {
        // エディタでの実行停止時にもリソースを解放
        CleanupPlayable();
    }
#endif
}
