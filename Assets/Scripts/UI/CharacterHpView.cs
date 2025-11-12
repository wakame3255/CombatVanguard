using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクターのHP表示を管理するViewクラス
/// UIのSliderを使用してHPを視覚的に表示する
/// </summary>
public class CharacterHpView : MonoBehaviour
{
    /// <summary>
    /// HPバーのSlider
    /// </summary>
    [SerializeField]
    private Slider _hpBar;

    /// <summary>
    /// HP値を初期設定する
    /// 最大値と現在値を同時に設定する
    /// </summary>
    /// <param name="hp">設定するHP値</param>
    public void SetHpValue(int hp)
    {
        _hpBar.maxValue = hp;
        _hpBar.value = hp;
    }

    /// <summary>
    /// 現在のHP値を変更する
    /// </summary>
    /// <param name="nowHp">現在のHP値</param>
    public void ChangeHpValue(int nowHp)
    {
        _hpBar.value = nowHp;
    }
}
