
using UnityEngine;
using System;

/// <summary>
/// �A�j���[�V�������̃^�[�Q�b�g�̃}�b�`���O�f�[�^��ێ�����N���X
/// </summary>
[CreateAssetMenu(fileName = "MatchTargetAnimationData", menuName = "Custom/Animation/MatchTargetAnimationData")]
public class MatchTargetAnimationData : ScriptableObject
{
    /// <summary>
    /// �A�j���[�V�������̓��莞�Ԕ͈͂ł̃^�[�Q�b�g�}�b�`���O����ێ�����N���X
    /// </summary>
    [Serializable]
    public class StartAnimationTimeList
    {
        [SerializeField, Range(0, 1)]
        private float _startNormalizedTime = 0; // �A�j���[�V�����J�n���̐��K�����ԁi0�`1�j

        [SerializeField, Range(0, 1)]
        private float _endNormalizedTime = 1; // �A�j���[�V�����I�����̐��K�����ԁi0�`1�j

        [SerializeField]
        private AvatarTarget _targetBodyPart = AvatarTarget.Root; // �^�[�Q�b�g�Ƃ���{�f�B�p�[�c

        [Header("�䗦")]
        [SerializeField, Range(0, 1)]
        private float _movePositionWeight = 1; // �^�[�Q�b�g�ւ̈ړ��̏d��

        /// <summary>
        /// �A�j���[�V�����J�n���̐��K�����Ԃ��擾���܂�
        /// </summary>
        public float StartNormalizedTime { get => _startNormalizedTime; }

        /// <summary>
        /// �A�j���[�V�����I�����̐��K�����Ԃ��擾���܂�
        /// </summary>
        public float EndNormalizedTime { get => _endNormalizedTime; }

        /// <summary>
        /// �^�[�Q�b�g�Ƃ���{�f�B�p�[�c���擾���܂�
        /// </summary>
        public AvatarTarget TargetBodyPart { get => _targetBodyPart; }

        /// <summary>
        /// �^�[�Q�b�g�ւ̈ړ��̏d�݂��擾���܂�
        /// </summary>
        public float MovePositionWeight { get => _movePositionWeight; }

#if UNITY_EDITOR
        /// <summary>
        /// �G�f�B�^��Ńv���p�e�B���ύX���ꂽ�ۂɌĂяo����A�I�����Ԃ��J�n���Ԃ������Ȃ��悤�������܂�
        /// </summary>
        private void OnValidate()
        {
            // startNormalizedTime �� endNormalizedTime �𒴂��Ȃ��悤�ɒ���
            _endNormalizedTime = Mathf.Max(_startNormalizedTime, _endNormalizedTime);
        }
#endif
    }

    [SerializeField]
    private AnimationClip _animationClip; // �ΏۂƂȂ�A�j���[�V�����N���b�v

    [SerializeField]
    private StartAnimationTimeList[] _animationTimeList; // �e���Ԕ͈͂ł̃^�[�Q�b�g�}�b�`���O���̔z��

    [Header("�Ώە�")]
    [SerializeField]
    private Vector3 _matchPosition; // �^�[�Q�b�g�Ƃ���ʒu

    [SerializeField]
    private Vector3 _matchRotationEuler; // �^�[�Q�b�g�Ƃ����]�i�I�C���[�p�j

    [SerializeField, Range(0, 1)]
    private float _rotationWeight = 0; // ��]�̏d��

    private MatchTargetWeightMask _weightMask; // �A�j���[�V�����}�b�`���O�̃E�F�C�g�}�X�N

    /// <summary>
    /// �A�j���[�V�����N���b�v���擾���܂�
    /// </summary>
    public AnimationClip AnimationClip { get => _animationClip; }

    /// <summary>
    /// �A�j���[�V�������ԃ��X�g���擾���܂�
    /// </summary>
    public StartAnimationTimeList[] AnimationTimeList { get => _animationTimeList; }

    /// <summary>
    /// �^�[�Q�b�g�̈ʒu���擾���܂�
    /// </summary>
    public Vector3 MatchPosition { get => _matchPosition; }

    /// <summary>
    /// �^�[�Q�b�g�̉�]���擾���܂��i�N�H�[�^�j�I���`���j
    /// </summary>
    public Quaternion MatchRotation { get => Quaternion.Euler(_matchRotationEuler); }

    /// <summary>
    /// �}�b�`���O���̃E�F�C�g�}�X�N���擾���܂�
    /// </summary>
    public MatchTargetWeightMask WeightMask { get => _weightMask; }

    /// <summary>
    /// ��]�̏d�݂��擾���܂�
    /// </summary>
    public float RotationWeight { get => _rotationWeight; }

    /// <summary>
    /// �X�N���v�g�̏��������ɃE�F�C�g�}�X�N��ݒ肵�܂�
    /// </summary>
    private void Awake()
    {
        // WeightMask�̏������i�ʒu�S�̂��}�X�N���A��]�͎w�肳�ꂽ�d�݂Ń}�X�N�j
        _weightMask = new MatchTargetWeightMask(Vector3.one, _rotationWeight);
    }
}
