using R3;
using System.ComponentModel.DataAnnotations;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField, Required]
    [Header("�A�N�V������u���e")]
    private GameObject _actionPosition;

    private Transform _playerCamera;
    private MoveAction _moveAction;
    private AttackAction _attackAction;

    private static readonly Vector3 RESET_DIRECTION = new Vector3(1f, 0, 1f);

    private void Awake()
    {
        _moveAction = this.CheckComponentMissing<MoveAction>(_actionPosition);
        _attackAction = this.CheckComponentMissing<AttackAction>(_actionPosition);

        _playerCamera = Camera.main.gameObject.transform;
        _moveAction.SetTransform(transform);
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
    .Subscribe(inputXY => _moveAction.DoMove(GetChangeInput(inputXY, _playerCamera.forward)));

        //�U���{�^���̓��͍w��
        inputInformation.ReactivePropertyAttack.Where(isAttack => isAttack).Subscribe(isAttack => _attackAction.DoAction());

        //�W�����v�{�^���̓��͍w��
        inputInformation.ReactivePropertyJump.Where(isJump => isJump).Subscribe(isJump => print("jump"));
    }


    /// <summary>
    /// vecter2�̓��͂��J������ɕϊ����郁�\�b�h
    /// </summary>
    /// <param name="input">�ړ�����</param>
    /// <param name="axisDir">�����Ă������</param>
    /// <returns>�ړ�����</returns>
    private Vector3 GetChangeInput(Vector2 input, Vector3 axisDir)
    {
        print(input+" "+axisDir);
        //axisDirection����ɂ����i�s����
        Vector3 axisForward = Vector3.Scale(axisDir, RESET_DIRECTION.normalized);
        Vector3 inputMoveDirection = axisForward.normalized * input.y - Vector3.Cross(axisDir, transform.up).normalized * input.x;

        print(inputMoveDirection);
        return inputMoveDirection;
    }
}
