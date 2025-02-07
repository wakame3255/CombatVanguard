using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public interface IInputInformation
{
    public ReactiveProperty<bool> ReactivePropertyAttack { get; }
    public ReactiveProperty<bool> ReactivePropertyAvoidance { get; }
    public ReactiveProperty<bool> ReactivePropertyDash { get; }
    public ReactiveProperty<bool> ReactivePropertyGuard { get; }
    public ReactiveProperty<Vector2> ReactivePropertyMove { get; }
}
