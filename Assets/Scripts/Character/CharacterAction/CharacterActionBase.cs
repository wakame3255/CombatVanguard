using R3;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

public abstract class CharacterActionBase : MonoBehaviour
{
    [SerializeField, Required]
    [Header("�A�N�V������u���e")]
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

        SetCharacterStateCont(new CharacterStateCont());
       
        SetInformationComponent();
    }

    /// <summary>
    /// vecter2�̓��͂��J������ɕϊ����郁�\�b�h
    /// </summary>
    /// <param name="input">�ړ�����</param>
    /// <param name="axisDir">�����Ă������</param>
    /// <returns>�ړ�����</returns>
    protected Vector3 GetChangeInput(Vector2 input, Vector3 axisDir)
    {
        //axisDirection����ɂ����i�s����
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

    /// <summary>
    /// �L�����N�^�[�̏�Ԃ����Z�b�g���郁�\�b�h
    /// </summary>
    /// <param name="isAnimation"></param>
    private void StateReset(bool isAnimation)
    {
        if (isAnimation) return;
        _characterStateChange.ApplicationStateChange(_characterStateChange.StateDataInformation.NormalStateData);       
    }


    /// <summary>
    /// �L�����N�^�[�̏�Ԃ��Ǘ�����N���X�̐���
    /// </summary>
    /// <param name="characterState">�X�e�[�g�Ǘ��N���X</param>
    private void SetCharacterStateCont(CharacterStateCont characterState)
    {
        _characterStateChange = characterState;
        new AnimationPresenter(characterState, _characterAnimation);
        _characterStatus.SetAnimationCont(characterState);
        _characterAnimation.ReactivePropertyIsAnimation.Subscribe(isAnimation => StateReset(isAnimation));
    }
}
