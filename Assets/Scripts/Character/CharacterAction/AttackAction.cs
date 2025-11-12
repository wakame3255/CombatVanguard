
using UnityEngine;
using R3;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

/// <summary>
/// 攻撃アクションを管理するクラス
/// アニメーションのタイミングに合わせて攻撃判定を行う
/// </summary>
public class AttackAction : MonoBehaviour, ISetTransform, ISetStateCont
{
    /// <summary>
    /// ヒット判定を行うレイヤーマスク
    /// </summary>
    [SerializeField]
    private LayerMask _hitLayerMask;

    /// <summary>
    /// キャラクターのTransform
    /// </summary>
    private Transform _characterTransform;

    /// <summary>
    /// 購読を管理するDisposable
    /// </summary>
    private CompositeDisposable _disposables = new CompositeDisposable();

    /// <summary>
    /// 非同期処理のキャンセルトークンソース
    /// </summary>
    private CancellationTokenSource _cancellationTokenSource;

    /// <summary>
    /// レイキャスト結果を格納する配列
    /// </summary>
    private RaycastHit[] _raycastHits;

    /// <summary>
    /// 初期化処理
    /// キャンセルトークンと配列を初期化する
    /// </summary>
    private void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _raycastHits = new RaycastHit[1];
    }


    /// <summary>
    /// 攻撃を実行するクラス
    /// アニメーションに沿った攻撃判定を行う
    /// </summary>
    /// <param name="animationClip">攻撃アニメーションデータ</param>
    public void DoAction(MatchTargetAnimationData animationClip)
    {
        List<Collider> hitList = new List<Collider>();
        _ = DoActionAsync(animationClip, _cancellationTokenSource.Token, hitList);
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
    /// ステート変更を監視して攻撃をキャンセルする
    /// </summary>
    /// <param name="characterStateCont">ステート制御インターフェース</param>
    public void SetStateCont(IApplicationStateChange characterStateCont)
    {
       characterStateCont.CurrentStateDataReactiveProperty.Subscribe(stateData => UpDateState(stateData))
            .AddTo(_disposables);
    }

    /// <summary>
    /// 攻撃アニメーションの実行中に攻撃判定を行う非同期処理
    /// </summary>
    /// <param name="animationClip">アニメーションデータ</param>
    /// <param name="token">キャンセルトークン</param>
    /// <param name="hitList">ヒット済みのコライダーリスト</param>
    private async UniTask DoActionAsync(MatchTargetAnimationData animationClip, CancellationToken token, List<Collider> hitList)
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + animationClip.AnimationClip.length;

        while (Time.timeSinceLevelLoad < endTime)
        {
            foreach (MatchTargetAnimationData.StartAnimationTimeList timeList in animationClip.AnimationTimeList)
            {
                float normalizedTime = (animationClip.AnimationClip.length - (endTime - Time.timeSinceLevelLoad)) / animationClip.AnimationClip.length;
                if (normalizedTime >= timeList.StartNormalizedTime && normalizedTime <= timeList.EndNormalizedTime)
                {
                    DoAttack(hitList);
                }
            }

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    /// <summary>
    /// 攻撃判定を行う
    /// スフィアキャストで前方の敵を検出してダメージを与える
    /// </summary>
    /// <param name="hitList">ヒット済みのコライダーリスト</param>
    private void DoAttack(List<Collider> hitList)
    {
        int hitCount = Physics.SphereCastNonAlloc(_characterTransform.position + (_characterTransform.forward * 0.1f), 0.5f, _characterTransform.forward, _raycastHits, 0.5f, _hitLayerMask);
        for (int i = 0; i < hitCount; i++)
        {
            bool isHit = false;

            foreach (Collider collider in hitList)
            {
                if (collider == _raycastHits[i].collider)
                {
                    isHit = true;
                    break; ;
                }
            }

            if (isHit)
            {
                continue;
            }

            if (_raycastHits[i].collider.TryGetComponent<CharacterStatus>(out CharacterStatus character))
            {
                character.DoDamage(1);
                hitList.Add(_raycastHits[i].collider);
            }
        }
    }

    /// <summary>
    /// ステート更新時の処理
    /// 攻撃ステート以外になった場合、攻撃をキャンセルする
    /// </summary>
    /// <param name="stateData">現在のステートデータ</param>
    private void UpDateState(StateJudgeBase stateData)
    {
        if (!(stateData is AttackStateJudge))
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}
