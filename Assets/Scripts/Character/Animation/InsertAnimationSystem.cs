using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using R3;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

[Serializable]
public class AttackAnimationInformation
{
    [SerializeField]
    private MatchTargetAnimationData _jabAnimation;

    [SerializeField]
    private MatchTargetAnimationData _mirrorJabAnimation;

    [SerializeField]
    private MatchTargetAnimationData _hitAnimation;

    [SerializeField]
    private MatchTargetAnimationData _guardHitAnimation;

    public MatchTargetAnimationData JabAnimation { get => _jabAnimation; }
    public MatchTargetAnimationData MirrorJabAnimation { get => _mirrorJabAnimation; }
    public MatchTargetAnimationData HitAnimation { get => _hitAnimation; }
    public MatchTargetAnimationData GuardHitAnimation { get => _guardHitAnimation; }
}

[Serializable]
public class InterruptionAnimationInformation
{
    [SerializeField]
    private MatchTargetAnimationData _turnAnimation;

    [SerializeField]
    private MatchTargetAnimationData _avoidanceAnimation;

    [SerializeField]
    private MatchTargetAnimationData _parryAnimation;

    [SerializeField]
    private MatchTargetAnimationData _hitParryAnimation;

    public MatchTargetAnimationData TurnAnimation { get => _turnAnimation; }
    public MatchTargetAnimationData AvoidanceAnimation { get => _avoidanceAnimation; }
    public MatchTargetAnimationData ParryAnimation { get => _parryAnimation; }
    public MatchTargetAnimationData HitParryAnimation { get => _hitParryAnimation; }
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

    private CancellationTokenSource _cancellationTokenSource;


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

    // UniTaskを用いたアニメーション再生メソッド
    public async UniTask AnimationPlay(MatchTargetAnimationData animationClip)
    {
        // 既存のトークンソースがあれば破棄して新規に作成
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

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

            // トランジション開始を待機（アニメーション長の半分の時間）
        await AnimTransition(animationClip.AnimationClip.length / 2f, true, _cancellationTokenSource.Token);

        // Playableグラフを停止
        _playableGraph.Stop();

        float playTime = animationClip.AnimationClip.length - animationClip.AnimationClip.length;
        // 残りの再生時間があれば待機
        if (playTime > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(playTime));
        }

        // Playableグラフを再生
        _playableGraph.Play();

        // トランジション終了を待機
        await AnimTransition(animationClip.AnimationClip.length / 2f, false, _cancellationTokenSource.Token);

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

    // UniTaskを用いたトランジション処理
    private async UniTask AnimTransition(float duration, bool isIn, CancellationToken token)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;

        // トランジション期間中、カーブに基づいてウェイトを調整
        while (Time.timeSinceLevelLoad < endTime)
        {
            token.ThrowIfCancellationRequested();

            float nowTime = (Time.timeSinceLevelLoad - startTime) / duration;
            if (!isIn)
            {
                nowTime = 1 - nowTime;
            }
            if (_playableOutput.IsOutputValid())
            {
                _playableOutput.SetWeight(_curve.Evaluate(nowTime));
            }
           
            await UniTask.Yield();
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