using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

/// <summary>
/// Playable APIを使用したアニメーションシステム
/// ロコモーション（移動）とワンショットアニメーションを管理する
/// 現在は未使用の可能性あり
/// </summary>
public class AnimationSystem : MonoBehaviour
{
    /// <summary>
    /// PlayableGraphのインスタンス
    /// </summary>
    PlayableGraph _playableGraph;

    /// <summary>
    /// トップレベルのミキサー
    /// </summary>
    readonly AnimationMixerPlayable topLevelMixer;

    /// <summary>
    /// ロコモーション（移動）用のミキサー
    /// </summary>
    readonly AnimationMixerPlayable locomotionMixer;

    /// <summary>
    /// ワンショット再生用のPlayable
    /// </summary>
    AnimationClipPlayable oneShotPlayable;

    /// <summary>
    /// コンストラクタ
    /// PlayableGraphを構築し、アニメーションシステムを初期化する
    /// </summary>
    /// <param name="animator">対象のAnimator</param>
    /// <param name="idleClip">待機アニメーション</param>
    /// <param name="walkClip">歩行アニメーション</param>
    public AnimationSystem(Animator animator, AnimationClip idleClip, AnimationClip walkClip)
    {
        _playableGraph = PlayableGraph.Create("AnimationSystem");

        AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(_playableGraph, "Animation", animator);

        topLevelMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
        playableOutput.SetSourcePlayable(topLevelMixer);

        locomotionMixer = AnimationMixerPlayable.Create(_playableGraph, 2);
        topLevelMixer.ConnectInput(0, locomotionMixer, 0);
        _playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);

        AnimationClipPlayable idlePlayable = AnimationClipPlayable.Create(_playableGraph, idleClip);
        AnimationClipPlayable walkPlayable = AnimationClipPlayable.Create(_playableGraph, walkClip);

        idlePlayable.GetAnimationClip().wrapMode = WrapMode.Loop;
        walkPlayable.GetAnimationClip().wrapMode = WrapMode.Loop;

        locomotionMixer.ConnectInput(0, idlePlayable, 0);
        locomotionMixer.ConnectInput(1, walkPlayable, 0);

        _playableGraph.Play();
    }

    /// <summary>
    /// ロコモーションのブレンドを更新する
    /// 速度に応じて待機と歩行アニメーションをブレンドする
    /// </summary>
    /// <param name="velocity">現在の速度</param>
    /// <param name="maxSpeed">最大速度</param>
    public void UpdateLocomotion(Vector3 velocity, float maxSpeed)
    {
        float weight = Mathf.InverseLerp(0f, maxSpeed, velocity.magnitude);
        locomotionMixer.SetInputWeight(0, 1 - weight);
        locomotionMixer.SetInputWeight(1, weight);
    }

    /// <summary>
    /// PlayableGraphを破棄する
    /// </summary>
    public void Destroy()
    {
        if (_playableGraph.IsValid())
        {
            _playableGraph.Destroy();
        }
    }
}
