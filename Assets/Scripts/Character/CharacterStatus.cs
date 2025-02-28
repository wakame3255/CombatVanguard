using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class CharacterStatus : MonoBehaviour, ISetAnimation
{
    public int _hp = 5;
    public ReactiveProperty<int> ReactivePropertyHp { get; private set; } = new ReactiveProperty<int>();

    private CharacterAnimation _characterAnimation;

    private IApplicationStateChange _characterStateCont;

    public IApplicationStateChange CharacterStateData { get => _characterStateCont; }

    private void Awake()
    {
        ReactivePropertyHp.Value = _hp;
    }

    public void DoDamage(int damage)
    {
        if (CheckDoAnimation())
        {
            _hp -= damage;
            ReactivePropertyHp.Value -= damage;

            CheckDeath();
        }     
    }

    public bool HitParry()
    {
        if (_characterStateCont.CurrentStateData is AttackStateData)
        {
           _characterStateCont.ApplicationStateChange(_characterStateCont.StateDataInformation.HitParryStateData);
            return true;
        }
        return false;
    }

    public void SetAnimationComponent(CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;
    }

    public void SetAnimationCont(IApplicationStateChange characterStateCont)
    {
        _characterStateCont = characterStateCont;
    }

    private void CheckDeath()
    {
        if (_hp < 1f)
        {
            gameObject.SetActive(false);
        }
    }

    private bool CheckDoAnimation()
    {
        switch (_characterStateCont.CurrentStateData)
        {
            case AvoidanceStateData:
                return false;

            case ParryStateData:
                return false;

            case GuardStateData:
                 _characterStateCont.ApplicationStateChange(_characterStateCont.StateDataInformation.GuardHitStateData);
                return false;
        }
        _characterStateCont.ApplicationStateChange(_characterStateCont.StateDataInformation.DownStateData);
        return true;
    }
}
