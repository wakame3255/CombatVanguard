using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMove : MonoBehaviour, ISetTransform
{
    private Transform _characterTransform;

   public void DoRotation(Vector3 moveDirection)
    {
        _characterTransform.rotation = Quaternion.RotateTowards(_characterTransform.rotation, Quaternion.Euler(moveDirection), 50);
    }

    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
