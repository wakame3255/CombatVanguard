using UnityEngine;
using System;

/// <summary>
/// アニメーション中のターゲットのマッチングデータを保持するクラス
/// アニメーションの特定フレームでターゲット位置に合わせる処理を定義する
/// </summary>
[CreateAssetMenu(fileName = "MatchTargetAnimationData", menuName = "Custom/Animation/MatchTargetAnimationData")]
public class MatchTargetAnimationData : ScriptableObject
{
    /// <summary>
    /// アニメーション中の特定時間範囲でのターゲットマッチング情報を保持するクラス
    /// </summary>
    [Serializable]
    public class StartAnimationTimeList
    {
        /// <summary>アニメーション開始時の正規化時間（0～1）</summary>
        [SerializeField, Range(0, 1)]
        private float _startNormalizedTime = 0;

        /// <summary>アニメーション終了時の正規化時間（0～1）</summary>
        [SerializeField, Range(0, 1)]
        private float _endNormalizedTime = 1;

        /// <summary>ターゲットとするボディパーツ</summary>
        [SerializeField]
        private AvatarTarget _targetBodyPart = AvatarTarget.Root;

        /// <summary>ターゲットへの移動の重み</summary>
        [Header("比率")]
        [SerializeField, Range(0, 1)]
        private float _movePositionWeight = 1;

        /// <summary>アニメーション開始時の正規化時間を取得します</summary>
        public float StartNormalizedTime { get => _startNormalizedTime; }

        /// <summary>アニメーション終了時の正規化時間を取得します</summary>
        public float EndNormalizedTime { get => _endNormalizedTime; }

        /// <summary>ターゲットとするボディパーツを取得します</summary>
        public AvatarTarget TargetBodyPart { get => _targetBodyPart; }

        /// <summary>ターゲットへの移動の重みを取得します</summary>
        public float MovePositionWeight { get => _movePositionWeight; }

#if UNITY_EDITOR
        /// <summary>
        /// エディタでプロパティが変更された際に呼び出され、終了時間が開始時間より小さくならないように調整します
        /// </summary>
        private void OnValidate()
        {
            // startNormalizedTime が endNormalizedTime を超えないように調整
            _endNormalizedTime = Mathf.Max(_startNormalizedTime, _endNormalizedTime);
        }
#endif
    }

    /// <summary>対象となるアニメーションクリップ</summary>
    [SerializeField]
    private AnimationClip _animationClip;

    /// <summary>各時間範囲でのターゲットマッチング情報の配列</summary>
    [SerializeField]
    private StartAnimationTimeList[] _animationTimeList;

    /// <summary>ターゲットとする位置</summary>
    [Header("対象値")]
    [SerializeField]
    private Vector3 _matchPosition;

    /// <summary>ターゲットとする回転（オイラー角）</summary>
    [SerializeField]
    private Vector3 _matchRotationEuler;

    /// <summary>回転の重み</summary>
    [SerializeField, Range(0, 1)]
    private float _rotationWeight = 0;

    /// <summary>アニメーションマッチングのウェイトマスク</summary>
    private MatchTargetWeightMask _weightMask;

    /// <summary>アニメーションクリップを取得します</summary>
    public AnimationClip AnimationClip { get => _animationClip; }

    /// <summary>アニメーション時間リストを取得します</summary>
    public StartAnimationTimeList[] AnimationTimeList { get => _animationTimeList; }

    /// <summary>ターゲットの位置を取得します</summary>
    public Vector3 MatchPosition { get => _matchPosition; }

    /// <summary>ターゲットの回転を取得します（クォータニオン形式）</summary>
    public Quaternion MatchRotation { get => Quaternion.Euler(_matchRotationEuler); }

    /// <summary>マッチング用のウェイトマスクを取得します</summary>
    public MatchTargetWeightMask WeightMask { get => _weightMask; }

    /// <summary>回転の重みを取得します</summary>
    public float RotationWeight { get => _rotationWeight; }

    /// <summary>
    /// スクリプトの初期化時にウェイトマスクを設定します
    /// </summary>
    private void Awake()
    {
        // WeightMaskの初期化（位置は全てをマスク、回転は指定された重みでマスク）
        _weightMask = new MatchTargetWeightMask(Vector3.one, _rotationWeight);
    }
}
