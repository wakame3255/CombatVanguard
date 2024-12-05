using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collision3D))]
public class PlayerCharacter : MonoBehaviour
{
    private Collision3D _collision3D;
    private PlayerAction _playerAction;

    private void Awake()
    {
        _collision3D = this.CheckComponentMissing<Collision3D>();
        _playerAction = this.CheckComponentMissing<PlayerAction>();
        _playerAction.SetInputEvent(PlayerInput.Instance);
    }

    private void FixedUpdate()
    {
        _collision3D.CheckCollision();
    }
}
