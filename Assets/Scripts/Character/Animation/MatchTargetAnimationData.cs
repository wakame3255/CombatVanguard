
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MatchTargetAnimationData", menuName = "Animation/MatchTargetAnimationData")]
public class MatchTargetAnimationData : ScriptableObject
{
    [Serializable]
    public class StartAnimationTimeList
    {
        [SerializeField, Range(0, 1)]
        private float _startNormalizedTime = 0;

        [SerializeField, Range(0, 1)]
        private float _endNormalizedTime = 1;

        [SerializeField]
        private AvatarTarget _targetBodyPart = AvatarTarget.Root;

        [Header("�䗦")]
        [SerializeField, Range(0, 1)]
        private float _movePositionWeight = 1;

        public float StartNormalizedTime { get => _startNormalizedTime; }
        public float EndNormalizedTime { get => _endNormalizedTime; }
        public AvatarTarget TargetBodyPart { get => _targetBodyPart; }
        public float MovePositionWeight { get => _movePositionWeight; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // startTime��endTime�𒴂��Ȃ��悤�ɂ���
            _endNormalizedTime = Mathf.Max(_startNormalizedTime, _endNormalizedTime);
        }
#endif
    }

    [SerializeField]
    private AnimationClip _animationClip;

    [SerializeField]
    private StartAnimationTimeList[] _animationTimeList; 

    [Header("�Ώە�")]
    [SerializeField]
    private Vector3 _matchPosition;

    [SerializeField]
    private Vector3 _matchRotationEuler;

    [SerializeField, Range(0, 1)]
    private float _rotationWeight = 0;

    private MatchTargetWeightMask _weightMask;

    // �v���p�e�B
    public AnimationClip AnimationClip {get => _animationClip; }
    public StartAnimationTimeList[] AnimationTimeList { get => _animationTimeList; }
    public Vector3 MatchPosition { get => _matchPosition; }
    public Quaternion MatchRotation { get => Quaternion.Euler(_matchRotationEuler); }
    public MatchTargetWeightMask WeightMask { get => _weightMask; }
    public float RotationWeight { get => _rotationWeight; }
}
