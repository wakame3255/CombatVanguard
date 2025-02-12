using R3;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

public abstract class CharacterActionBase : MonoBehaviour
{
    [SerializeField, Required]
    [Header("アクションを置く親")]
    protected GameObject _actionPosition;

    protected RotationMove _rotationMove;
    protected PositionMoveAction _moveAction;
    protected AttackAction _attackAction;
    protected CharacterAnimation _characterAnimation;
    protected CharacterStatus _characterStatus;
    protected IApplicationStateChange _characterStateChange;
    protected CompositeDisposable _disposables = new CompositeDisposable();

    protected static readonly Vector3 RESET_DIRECTION = new Vector3(1f, 0, 1f);

    protected virtual void Awake()
    {
        _moveAction = this.CheckComponentMissing<PositionMoveAction>(_actionPosition);
        _attackAction = this.CheckComponentMissing<AttackAction>(_actionPosition);
        _rotationMove = this.CheckComponentMissing<RotationMove>(_actionPosition);
        _characterStatus = this.CheckComponentMissing<CharacterStatus>();
        _characterAnimation = this.CheckComponentMissing<CharacterAnimation>();

        CharacterStateCont characterStateCont = new CharacterStateCont();

        _characterStateChange = characterStateCont;
        new AnimationPresenter(characterStateCont, _characterAnimation);

        _characterStateChange.ApplicationStateChange(characterStateCont.StateDataInformation.NormalStateData);
        _characterStatus.SetAnimationCont(characterStateCont);
        SetInformationComponent();
    }

    protected virtual void Update()
    {
        if (!_characterAnimation.IsAnimation)
        {
            _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.NormalStateData);
        }
    }

    /// <summary>
    /// vecter2の入力をカメラ基準に変換するメソッド
    /// </summary>
    /// <param name="input">移動入力</param>
    /// <param name="axisDir">向いている方向</param>
    /// <returns>移動方向</returns>
    protected Vector3 GetChangeInput(Vector2 input, Vector3 axisDir)
    {
        //axisDirectionを基準にした進行方向
        Vector3 axisForward = Vector3.Scale(axisDir, RESET_DIRECTION.normalized);
        Vector3 inputMoveDirection = axisForward.normalized * input.y - Vector3.Cross(axisDir, transform.up).normalized * input.x;

        return inputMoveDirection;
    }

    protected void SetInformationComponent()
    {
        ISetTransform[] setTransforms = new ISetTransform[] { _moveAction, _rotationMove, _characterAnimation, _attackAction };
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
