using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : MonoBehaviour, ISetAnimation
{
    private CharacterAnimation _characterAnimation;

    [SerializeField]
    private LayerMask _hitLayerMask;

    private RaycastHit[] _raycastHits;

    private void Awake()
    {
        _raycastHits = new RaycastHit[5];
    }

    public void DoAction()
    {
        _characterAnimation.DoAttackAnimation();

        int hitCount = Physics.SphereCastNonAlloc(transform.position, 0.5f, transform.forward, _raycastHits, 1f, _hitLayerMask);

        for (int i = 0; i < hitCount; i++)
        {
            if (_raycastHits[i].collider.TryGetComponent<CharacterStatus>(out CharacterStatus character))
            {
                character.DoDamage(1);
                print("hit");
            }
        }
    }

    public void SetAnimationComponent(CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;
    }
}
