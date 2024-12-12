using R3;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField, Required]
    [Header("アクションを置く親")]
    private GameObject _actionPosition;

    private Transform _playerCamera;
    private RotationMove _rotationMove;
    private MoveAction _moveAction;
    private AttackAction _attackAction;
    private CharacterAnimation _characterAnimation;

    private static readonly Vector3 RESET_DIRECTION = new Vector3(1f, 0, 1f);

    private void Awake()
    {
        _moveAction = this.CheckComponentMissing<MoveAction>(_actionPosition);
        _attackAction = this.CheckComponentMissing<AttackAction>(_actionPosition);
        _rotationMove = this.CheckComponentMissing<RotationMove>(_actionPosition);
        _characterAnimation = this.CheckComponentMissing<CharacterAnimation>();

        _playerCamera = Camera.main.gameObject.transform;
        _moveAction.SetCharacterTransform(transform);
        _rotationMove.SetCharacterTransform(transform);
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
    .Subscribe(inputXY => _moveAction.DoMove(GetChangeInput(inputXY, _playerCamera.forward)));

    //    Observable.EveryUpdate()
    //.WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
    //.Subscribe(inputXY => _rotationMove.DoRotation(GetChangeInput(inputXY, _playerCamera.forward)));

        Observable.EveryUpdate()
   .WithLatestFrom(inputInformation.ReactivePropertyMove, (_, move) => move)
   .Subscribe(inputXY => _characterAnimation.DoMoveAnimation(inputXY));

        //攻撃ボタンの入力購読
        inputInformation.ReactivePropertyAttack.Where(isAttack => isAttack).Subscribe(isAttack => _attackAction.DoAction());

        //ジャンプボタンの入力購読
        inputInformation.ReactivePropertyJump.Where(isJump => isJump).Subscribe(isJump => print("jump"));
    }


    /// <summary>
    /// vecter2の入力をカメラ基準に変換するメソッド
    /// </summary>
    /// <param name="input">移動入力</param>
    /// <param name="axisDir">向いている方向</param>
    /// <returns>移動方向</returns>
    private Vector3 GetChangeInput(Vector2 input, Vector3 axisDir)
    {
        //axisDirectionを基準にした進行方向
        Vector3 axisForward = Vector3.Scale(axisDir, RESET_DIRECTION.normalized);
        Vector3 inputMoveDirection = axisForward.normalized * input.y - Vector3.Cross(axisDir, transform.up).normalized * input.x;

        return inputMoveDirection;
    }
}
