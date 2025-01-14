using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class GameJuge : MonoBehaviour
{
    [SerializeField]
    private CharacterStatus _playerStatus;

    [SerializeField]
    private CharacterStatus _enemyStatus;

    [SerializeField]
    private SceneChenger _sceneChenger;

    [SerializeField]
    private string ClearScene;

    [SerializeField]
    private string GameOverScene;

    private void Update()
    {
        if (_playerStatus.ReactivePropertyHp.Value <= 0)
        {
            _sceneChenger.SceneChenge(ClearScene);
        }

        if (_enemyStatus.ReactivePropertyHp.Value <= 0)
        {
            _sceneChenger.SceneChenge(GameOverScene);
        }
    }
}
