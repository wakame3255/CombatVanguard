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
    /// <param name="inputXY"></param>
   public void DoMove(Vector2 inputXY)
    {
        _characterTransform.position += new Vector3(inputXY.x, 0, inputXY.y).normalized* _speed * Time.deltaTime; 
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
