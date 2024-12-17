using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : MonoBehaviour, ISetAnimation
{
    private CharacterAnimation _characterAnimation;

    public void DoAction()
    {
        _characterAnimation.DoAttackAnimation();
    }

    public void SetAnimationComponent(CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;
    }
}
