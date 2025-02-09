using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : MonoBehaviour, ISetAnimation, ISetTransform
{
    private CharacterAnimation _characterAnimation;
    private Transform _characterTransform;

    [SerializeField]
    private LayerMask _hitLayerMask;

    private RaycastHit[] _raycastHits;

    private void Awake()
    {
        _raycastHits = new RaycastHit[1];
    }

    public void DoAction()
    {
        int hitCount = Physics.SphereCastNonAlloc(_characterTransform.position + (_characterTransform.forward * 1f), 0.5f, _characterTransform.forward, _raycastHits, 1f, _hitLayerMask);

        for (int i = 0; i < hitCount; i++)
        {
            if (_raycastHits[i].collider.TryGetComponent<CharacterStatus>(out CharacterStatus character))
            {
                character.DoDamage(1);
            }      
        }
    }

    public void SetAnimationComponent(CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
