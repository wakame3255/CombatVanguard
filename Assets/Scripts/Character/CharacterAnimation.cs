using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    private AnimationClip _idleAnimation;

    [SerializeField]
    private AnimationClip _walkAnimation;

    private Animator _animator;
    private AnimationSystem _animationSystem;

    private void Awake()
    {
        _animator = this.CheckComponentMissing<Animator>();
        _animationSystem = new AnimationSystem(_animator, _idleAnimation, _walkAnimation);
    }
    private void Update()
    {
        _animationSystem.UpdateLocomotion(Vector3.forward, 0.00001f);
    }
}
