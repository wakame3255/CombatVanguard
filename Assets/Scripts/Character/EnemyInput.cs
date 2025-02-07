using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInput : MonoBehaviour, IInputInformation
{
    private ReactiveProperty<Vector2> _reactivePropertyMove = new ReactiveProperty<Vector2>();
    private ReactiveProperty<bool> _reactivePropertyAttack = new ReactiveProperty<bool>();
    private ReactiveProperty<bool> _reactivePropertyAvoidance = new ReactiveProperty<bool>();
    private ReactiveProperty<bool> _reactivePropertyDash = new ReactiveProperty<bool>();
    private ReactiveProperty<bool> _reactivePropertyGuard = new ReactiveProperty<bool>();


    public static PlayerInput Instance { get; private set; }

    public ReactiveProperty<bool> ReactivePropertyAttack { get => _reactivePropertyAttack; }
    public ReactiveProperty<bool> ReactivePropertyAvoidance { get => _reactivePropertyAvoidance; }
    public ReactiveProperty<bool> ReactivePropertyDash { get => _reactivePropertyDash; }
    public ReactiveProperty<bool> ReactivePropertyGuard { get => _reactivePropertyGuard; }
    public ReactiveProperty<Vector2> ReactivePropertyMove { get => _reactivePropertyMove; }

    public void SetMoveInfomation(Vector2 moveDirection)
    {
        _reactivePropertyMove.Value = moveDirection;
    }

    public void DoAttack(bool isAttack)
    {
        _reactivePropertyAttack.Value = isAttack;    
    }

    public void DoDash(bool isDash)
    {
        _reactivePropertyDash.Value = isDash;
    }

    public void DoAvoidance(bool isAvoidance)
    {
        _reactivePropertyAvoidance.Value = isAvoidance;
    }

    public void DoGuard(bool isGuard)
    {
        _reactivePropertyGuard.Value = isGuard;
    }
}
