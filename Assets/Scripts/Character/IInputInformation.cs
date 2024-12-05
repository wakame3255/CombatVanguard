using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

public interface IInputInformation
{
    public ReactiveProperty<bool> ReactivePropertyAttack { get; }
    public ReactiveProperty<bool> ReactivePropertyJump { get; }
    public ReactiveProperty<Vector2> ReactivePropertyMove { get; }
}
