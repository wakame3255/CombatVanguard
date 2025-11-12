using R3;
using UnityEngine;

/// <summary>
/// 敵の行動を管理するクラス
/// 入力情報を受け取り、移動、攻撃、回避などのアクションを実行する
/// </summary>
public class EnemyAction : CharacterActionBase
{
    /// <summary>
    /// 初期化処理
    /// 基底クラスの初期化を呼び出す
    /// </summary>
    protected override void Awake()
    {
       base.Awake();
    }

    /// <summary>
    /// 毎フレーム呼ばれる更新処理
    /// デバッグ用の更新処理（現在はコメントアウト）
    /// </summary>
    private void Update()
    {
        //_characterStateChange.UpdateDebug();
    }

    /// <summary>
    /// 破棄時の処理
    /// 購読を解除する
    /// </summary>
    void OnDestroy()
    {
        _disposables.Dispose();
    }

    /// <summary>
    /// 入力関連を登録するメソッド
    /// 敵の各種入力イベントを監視し、対応するアクションを実行する
    /// </summary>
    /// <param name="inputInformation">入力情報インターフェース</param>
    public void SetInputEvent(IInputInformation inputInformation)
    {
        MyExtensionClass.CheckArgumentNull(inputInformation, nameof(inputInformation));

        // 毎フレーム更新の移動入力購読
        Observable.EveryUpdate()
     .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
     .Where(_ => !_characterAnimation.IsAnimation)
     .Subscribe(inputXY => _moveAction.DoMove(GetChangeInput(inputXY, Vector3.forward), _characterStateChange))
     .AddTo(_disposables);

        // 毎フレーム更新の向き変更更新
        Observable.EveryUpdate()
     .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
      .Where(_ => !_characterAnimation.IsAnimation)
     .Subscribe(inputXY => _rotationMove.CheckRotation(GetChangeInput(inputXY, Vector3.forward)))
     .AddTo(_disposables);

        // 攻撃ボタンの入力購読
        inputInformation.ReactivePropertyAttack.Where(isAttack => isAttack)
            .Where(_ => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AttackStateData))
            .Subscribe(isAttack => _attackAction.DoAction(_characterAnimation.AnimationData.AttackAnimation.JabAnimation))
        .AddTo(_disposables);

        // ジャンプボタンの入力購読（回避）
        inputInformation.ReactivePropertyAvoidance
            .Where(_ => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AvoidanceStateData))
            .Where(isAvoiding => isAvoiding)
            .Subscribe(isAvoiding => _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.AvoidanceStateData))
        .AddTo(_disposables);

        // ダッシュボタンの入力購読
        Observable.EveryUpdate()
           .WithLatestFrom(inputInformation.ReactivePropertyDash, (_, move) => move)
           .Where(_ => !_characterAnimation.IsAnimation)
           .Subscribe()
       .AddTo(_disposables);

        // ガードボタン、ダッシュボタンの購読
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
