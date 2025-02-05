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
       
    }

    public void CheckChangeState(CharacterStateType stateType)
    {
        if (_currentStateType == stateType)
        {
            return;
        }
        ChangeState(stateType);
    }
}


public enum CharacterStateType
{
    Idle,
    Guard,
    GurdHit,
    Attack,
    Avoidance,
    Down,
    Stan,
    Dead,
}
