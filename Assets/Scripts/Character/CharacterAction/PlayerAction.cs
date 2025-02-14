using R3;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

public class PlayerAction : CharacterActionBase
{
    private Transform _playerCamera;

    protected override void Awake()
    {
        _playerCamera = Camera.main.gameObject.transform;

        base.Awake();
    }

     void Update()
    {
        _characterStateChange.UpdateDebug();
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
     .Subscribe(inputXY => _moveAction.DoMove(GetChangeInput(inputXY, _playerCamera.forward), _characterStateChange))
     .AddTo(_disposables);

        //���t���[���K�[�h�{�^��,�_�b�V���{�^���̍w��
        Observable.EveryUpdate()
             .Subscribe(_ =>
             {
                 bool isDash = inputInformation.ReactivePropertyDash.Value;
                 bool isGuard = inputInformation.ReactivePropertyGuard.Value;
                 _characterStateChange.CheckMoveState(isDash, isGuard);
             })
             .AddTo(_disposables);

        //���t���[���X�V�̌����ύX�X�V
        Observable.EveryUpdate()
     .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
      .Where(_ => !_characterAnimation.IsAnimation)
     .Subscribe(inputXY => _rotationMove.DoRotation(GetChangeInput(inputXY, _playerCamera.forward)))
     .AddTo(_disposables);

        //�U���{�^���̓��͍w��
        inputInformation.ReactivePropertyAttack.Where(isAttack => isAttack)
            .Where(_ => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AttackStateData))
            .Subscribe(isAttack => _attackAction.DoAction(_characterAnimation.AnimationData.AttackAnimation.JabAnimation))
        .AddTo(_disposables);

        //����̓��͍w��
        inputInformation.ReactivePropertyAvoidance
            .Where(_ => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AvoidanceStateData))
            .Where(isAvoiding => isAvoiding)
            .Subscribe(isAvoiding => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AvoidanceStateData))
        .AddTo(_disposables);
    }

   
}
