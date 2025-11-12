using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

/// <summary>
/// HP表示のPresenterクラス
/// キャラクターのステータスとHP表示Viewを繋ぐ役割を担う
/// </summary>
public class HpPresenter : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのキャラクターステータス
    /// </summary>
    [SerializeField]
    CharacterStatus _playerStatus;

    /// <summary>
    /// 敵のキャラクターステータス
    /// </summary>
    [SerializeField]
    CharacterStatus _enemyStatus;

    /// <summary>
    /// プレイヤーのHP表示View
    /// </summary>
    [SerializeField]
    CharacterHpView _playerHpView;

    /// <summary>
    /// 敵のHP表示View
    /// </summary>
    [SerializeField]
    CharacterHpView _enemyHpView;

    /// <summary>
    /// 開始時の処理
    /// 初期HP値を設定し、HP変更を監視する
    /// </summary>
    private void Start()
    {
        // 初期HP値を設定
        _playerHpView.SetHpValue(_playerStatus.ReactivePropertyHp.Value);
        _enemyHpView.SetHpValue(_enemyStatus.ReactivePropertyHp.Value);

        // HP変更を監視してViewを更新
        _playerStatus.ReactivePropertyHp.Subscribe(hp => _playerHpView.ChangeHpValue(hp));
        _enemyStatus.ReactivePropertyHp.Subscribe(hp => _enemyHpView.ChangeHpValue(hp));
    }
}
