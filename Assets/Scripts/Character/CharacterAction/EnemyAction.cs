using R3;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    [SerializeField, Required]
    [Header("�A�N�V������u���e")]
    private GameObject _actionPosition;

    private RotationMove _rotationMove;
    private PositionMoveAction _moveAction;
    private AttackAction _attackAction;
    private CharacterAnimation _characterAnimation;
    private CharacterStatus _characterStatus;
    private CharacterStateCont _characterStateCont;
    private AnimationPresenter _animationPresenter;
    private CompositeDisposable _disposables = new CompositeDisposable();

    private static readonly Vector3 RESET_DIRECTION = new Vector3(1f, 0, 1f);

    private void Awake()
    {
        _moveAction = this.CheckComponentMissing<PositionMoveAction>(_actionPosition);
        _attackAction = this.CheckComponentMissing<AttackAction>(_actionPosition);
        _rotationMove = this.CheckComponentMissing<RotationMove>(_actionPosition);
        _characterStatus = this.CheckComponentMissing<CharacterStatus>();
        _characterAnimation = this.CheckComponentMissing<CharacterAnimation>();

        _characterStateCont = new CharacterStateCont();
        _animationPresenter = new AnimationPresenter(_characterStateCont, _characterAnimation);

        SetInformationComponent();
    }

    void OnDestroy()
    {
        _disposables.Dispose();
    }

    /// <summary>
    /// ���͊֘A���w�ǂ��郁�\�b�h
    /// </summary>
    /// <param name="inputInformation"></param>
    public void SetInputEvent(IInputInformation inputInformation)
    {
        MyExtensionClass.CheckArgumentNull(inputInformation, nameof(inputInformation));

        //���t���[���X�V�̈ړ����͍w��
        Observable.EveryUpdate()
     .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
     .Where(_ => !_characterAnimation.IsAnimation)
     .Subscribe(inputXY => _moveAction.DoMove(GetChangeInput(inputXY, Vector3.forward)))
     .AddTo(_disposables);

        //���t���[���X�V�̌����ύX�X�V
        Observable.EveryUpdate()
     .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
      .Where(_ => !_characterAnimation.IsAnimation)
     .Subscribe(inputXY => _rotationMove.DoRotation(GetChangeInput(inputXY, Vector3.forward)))
     .AddTo(_disposables);

        //�U���{�^���̓��͍w��
        inputInformation.ReactivePropertyAttack.Where(isAttack => isAttack).Where(_ => !_characterAnimation.IsAnimation)
            .Subscribe(isAttack => _attackAction.DoAction())
        .AddTo(_disposables);

        //�W�����v�{�^���̓��͍w��
        inputInformation.ReactivePropertyJump.Where(_ => !_characterAnimation.IsAnimation).Where(isJump => isJump)
            .Subscribe(isJump => _characterAnimation.DoTurnAnimation())
        .AddTo(_disposables);

        //�_�b�V���{�^���̓��͍w��
        Observable.EveryUpdate()
           .WithLatestFrom(inputInformation.ReactivePropertyDash, (_, move) => move)
           .Where(_ => !_characterAnimation.IsAnimation)
           .Subscribe(isDash => _moveAction.SetDashTrigger(isDash))
       .AddTo(_disposables);
    }


    /// <summary>
    /// vecter2�̓��͂��J������ɕϊ����郁�\�b�h
    /// </summary>
    /// <param name="input">�ړ�����</param>
    /// <param name="axisDir">�����Ă������</param>
    /// <returns>�ړ�����</returns>
    private Vector3 GetChangeInput(Vector2 input, Vector3 axisDir)
    {
        //axisDirection����ɂ����i�s����
        Vector3 axisForward = Vector3.Scale(axisDir, RESET_DIRECTION.normalized);
        Vector3 inputMoveDirection = axisForward.normalized * input.y - Vector3.Cross(axisDir, transform.up).normalized * input.x;

        return inputMoveDirection;
    }

    private void SetInformationComponent()
    {
        ISetTransform[] setTransforms = new ISetTransform[] { _moveAction, _rotationMove, _characterAnimation, _attackAction};
        foreach (ISetTransform hasComp in setTransforms)
        {
            hasComp.SetCharacterTransform(transform);
        }

        ISetAnimation[] setAnimations = new ISetAnimation[] { _moveAction, _attackAction, _characterStatus };
        foreach (ISetAnimation hasComp in setAnimations)
        {
            hasComp.SetAnimationComponent(_characterAnimation);
        }
    }
}
