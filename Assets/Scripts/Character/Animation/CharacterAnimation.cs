using UnityEngine;
using R3;
using Cysharp.Threading.Tasks;

/// <summary>
/// キャラクターのアニメーションを管理するクラス
/// 移動アニメーション、挿入型アニメーション、Animatorパラメータの制御を行う
/// </summary>
[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour, ISetTransform
{
    /// <summary>
    /// 歩行アニメーションの補間速度
    /// </summary>
    [SerializeField]
    private float _walkDamp;

    /// <summary>
    /// アニメーションデータ（ScriptableObject）
    /// </summary>
    [SerializeField]
    private AnimationData _animationData;
 
    /// <summary>
    /// 挿入型アニメーションシステム
    /// </summary>
    [SerializeField]
    private InsertAnimationSystem _insertAnimationSystem;

    /// <summary>
    /// キャラクターのTransform
    /// </summary>
    private Transform _characterTransform;

    /// <summary>
    /// Animatorコンポーネント
    /// </summary>
    private Animator _animator;

    /// <summary>アニメーションデータのプロパティ</summary>
   public AnimationData AnimationData { get => _animationData; }

    /// <summary>アニメーション実行中かどうかのReactiveProperty</summary>
    public ReactiveProperty<bool> ReactivePropertyIsAnimation { get => _insertAnimationSystem.ReactivePropertyIsAnimation; }

    /// <summary>アニメーション実行中かどうか</summary>
    public bool IsAnimation { get; private set; }

    /// <summary>
    /// 初期化処理
    /// Animatorコンポーネントを取得する
    /// </summary>
    private void Awake()
    {
        _animator = this.CheckComponentMissing<Animator>();
    }
   
    /// <summary>
    /// 歩行アニメーションを実行する
    /// 移動方向に応じてAnimatorのパラメータを更新する
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
   public void DoWalkAnimation(Vector3 moveDirection)
    {
        Vector2 changeInput = GetDirectionToAnimationValue(moveDirection);
        _animator.SetFloat(AnimationStringUtility.MoveInputXName, changeInput.x, _walkDamp, Time.deltaTime);
        _animator.SetFloat(AnimationStringUtility.MoveInputYName, changeInput.y, _walkDamp, Time.deltaTime);
    }

    /// <summary>
    /// 挿入型アニメーションを再生する
    /// 一度だけ再生されるアニメーション（攻撃、回避など）を実行する
    /// </summary>
    /// <param name="animationData">アニメーションデータ</param>
    public void DoAnimation(MatchTargetAnimationData animationData)
    {
        _insertAnimationSystem.AnimationPlay(animationData).Forget();
    }

    /// <summary>
    /// AnimatorのBoolパラメータを設定する
    /// 指定されたパラメータのみをtrueにし、他をfalseにする
    /// </summary>
    /// <param name="AnimName">パラメータ名</param>
    public void SetAnimationBool(string AnimName)
    {
        AnimationReset(AnimName);
    }

    /// <summary>
    /// キャラクターのTransformを設定する
    /// </summary>
    /// <param name="characterTransform">キャラクターのTransform</param>
    public void SetCharacterTransform(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    /// <summary>
    /// ワールド空間の移動方向をキャラクターローカル空間のアニメーション値に変換する
    /// </summary>
    /// <param name="moveDirection">ワールド空間の移動方向</param>
    /// <returns>アニメーション用の2D入力値</returns>
    private Vector2 GetDirectionToAnimationValue(Vector3 moveDirection)
    {
        MyExtensionClass.CheckArgumentNull(moveDirection, nameof(moveDirection));

        // キャラクターの右方向と前方向への投影
        float inputX = Vector3.Dot(moveDirection, _characterTransform.right);
        float inputY = Vector3.Dot(moveDirection, _characterTransform.forward);

        return new Vector2(inputX, inputY);
    }

    /// <summary>
    /// AnimatorのBoolパラメータをリセットする
    /// 指定されたパラメータのみをtrueにし、他の全てのBoolパラメータをfalseにする
    /// </summary>
    /// <param name="DoAnim">trueにするパラメータ名</param>
    private void AnimationReset(string DoAnim)
    {
        foreach (AnimatorControllerParameter anim in _animator.parameters)
        {
            if (anim.type  != AnimatorControllerParameterType.Bool)
            {
                continue;
            }
            if (anim.name == DoAnim)
            {              
                _animator.SetBool(anim.name, true);
            }
            else
            {
                _animator.SetBool(anim.name, false);
            }
        }
    }
}
