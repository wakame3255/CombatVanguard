using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using R3;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour, ISetTransform
{
    [SerializeField]
    private float _walkDamp;

    [SerializeField]
    private WalkTurnAnimationInformation _walkAnimationInfo;
    [SerializeField]
    private AttackAnimationInformation _attackAnimationInfo;
    [SerializeField]
    private InsertAnimationSystem _insertAnimationSystem;

    [SerializeField]
    private string _moveInputXName;
    [SerializeField]
    private string _moveInputYName;
    [SerializeField]
    private string _isDashName = "IsDash";
    [SerializeField]
    private string _isGuard = "IsGuard";

    private Transform _characterTransform;
    private Animator _animator;

    private int _moveInputXHash;
    private int _moveInputYHash;
    private int _isDashHash;

    public bool IsAnimation { get; private set; }

    private void Awake()
    {
        _animator = this.CheckComponentMissing<Animator>();

        _moveInputXHash = Animator.StringToHash(_moveInputXName);
        _moveInputYHash = Animator.StringToHash(_moveInputYName);
        _isDashHash = Animator.StringToHash(_isDashName);

        _insertAnimationSystem.ReactivePropertyIsAnimation.Subscribe(isAnim => IsAnimation = isAnim);
    }
   
   public void DoWalkAnimation(Vector3 moveDirection)
    {
        Vector2 changeInput = GetDirectionToAnimationValue(moveDirection);
        _animator.SetFloat(_moveInputXHash, changeInput.x, _walkDamp, Time.deltaTime);
        _animator.SetFloat(_moveInputYHash, changeInput.y, _walkDamp, Time.deltaTime);

        _animator.SetBool(_isDashHash, false);
    }

    public void DoDashAnimation(Vector3 moveDirection)
    {
        Vector2 changeInput = GetDirectionToAnimationValue(moveDirection);
        _animator.SetFloat(_moveInputXHash, changeInput.x, _walkDamp, Time.deltaTime);
        _animator.SetFloat(_moveInputYHash, changeInput.y, _walkDamp, Time.deltaTime);

        _animator.SetBool(_isDashHash, true);
    }

    public void DoTurnAnimation()
    {
        _insertAnimationSystem.AnimationPlay(_walkAnimationInfo.ForwardTurnAnimation).Forget();
    }
    public void DoAttackAnimation()
    {
        _insertAnimationSystem.AnimationPlay(_attackAnimationInfo.JabAnimation).Forget();
    }
    public void DoHitAnimation()
    {
        _insertAnimationSystem.AnimationPlay(_attackAnimationInfo.HitAnimation).Forget();
    }
    public void DoAvoidanceAnimation()
    {
        _insertAnimationSystem.AnimationPlay(_walkAnimationInfo.AvoidanceAnimation).Forget();
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    private Vector2 GetDirectionToAnimationValue(Vector3 moveDirection)
    {
        MyExtensionClass.CheckArgumentNull(moveDirection, nameof(moveDirection));

        float inputX = Vector3.Dot(moveDirection, _characterTransform.right);
        float inputY = Vector3.Dot(moveDirection, _characterTransform.forward);

        return new Vector2(inputX, inputY);
    }
}


