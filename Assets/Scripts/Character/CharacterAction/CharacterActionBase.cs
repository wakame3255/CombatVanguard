using R3;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

/// <summary>
/// キャラクターアクションの基底クラス
/// プレイヤーと敵の共通アクション処理を提供する
/// </summary>
public abstract class CharacterActionBase : MonoBehaviour
{
    /// <summary>
    /// アクションを行う位置のゲームオブジェクト
    /// </summary>
    [SerializeField, Required]
    [Header("アクションを行う拠点")]
    protected GameObject _actionPosition;

    /// <summary>
    /// 回転移動コンポーネント
    /// </summary>
    protected RotationMove _rotationMove;

    /// <summary>
    /// 位置移動アクションコンポーネント
    /// </summary>
    protected PositionMoveAction _moveAction;

    /// <summary>
    /// 攻撃アクションコンポーネント
    /// </summary>
    protected AttackAction _attackAction;

    /// <summary>
    /// パリィアクションコンポーネント
    /// </summary>
    protected ParryAction _parryAction;

    /// <summary>
    /// キャラクターアニメーションコンポーネント
    /// </summary>
    protected CharacterAnimation _characterAnimation;

    /// <summary>
    /// キャラクターステータスコンポーネント
    /// </summary>
    protected CharacterStatus _characterStatus;

    /// <summary>
    /// キャラクターステート変更インターフェース
    /// </summary>
    protected IApplicationStateChange _characterStateChange;

    /// <summary>
    /// 購読を管理するDisposable
    /// </summary>
    protected CompositeDisposable _disposables = new CompositeDisposable();

    /// <summary>
    /// 方向リセット用の定数ベクトル
    /// </summary>
    protected static readonly Vector3 RESET_DIRECTION = new Vector3(1f, 0, 1f);

    /// <summary>
    /// 初期化処理
    /// 必要なコンポーネントを取得し、設定する
    /// </summary>
    protected virtual void Awake()
    {
        _moveAction = this.CheckComponentMissing<PositionMoveAction>(_actionPosition);
        _attackAction = this.CheckComponentMissing<AttackAction>(_actionPosition);
        _parryAction = this.CheckComponentMissing<ParryAction>(_actionPosition);
        _rotationMove = this.CheckComponentMissing<RotationMove>(_actionPosition);
        _characterStatus = this.CheckComponentMissing<CharacterStatus>();
        _characterAnimation = this.CheckComponentMissing<CharacterAnimation>();

        SetCharacterStateCont(new CharacterStateCont(_characterAnimation));
       
        SetInformationComponent();
    }

    /// <summary>
    /// Vector2の入力をカメラ軸に変換するメソッド
    /// </summary>
    /// <param name="input">移動入力</param>
    /// <param name="axisDir">基準となる方向</param>
    /// <returns>変換された移動方向</returns>
    protected Vector3 GetChangeInput(Vector2 input, Vector3 axisDir)
    {
        // axisDirを軸にした進行方向
        Vector3 axisForward = Vector3.Scale(axisDir, RESET_DIRECTION.normalized);
        Vector3 inputMoveDirection = axisForward.normalized * input.y - Vector3.Cross(axisDir, transform.up).normalized * input.x;

        return inputMoveDirection;
    }

    /// <summary>
    /// 各コンポーネントに必要な情報を設定する
    /// </summary>
    protected void SetInformationComponent()
    {
        ISetTransform[] setTransforms = new ISetTransform[] 
        { _moveAction, _rotationMove, _characterAnimation, _attackAction, _parryAction};
        foreach (ISetTransform hasComp in setTransforms)
        {
            hasComp.SetCharacterTransform(transform);
        }

        ISetAnimation[] setAnimations = new ISetAnimation[] { _moveAction, _characterStatus };
        foreach (ISetAnimation hasComp in setAnimations)
        {
            hasComp.SetAnimationComponent(_characterAnimation);
        }

        _attackAction.SetStateCont(_characterStateChange);
        _parryAction.SetStateCont(_characterStateChange);
    }

    /// <summary>
    /// キャラクターの状態をリセットするメソッド
    /// アニメーションが終了したら通常状態に戻す
    /// </summary>
    /// <param name="isAnimation">アニメーション実行中かどうか</param>
    private void StateReset(bool isAnimation)
    {
        if (isAnimation) return;
        _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.NormalStateData);       
    }


    /// <summary>
    /// キャラクターの状態を管理するクラスの生成
    /// </summary>
    /// <param name="characterState">ステート管理クラス</param>
    private void SetCharacterStateCont(CharacterStateCont characterState)
    {
        _characterStateChange = characterState;
        _characterStatus.SetAnimationCont(characterState);
        _characterAnimation.ReactivePropertyIsAnimation.Subscribe(isAnimation => StateReset(isAnimation));
    }
}
