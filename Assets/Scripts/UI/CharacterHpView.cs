using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHpView : MonoBehaviour
{
    [SerializeField]
    private Slider _hpBar;

    public void SetHpValue(int hp)
    {
        _hpBar.maxValue = hp;
        _hpBar.value = hp;
    }

    public void ChangeHpValue(int nowHp)
    {
        _hpBar.value = nowHp;
    }
}
