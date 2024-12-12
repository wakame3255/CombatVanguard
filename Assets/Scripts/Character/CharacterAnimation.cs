using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    private WalkAnimationInformation _walkAnimationInfo;

    [SerializeField]
    private string _moveInputXName;
    [SerializeField]
    private string _moveInputYName;

    private Animator _animator;
    private WalkAnimationSystem _walkAnimationSystem;

    private int _moveInputXHash { get; init; }
    private int _moveInputYHash { get; init; }

    public CharacterAnimation()
    {
       
    }

    private void Awake()
    {
        _animator = this.CheckComponentMissing<Animator>();
        _walkAnimationSystem = new(_animator, _walkAnimationInfo);

        //_moveInputXHash = new int(Animator.StringToHash(_moveInputXName));
        //_moveInputYHash = Animator.StringToHash(_moveInputYName);
    }
   
   public void DoMoveAnimation(Vector2 inputXY)
    {
        _walkAnimationSystem.UpdateLocomotion(inputXY.normalized, 1f);
        //_animator.SetFloat(_moveInputXHash, inputXY.x);
        //_animator.SetFloat(_moveInputYHash, inputXY.y);
    }
}


