
using UnityEngine;
using R3;

public class AttackAction : MonoBehaviour, ISetTransform, ISetStateCont
{
    private Transform _characterTransform;

    private CompositeDisposable _disposables = new CompositeDisposable();

    [SerializeField]
    private LayerMask _hitLayerMask;

    private RaycastHit[] _raycastHits;

    private void Awake()
    {
        _raycastHits = new RaycastHit[1];
    }


    /// <summary>
    /// 攻撃を行ってくれるクラス
    /// </summary>
    /// <param name="animationClip">アニメーションに準拠して攻撃判定を行う</param>
    public void DoAction(AnimationClip animationClip)
    {
        int hitCount = Physics.SphereCastNonAlloc(_characterTransform.position + (_characterTransform.forward * 0.1f), 0.5f, _characterTransform.forward, _raycastHits, 0.5f, _hitLayerMask);

        for (int i = 0; i < hitCount; i++)
        {
            if (_raycastHits[i].collider.TryGetComponent<CharacterStatus>(out CharacterStatus character))
            {
                character.DoDamage(1);
            }      
        }
    }

　//private UniTask DoActionAsync(AnimationClip animationClip, )
 //   {
 //       DoAction(animationClip);
 //       return UniTask.CompletedTask;
 //   }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    public void SetStateCont(IApplicationStateChange characterStateCont)
    {
       characterStateCont.CurrentStateDataReactiveProperty.Subscribe(stateData => UpDateState(stateData))
            .AddTo(_disposables);
    }

    private void UpDateState(StateDataBase stateData)
    {

    }
}
