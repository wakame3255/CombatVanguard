using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R3;

/// <summary>
/// キャラクターのステータスを管理するクラス
/// HP、ダメージ処理、パリィ判定などを行う
/// </summary>
public class CharacterStatus : MonoBehaviour, ISetAnimation
{
    /// <summary>
    /// 初期HP値
    /// </summary>
    public int _hp = 5;

    /// <summary>
    /// 現在のHPを監視可能なReactiveProperty
    /// </summary>
    public ReactiveProperty<int> ReactivePropertyHp { get; private set; } = new ReactiveProperty<int>();

    /// <summary>
    /// キャラクターアニメーションコンポーネント
    /// </summary>
    private CharacterAnimation _characterAnimation;

    /// <summary>
    /// キャラクターステート制御インターフェース
    /// </summary>
    private IApplicationStateChange _characterStateCont;

    /// <summary>
    /// キャラクターステートデータのプロパティ
    /// </summary>
    public IApplicationStateChange CharacterStateData { get => _characterStateCont; }

    /// <summary>
    /// 初期化処理
    /// HPの初期値を設定する
    /// </summary>
    private void Awake()
    {
        ReactivePropertyHp.Value = _hp;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// アニメーション実行可能な状態の場合のみダメージを適用する
    /// </summary>
    /// <param name="damage">受けるダメージ量</param>
    public void DoDamage(int damage)
    {
        if (CheckDoAnimation())
        {
            _hp -= damage;
            ReactivePropertyHp.Value -= damage;

            CheckDeath();
        }     
    }

    /// <summary>
    /// パリィが成功したかチェックする
    /// 攻撃状態の時にパリィされた場合、パリィ被弾状態に遷移する
    /// </summary>
    /// <returns>パリィが成功した場合true</returns>
    public bool HitParry()
    {
        if (_characterStateCont.CurrentStateData is AttackStateJudge)
        {
           _characterStateCont.ApplicationStateChange(_characterStateCont.StateDataInformation.HitParryStateData);
            return true;
        }
        return false;
    }

    /// <summary>
    /// アニメーションコンポーネントを設定する
    /// </summary>
    /// <param name="characterAnimation">キャラクターアニメーションコンポーネント</param>
    public void SetAnimationComponent(CharacterAnimation characterAnimation)
    {
        _characterAnimation = characterAnimation;
    }

    /// <summary>
    /// アニメーション制御インターフェースを設定する
    /// </summary>
    /// <param name="characterStateCont">キャラクターステート制御インターフェース</param>
    public void SetAnimationCont(IApplicationStateChange characterStateCont)
    {
        _characterStateCont = characterStateCont;
    }

    /// <summary>
    /// 死亡判定
    /// HPが0以下になった場合、ゲームオブジェクトを非アクティブにする
    /// </summary>
    private void CheckDeath()
    {
        if (_hp < 1f)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// アニメーション実行可能かチェックする
    /// 現在の状態に応じて適切なステート遷移を行う
    /// </summary>
    /// <returns>アニメーション実行可能な場合true</returns>
    private bool CheckDoAnimation()
    {
        switch (_characterStateCont.CurrentStateData)
        {
            case AvoidanceStateJudge:
                return false;

            case ParryStateJudge:
                return false;

            case DownStateJudge:
                return false;

            case GuardStateJudge:
                 _characterStateCont.ApplicationStateChange(_characterStateCont.StateDataInformation.GuardHitStateData);
                return false;
        }
        _characterStateCont.ApplicationStateChange(_characterStateCont.StateDataInformation.DownStateData);
        return true;
    }
}
