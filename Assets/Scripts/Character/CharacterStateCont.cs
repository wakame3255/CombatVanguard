using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateCont
{
   private CharacterStateType _currentStateType;

    public CharacterStateCont()
    {

    }

    public void ChangeState(CharacterStateType stateType)
    {
        _currentStateType = stateType;
    }
}

public enum CharacterStateType
{
    Normal,
    Guard,
    Attack,
    Down,
    Dead,
}
