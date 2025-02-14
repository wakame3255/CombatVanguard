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
    /// 入力関連を購読するメソッド
    /// </summary>
    /// <param name="inputInformation"></param>
    public void SetInputEvent(IInputInformation inputInformation)
    {
        MyExtensionClass.CheckArgumentNull(inputInformation, nameof(inputInformation));

        //舞フレーム更新の移動入力購読
        Observable.EveryUpdate()
     .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
     .Where(_ => !_characterAnimation.IsAnimation)
     .Subscribe(inputXY => _moveAction.DoMove(GetChangeInput(inputXY, Vector3.forward), _characterStateChange))
     .AddTo(_disposables);

        //毎フレーム更新の向き変更更新
        Observable.EveryUpdate()
     .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
      .Where(_ => !_characterAnimation.IsAnimation)
     .Subscribe(inputXY => _rotationMove.DoRotation(GetChangeInput(inputXY, Vector3.forward)))
     .AddTo(_disposables);

        //攻撃ボタンの入力購読
        inputInformation.ReactivePropertyAttack.Where(isAttack => isAttack)
            .Where(_ => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AttackStateData))
            .Subscribe(isAttack => _attackAction.DoAction(_characterAnimation.AnimationData.AttackAnimation.JabAnimation))
        .AddTo(_disposables);

        //ジャンプボタンの入力購読
        inputInformation.ReactivePropertyAvoidance
            .Where(_ => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AvoidanceStateData))
            .Where(isAvoiding => isAvoiding)
            .Subscribe(isAvoiding => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AvoidanceStateData))
        .AddTo(_disposables);

        //ダッシュボタンの入力購読
        Observable.EveryUpdate()
           .WithLatestFrom(inputInformation.ReactivePropertyDash, (_, move) => move)
           .Where(_ => !_characterAnimation.IsAnimation)
           .Subscribe()
       .AddTo(_disposables);

        //ガードボタン,ダッシュボタンの購読
        Observable.EveryUpdate()
             .Subscribe(_ =>
             {
                 bool isDash = inputInformation.ReactivePropertyDash.Value;
                 bool isGuard = inputInformation.ReactivePropertyGuard.Value;
                 _characterStateChange.CheckMoveState(isDash, isGuard);
             })
             .AddTo(_disposables);
    }
}
