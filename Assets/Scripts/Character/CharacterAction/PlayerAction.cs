using R3;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

public class PlayerAction : CharacterActionBase
{
    private Transform _playerCamera;

    private bool _isWalk = default;
    private bool _isDash = default;
    private bool _isGuard = default;

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

        //���t���[���X�V�̌����ύX�X�V
        Observable.EveryUpdate()
     .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
      .Where(_ => !_characterAnimation.IsAnimation)
     .Subscribe(inputXY => _rotationMove.DoRotation(GetChangeInput(inputXY, _playerCamera.forward)))
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
           .Subscribe(isDash => _isDash = isDash)
       .AddTo(_disposables);


        //�K�[�h�{�^���̓��͍w��
        Observable.EveryUpdate()
           .WithLatestFrom(inputInformation.ReactivePropertyGuard, (_, move) => move)
           .Subscribe(isGuard => _isGuard = isGuard)
       .AddTo(_disposables);
    }

    private void CheckState()
    {
        if (_isGuard)
        {
            _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.GuardStateData);
        }
        else if (_isGuard)
        {

        }
    }
}
