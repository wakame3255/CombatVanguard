using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private AnimationClip _animationClip;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックを検出
        {
            _animator.Play("Jab");
        }
    }

    private void AddAnimationClipToAnimator(Animator animator, AnimationClip clip)
    {
        RuntimeAnimatorController rac = animator.runtimeAnimatorController;
        AnimatorOverrideController aoc = new AnimatorOverrideController(rac);
        aoc["DefaultClip"] = clip; // "DefaultClip" はデフォルトのアニメーションクリップ名に置き換えてください
        animator.runtimeAnimatorController = aoc;
    }
}
