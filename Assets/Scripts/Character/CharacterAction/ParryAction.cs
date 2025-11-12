using System;
using UnityEngine;
using R3;

/// <summary>
/// パリィアクションを管理するクラス
/// ガード状態時に敵の攻撃をパリィして反撃チャンスを作る
/// </summary>
public class ParryAction : MonoBehaviour, ISetTransform, ISetStateCont
{
    /// <summary>
    /// ヒット判定を行うレイヤーマスク
    /// </summary>
    [SerializeField]
    private LayerMask _hitLayeMask;

    /// <summary>
    /// 検出するヒット数の最大値
    /// </summary>
    [SerializeField]
    private int _hitCount;

    /// <summary>
    /// ステート制御インターフェース
    /// </summary>
    private IApplicationStateChange _stateCont;

    /// <summary>
    /// キャラクターのTransform
    /// </summary>
    private Transform _characterTransform;

    /// <summary>
    /// ヒット判定結果を格納する配列
    /// </summary>
    private Collider[] _hitColliders;

    /// <summary>
    /// 初期化処理
    /// ヒット判定用の配列を初期化する
    /// </summary>
    private void Awake()
    {
        _hitColliders = new Collider[_hitCount];
    }

    /// <summary>
    /// キャラクターのTransformを設定する
    /// </summary>
    /// <param name="characterTransform">キャラクターのTransform</param>
    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    /// <summary>
    /// ステートコントロールを設定する
    /// ステート変更を監視してパリィ判定を行う
    /// </summary>
    /// <param name="stateCont">ステート制御インターフェース</param>
    public void SetStateCont(IApplicationStateChange stateCont)
    {
        _stateCont = stateCont;
        stateCont.CurrentStateDataReactiveProperty.Subscribe(stateData => CheckParry(stateData));
    }

    /// <summary>
    /// パリィ判定を行う
    /// ガード状態時に範囲内の敵をチェックし、パリィ可能であればパリィステートに遷移
    /// </summary>
    /// <param name="stateData">現在のステートデータ</param>
    private void CheckParry(StateJudgeBase stateData)
    {
        // ガード状態でない場合は処理しない
        if (!(stateData is GuardStateJudge))
        {
            return;
        }

        // 周囲の敵を検索
        int hitCount = Physics.OverlapSphereNonAlloc(_characterTransform.position, 2f, _hitColliders, _hitLayeMask);

        for (int i = 0; i < hitCount; i++)
        {
            if(!_hitColliders[i].TryGetComponent<CharacterStatus>(out CharacterStatus status))
            {
                return;
            }

            // パリィ成功時、パリィステートに遷移
            if(status.HitParry())
            {
                _stateCont.ApplicationStateChange(_stateCont.StateDataInformation.ParryStateData);
            }
        }
    }
}