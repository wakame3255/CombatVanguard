using System;
using UnityEngine;
using R3;
public class ParryAction : MonoBehaviour, ISetTransform, ISetStateCont
{
    [SerializeField]
    private LayerMask _hitLayeMask;

    [SerializeField]
    private int _hitCount;

    private IApplicationStateChange _stateCont;
    private Transform _characterTransform;

    private Collider[] _hitColliders;

    private void Awake()
    {
        _hitColliders = new Collider[_hitCount];
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    public void SetStateCont(IApplicationStateChange stateCont)
    {
        _stateCont = stateCont;
        stateCont.CurrentStateDataReactiveProperty.Subscribe(stateData => CheckParry(stateData));
    }

    private void CheckParry(StateJudgeBase stateData)
    {
        if (!(stateData is GuardStateJudge))
        {
            return;
        }

        int hitCount = Physics.OverlapSphereNonAlloc(_characterTransform.position, 2f, _hitColliders, _hitLayeMask);

        for (int i = 0; i < hitCount; i++)
        {
            if(!_hitColliders[i].TryGetComponent<CharacterStatus>(out CharacterStatus status))
            {
                return;
            }

            if(status.HitParry())
            {
                _stateCont.ApplicationStateChange(_stateCont.StateDataInformation.ParryStateData);
            }
        }
    }
}