
using UnityEngine;
using R3;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class AttackAction : MonoBehaviour, ISetTransform, ISetStateCont
{
    [SerializeField]
    private LayerMask _hitLayerMask;

    private Transform _characterTransform;

    private CompositeDisposable _disposables = new CompositeDisposable();

    private CancellationTokenSource _cancellationTokenSource;

    private RaycastHit[] _raycastHits;

    private void Awake()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _raycastHits = new RaycastHit[1];
    }


    /// <summary>
    /// 攻撃を行ってくれるクラス
    /// </summary>
    /// <param name="animationClip">アニメーションに準拠して攻撃判定を行う</param>
    public async UniTask DoAction(MatchTargetAnimationData animationClip)
    {
        List<Collider> hitList = new List<Collider>();
        await DoActionAsync(animationClip, _cancellationTokenSource.Token, hitList);
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    public void SetStateCont(IApplicationStateChange characterStateCont)
    {
       characterStateCont.CurrentStateDataReactiveProperty.Subscribe(stateData => UpDateState(stateData))
            .AddTo(_disposables);
    }

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

    private void UpDateState(StateJudgeBase stateData)
    {
        if (!(stateData is AttackStateJudge))
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}
