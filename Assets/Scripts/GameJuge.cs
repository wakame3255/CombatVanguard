using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの勝敗判定を行うクラス
/// プレイヤーと敵のHPを監視し、ゲームオーバーまたはクリアのシーン遷移を制御する
/// </summary>
public class GameJuge : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのキャラクターステータス
    /// </summary>
    [SerializeField]
    private CharacterStatus _playerStatus;

    /// <summary>
    /// 敵のキャラクターステータス
    /// </summary>
    [SerializeField]
    private CharacterStatus _enemyStatus;

    /// <summary>
    /// シーン遷移を管理するクラス
    /// </summary>
    [SerializeField]
    private SceneChenger _sceneChenger;

    /// <summary>
    /// クリア時に遷移するシーン名
    /// </summary>
    [SerializeField]
    private string ClearScene;

    /// <summary>
    /// ゲームオーバー時に遷移するシーン名
    /// </summary>
    [SerializeField]
    private string GameOverScene;

    /// <summary>
    /// 毎フレーム呼ばれる更新処理
    /// プレイヤーと敵のHPをチェックし、ゲーム終了判定を行う
    /// </summary>
    private void Update()
    {
        // 敵のHPが0以下になった場合、クリアシーンへ遷移
        if (_enemyStatus.ReactivePropertyHp.Value <= 0)
        {
            _sceneChenger.SceneChenge(ClearScene);
        }
        
        // プレイヤーのHPが0以下になった場合、ゲームオーバーシーンへ遷移
        if (_playerStatus.ReactivePropertyHp.Value <= 0)
        {
            _sceneChenger.SceneChenge(GameOverScene);
        }
    }
}
