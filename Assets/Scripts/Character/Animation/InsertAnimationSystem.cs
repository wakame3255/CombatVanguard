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

/// <summary>
/// 攻撃アニメーション情報を保持するクラス
/// ジャブ、被弾、ガード被弾などのアニメーションデータを管理する
/// </summary>
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

    /// <summary>ジャブアニメーション</summary>
    public MatchTargetAnimationData JabAnimation { get => _jabAnimation; }

    /// <summary>ミラージャブアニメーション</summary>
    public MatchTargetAnimationData MirrorJabAnimation { get => _mirrorJabAnimation; }

    /// <summary>被弾アニメーション</summary>
    public MatchTargetAnimationData HitAnimation { get => _hitAnimation; }

    /// <summary>ガード被弾アニメーション</summary>
    public MatchTargetAnimationData GuardHitAnimation { get => _guardHitAnimation; }
}

/// <summary>
/// 割り込みアニメーション情報を保持するクラス
/// ターン、回避、パリィなどのアニメーションデータを管理する
/// </summary>
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

    /// <summary>ターンアニメーション</summary>
    public MatchTargetAnimationData TurnAnimation { get => _turnAnimation; }

    /// <summary>回避アニメーション</summary>
    public MatchTargetAnimationData AvoidanceAnimation { get => _avoidanceAnimation; }

    /// <summary>パリィアニメーション</summary>
    public MatchTargetAnimationData ParryAnimation { get => _parryAnimation; }

    /// <summary>パリィ被弾アニメーション</summary>
    public MatchTargetAnimationData HitParryAnimation { get => _hitParryAnimation; }
}

/// <summary>
/// アニメーションを動的に挿入するシステム
/// Playable APIを使用してアニメーションを滑らかにブレンドしながら再生する
/// </summary>
public class InsertAnimationSystem : MonoBehaviour
{
    /// <summary>
    /// アニメーションのフェードイン/アウトに使用するカーブ
    /// </summary>
    [SerializeField]
    private AnimationCurve _curve;

    /// <summary>
    /// ターゲット位置（攻撃などの目標地点）
    /// </summary>
    [SerializeField]
    private Transform _targetPos;

    /// <summary>
    /// ターゲットマッチ移動コンポーネント
    /// </summary>
    private TargetMatchMove _targetMatchMove;

    /// <summary>
    /// Animatorコンポーネント
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// PlayableGraphインスタンス
    /// </summary>
    private PlayableGraph _playableGraph;

    /// <summary>
    /// 現在のPlayable
    /// </summary>
    private Playable _playable;

    /// <summary>
    /// アニメーション出力
    /// </summary>
    private AnimationPlayableOutput _playableOutput;

    /// <summary>
    /// 非同期処理のキャンセルトークンソース
    /// </summary>
    private CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// アニメーション実行中かどうかのReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyIsAnimation = new ReactiveProperty<bool>(false);

    /// <summary>アニメーション実行中かどうかのプロパティ</summary>
    public ReactiveProperty<bool> ReactivePropertyIsAnimation { get => _reactivePropertyIsAnimation; }

    /// <summary>
    /// 初期化処理
    /// 必要なコンポーネントを取得し、PlayableGraphを初期化する
    /// </summary>
    private void Awake()
    {
        // TargetMatchMoveコンポーネントを追加
        _targetMatchMove = gameObject.AddComponent<TargetMatchMove>();
        // Animatorコンポーネントを取得
        _animator = GetComponent<Animator>();
        // PlayableGraphを初期化
        InitializePlayableGraph();
    }

    /// <summary>
    /// 破棄時の処理
    /// Playableリソースをクリーンアップする
    /// </summary>
    private void OnDestroy()
    {
        // Playableリソースをクリーンアップ
        CleanupPlayable();
    }

    /// <summary>
    /// エディタでの検証処理
    /// カーブが未設定の場合、デフォルトカーブを生成する
    /// </summary>
    private void OnValidate()
    {
        // カーブが設定されていない場合、デフォルトのカーブを生成
        if (_curve == null)
        {
            _curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        }
    }

    /// <summary>
    /// PlayableGraphを初期化する
    /// </summary>
    private void InitializePlayableGraph()
    {
        // PlayableGraphが無効な場合に新規作成
        if (!_playableGraph.IsValid())
        {
            _playableGraph = PlayableGraph.Create();
            _playableOutput = AnimationPlayableOutput.Create(_playableGraph, name, _animator);
            _playableOutput.SetWeight(0f);
        }
    }

    /// <summary>
    /// UniTaskを使用してアニメーションを再生するメソッド
    /// フェードイン/アウトとターゲットマッチングを行いながらアニメーションを再生する
    /// </summary>
    /// <param name="animationClip">再生するアニメーションデータ</param>
    public async UniTask AnimationPlay(MatchTargetAnimationData animationClip)
    {
        // 前回のトークンソースがあれば破棄して新規に作成
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        // アニメーションクリップのnullチェック
        MyExtensionClass.CheckArgumentNull(animationClip, nameof(animationClip));

        // アニメーション中の状態をtrueに設定
        _reactivePropertyIsAnimation.Value = true;

        // PlayableGraphの初期化を確実に行う
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

        // トランジション開始を待機（アニメーション前半の時間）
        await AnimTransition(animationClip.AnimationClip.length / 2f, true, _cancellationTokenSource.Token);

        // PlayableGraphを停止
        _playableGraph.Stop();

        float playTime = animationClip.AnimationClip.length - animationClip.AnimationClip.length;
        // 残りの再生時間があれば待機
        if (playTime > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(playTime));
        }

        // PlayableGraphを再生
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
    /// 挿し込むアニメーションの生成メソッド
    /// </summary>
    /// <param name="animClip">挿し込むアニメーションクリップ</param>
    private void SetupNewPlayable(AnimationClip animClip)
    {
        // 挿し込みアニメーションをPlayableとして作成
        _playable = AnimationClipPlayable.Create(_playableGraph, animClip);

        // Playable出力にセット
        _playableOutput.SetSourcePlayable(_playable);

        // PlayableGraphを再生
        _playableGraph.Play();
    }

    /// <summary>
    /// UniTaskを使用したトランジション処理
    /// フェードイン/アウトを滑らかに行う
    /// </summary>
    /// <param name="duration">トランジションの期間</param>
    /// <param name="isIn">フェードインかどうか</param>
    /// <param name="token">キャンセルトークン</param>
    private async UniTask AnimTransition(float duration, bool isIn, CancellationToken token)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;

        // トランジション中、カーブに基づいてウェイトを調整
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

    /// <summary>
    /// Playableリソースをクリーンアップする
    /// </summary>
    private void CleanupPlayable()
    {
        // Playableが有効な場合は破棄
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        // PlayableGraphが有効な場合は破棄
        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// エディタでの実行停止時にリソースを解放する
    /// </summary>
    private void OnDisable()
    {
        // エディタでの実行停止時にリソースを解放
        CleanupPlayable();
    }
#endif
}
