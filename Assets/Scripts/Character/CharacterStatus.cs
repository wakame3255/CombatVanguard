using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public class CharacterStatus : MonoBehaviour, ISetAnimation
{
    public int _hp = 5;
    public ReactiveProperty<int> ReactivePropertyHp { get; private set; } = new ReactiveProperty<int>();

    private CharacterAnimation _characterAnimation;

    private void Awake()
    {
        ReactivePropertyHp.Value = _hp;
    }

    public void DoDamage(int damage)
    {
        _hp -= damage;
        ReactivePropertyHp.Value -= damage;

        CheckDeath();

        _characterAnimation.DoHitAnimation();
    }

    public void SetAnimationComponent(CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;
    }

    private void CheckDeath()
    {
        if (_hp < 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
