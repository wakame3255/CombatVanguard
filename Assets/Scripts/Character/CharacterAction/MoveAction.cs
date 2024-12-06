using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Transform _characterTransform;

    /// <summary>
    /// 動くメソッド
    /// </summary>
    /// <param name="moveDirection">移動する方向</param>
    public void DoMove(Vector3 moveDirection)
    {
        _characterTransform.position += moveDirection.normalized* _speed * Time.deltaTime; 
    }

    /// <summary>
    /// トランスフォームをもらうメソッド
    /// </summary>
    /// <param name="characterTransform">キャラクターのトランスフォーム</param>
    public void SetTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }
}
