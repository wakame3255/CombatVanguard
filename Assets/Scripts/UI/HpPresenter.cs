using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;


public class HpPresenter : MonoBehaviour
{
    [SerializeField]
    CharacterStatus _playerStatus;

    [SerializeField]
    CharacterStatus _enemyStatus;

    [SerializeField]
    CharacterHpView _playerHpView;

    [SerializeField]
    CharacterHpView _enemyHpView;

    private void Start()
    {
        _playerHpView.SetHpValue(_playerStatus.ReactivePropertyHp.Value);
        _enemyHpView.SetHpValue(_enemyStatus.ReactivePropertyHp.Value);

        _playerStatus.ReactivePropertyHp.Subscribe(hp => _playerHpView.ChangeHpValue(hp));
        _enemyStatus.ReactivePropertyHp.Subscribe(hp => _enemyHpView.ChangeHpValue(hp));
    }
}
