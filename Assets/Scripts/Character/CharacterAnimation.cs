using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    [SerializeField]
    private WalkAnimationInformation _walkAnimationInfo;

    [SerializeField]
    private string _moveInputX;
    [SerializeField]
    private string _moveInputY;

    private Animator _animator;
    private WalkAnimationSystem _walkAnimationSystem;

    private readonly int _moveInputXHash;
    private readonly int _moveInputYHash;

    private void Awake()
    {
        //_moveInputXHash =

        _animator = this.CheckComponentMissing<Animator>();
        _walkAnimationSystem = new(_animator, _walkAnimationInfo);
    }
   
   public void DoMoveAnimation(Vector2 inputXY)
    {
        _walkAnimationSystem.UpdateLocomotion(inputXY.normalized, 1f);
    }
}
