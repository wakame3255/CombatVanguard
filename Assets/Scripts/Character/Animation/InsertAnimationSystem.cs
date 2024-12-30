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
   
    public MatchTargetAnimationData JabAnimation { get => _jabAnimation; }
}

[Serializable]
public class WalkTurnAnimationInformation
{
    [SerializeField]
    private MatchTargetAnimationData _forwardTurnAnimation;

    public MatchTargetAnimationData ForwardTurnAnimation { get => _forwardTurnAnimation; }
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
        _targetMatchMove = gameObject.AddComponent<TargetMatchMove>();
        _animator = GetComponent<Animator>();
        InitializePlayableGraph();
    }

    private void OnDestroy()
    {
        CleanupPlayable();
    }

    private void OnValidate()
    {
        if (_curve == null)
        {
            _curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        }
    }

    private void InitializePlayableGraph()
    {
        if (!_playableGraph.IsValid())
        {
            _playableGraph = PlayableGraph.Create();
            _playableOutput = AnimationPlayableOutput.Create(_playableGraph, name, _animator);
            _playableOutput.SetWeight(0f);
        }
    }

    public IEnumerator AnimationPlay(MatchTargetAnimationData animationClip)
    {
        MyExtensionClass.CheckArgumentNull(animationClip, nameof(animationClip));

        Debug.Log("アニメーション開始");
        _reactivePropertyIsAnimation.Value = true;

        // グラフの初期化を確実に行う
        InitializePlayableGraph();

        // 既存のPlayableをクリーンアップ
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        // 新しいPlayableの設定
        SetupNewPlayable(animationClip.AnimationClip);
        _targetMatchMove.SetMatchTargetAnimationData(animationClip, _targetPos);

        yield return StartTransition(animationClip.AnimationClip.length / 2f, true, animationClip);

        _playableGraph.Stop();

        float playTime = animationClip.AnimationClip.length - (animationClip.AnimationClip.length / 2f * 2);

        if (playTime > 0)
        {
            yield return new WaitForSeconds(playTime);
        }

        _playableGraph.Play();
       
        yield return EndTransition(animationClip.AnimationClip.length / 2f, false);

        // Playableのクリーンアップのみを行い、グラフは維持
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        Debug.Log("アニメーション終了");
        _reactivePropertyIsAnimation.Value = false;
    }


    /// <summary>
    /// 割り込ませるアニメーションの生成メソッド
    /// </summary>
    /// <param name="animClip">割り込ませたいアニメクリップ</param>
    private void SetupNewPlayable(AnimationClip animClip)
    {
        //割り込みアニメを生成
        _playable = AnimationClipPlayable.Create(_playableGraph, animClip);

        _playableOutput.SetSourcePlayable(_playable);
        _playableGraph.Play();

        
    }

    private IEnumerator StartTransition(float duration, bool isIn, MatchTargetAnimationData animationData)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;
        print("StartTransition");
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

        // 最終的な重みを確実に設定
        _playableOutput.SetWeight(isIn ? 1f : 0f);
    }

    private IEnumerator EndTransition(float duration, bool isIn)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration;
        print("EndTransition");
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

        // 最終的な重みを確実に設定
        _playableOutput.SetWeight(isIn ? 1f : 0f);
    }

    private void CleanupPlayable()
    {
        if (_playable.IsValid())
        {
            _playable.Destroy();
        }

        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
            Debug.Log("PlayableGraphを破棄しました");
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