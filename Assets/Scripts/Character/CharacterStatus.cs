using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public int Hp { get; private set; } = 5;

    public void DoDamage(int damage)
    {
        Hp -= damage;
    }
}
