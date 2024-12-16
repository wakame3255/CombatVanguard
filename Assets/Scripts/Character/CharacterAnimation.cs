using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    private float _walkDamp;

    [SerializeField]
    private WalkTurnAnimationInformation _walkAnimationInfo;
    [SerializeField]
    private WalkAnimationSystem _walkAnimationSystem;
    [SerializeField]
    private AnimationClip _animationClip;

    [SerializeField]
    private string _moveInputXName;
    [SerializeField]
    private string _moveInputYName;

    private Transform _characterTransform;
    private Animator _animator;

    private int _moveInputXHash;
    private int _moveInputYHash;

    private void Awake()
    {
        _animator = this.CheckComponentMissing<Animator>();

        _moveInputXHash = Animator.StringToHash(_moveInputXName);
        _moveInputYHash = Animator.StringToHash(_moveInputYName);
    }
   
   public void DoMoveAnimation(Vector3 moveDirection)
    {
        Vector2 changeInput = GetdirectionToAnimationValue(moveDirection);
        _animator.SetFloat(_moveInputXHash, changeInput.x, _walkDamp, Time.deltaTime);
        _animator.SetFloat(_moveInputYHash, changeInput.y, _walkDamp, Time.deltaTime);
    }

    public void DoTurn()
    {
        StartCoroutine(_walkAnimationSystem.AnimationPlay(0.2f, _animationClip));
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    private Vector2 GetdirectionToAnimationValue(Vector3 moveDirection)
    {
        MyExtensionClass.CheckArgumentNull(moveDirection, nameof(moveDirection));

        float inputX = Vector3.Dot(moveDirection, _characterTransform.right);
        float inputY = Vector3.Dot(moveDirection, _characterTransform.forward);

        return new Vector2(inputX, inputY);
    }
}


