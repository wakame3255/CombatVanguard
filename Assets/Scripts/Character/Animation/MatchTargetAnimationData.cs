
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

        [Header("比率")]
        [SerializeField]
        private Vector3 _positionWeight = Vector3.one;

        public float StartNormalizedTime { get => _startNormalizedTime; }
        public float EndNormalizedTime { get => _endNormalizedTime; }
        public AvatarTarget TargetBodyPart { get => _targetBodyPart; }
        public Vector3 PositionWeight { get => _positionWeight; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // startTimeがendTimeを超えないようにする
            _endNormalizedTime = Mathf.Max(_startNormalizedTime, _endNormalizedTime);

            // ウェイトを0-1の範囲に制限
            _positionWeight.x = Mathf.Clamp01(_positionWeight.x);
            _positionWeight.y = Mathf.Clamp01(_positionWeight.y);
            _positionWeight.z = Mathf.Clamp01(_positionWeight.z);
        }
#endif
    }

    [SerializeField]
    private AnimationClip _animationClip;

    [SerializeField]
    private StartAnimationTimeList[] _animationTimeList; 

    [Header("対象物")]
    [SerializeField]
    private Vector3 _matchPosition;

    [SerializeField]
    private Vector3 _matchRotationEuler;

    [SerializeField, Range(0, 1)]
    private float _rotationWeight = 0;

    private MatchTargetWeightMask _weightMask;

    // プロパティ
    public AnimationClip AnimationClip {get => _animationClip; }
    public StartAnimationTimeList[] AnimationTimeList { get => _animationTimeList; }
    public Vector3 MatchPosition { get => _matchPosition; }
    public Quaternion MatchRotation { get => Quaternion.Euler(_matchRotationEuler); }
    public MatchTargetWeightMask WeightMask { get => _weightMask; }
    public float RotationWeight { get => _rotationWeight; }
}
