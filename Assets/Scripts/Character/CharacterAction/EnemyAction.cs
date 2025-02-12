using R3;
using UnityEngine;

public class EnemyAction : CharacterActionBase
{

    protected override void Awake()
    {
       base.Awake();
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
        inputInformation.ReactivePropertyAttack.Where(isAttack => isAttack)
            .Where(_ => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AttackStateData))
            .Subscribe(isAttack => _attackAction.DoAction())
        .AddTo(_disposables);

        //�W�����v�{�^���̓��͍w��
        inputInformation.ReactivePropertyAvoidance
            .Where(_ => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AvoidanceStateData))
            .Where(isAvoiding => isAvoiding)
            .Subscribe(isAvoiding => _characterAnimation.DoAnimation(_characterAnimation.InterruptionAnimationInfo.AvoidanceAnimation))
        .AddTo(_disposables);

        //�_�b�V���{�^���̓��͍w��
        Observable.EveryUpdate()
           .WithLatestFrom(inputInformation.ReactivePropertyDash, (_, move) => move)
           .Where(_ => !_characterAnimation.IsAnimation)
           .Subscribe(isDash => _moveAction.SetDashTrigger(isDash))
       .AddTo(_disposables);
    }
}
