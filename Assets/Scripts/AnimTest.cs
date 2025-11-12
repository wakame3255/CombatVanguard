using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションのテストを行うクラス
/// マウス入力によってアニメーションを再生する
/// </summary>
public class AnimTest : MonoBehaviour
{
    /// <summary>
    /// 制御するアニメーター
    /// </summary>
    [SerializeField]
    private Animator _animator;

    /// <summary>
    /// テスト用アニメーションクリップ
    /// </summary>
    [SerializeField]
    private AnimationClip _animationClip;

    /// <summary>
    /// 開始時に呼ばれる初期化処理
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// 毎フレーム呼ばれる更新処理
    /// マウス左クリックでアニメーションを再生する
    /// </summary>
    void Update()
    {
        // 左クリックされたらジャブアニメーションを再生
        if (Input.GetMouseButtonDown(0))
        {
            _animator.Play("Jab");
        }
    }

    /// <summary>
    /// アニメーターにアニメーションクリップを動的に追加する
    /// </summary>
    /// <param name="animator">対象のアニメーター</param>
    /// <param name="clip">追加するアニメーションクリップ</param>
    private void AddAnimationClipToAnimator(Animator animator, AnimationClip clip)
    {
        RuntimeAnimatorController rac = animator.runtimeAnimatorController;
        AnimatorOverrideController aoc = new AnimatorOverrideController(rac);
        // "DefaultClip" はデフォルトのアニメーションクリップ名に置き換える必要がある
        aoc["DefaultClip"] = clip;
        animator.runtimeAnimatorController = aoc;
    }
}
