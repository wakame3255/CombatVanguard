using R3;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの入力を管理するクラス
/// キーボードとマウスからの入力を検知し、ReactivePropertyで状態を管理する
/// </summary>
public class PlayerInput : MonoBehaviour, IInputInformation
{
    /// <summary>
    /// 移動入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<Vector2> _reactivePropertyMove = new ReactiveProperty<Vector2>();

    /// <summary>
    /// 攻撃入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyAttack = new ReactiveProperty<bool>();

    /// <summary>
    /// 回避入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyAvoidance = new ReactiveProperty<bool>();

    /// <summary>
    /// ダッシュ入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyDash = new ReactiveProperty<bool>();

    /// <summary>
    /// ガード入力のReactiveProperty
    /// </summary>
    private ReactiveProperty<bool> _reactivePropertyGuard = new ReactiveProperty<bool>();

    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    public static PlayerInput Instance { get; private set; }

    /// <summary>
    /// 攻撃入力プロパティ
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyAttack { get => _reactivePropertyAttack; }

    /// <summary>
    /// 回避入力プロパティ
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyAvoidance { get => _reactivePropertyAvoidance; }

    /// <summary>
    /// ダッシュ入力プロパティ
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyDash { get => _reactivePropertyDash; }

    /// <summary>
    /// ガード入力プロパティ
    /// </summary>
    public ReactiveProperty<bool> ReactivePropertyGuard { get => _reactivePropertyGuard; }

    /// <summary>
    /// 移動入力プロパティ
    /// </summary>
    public ReactiveProperty<Vector2> ReactivePropertyMove { get => _reactivePropertyMove; }

    /// <summary>
    /// 初期化処理
    /// シングルトンパターンでインスタンスを管理する
    /// </summary>
    private void Awake()
    {
        // インスタンスが存在しない場合、自身をインスタンスにする
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError(transform.root.name + "他のインスタンスが存在します");
        }
    }

    /// <summary>
    /// 毎フレーム呼ばれる更新処理
    /// 入力デバイスをチェックしてボタン状態を更新する
    /// </summary>
    private void Update()
    {
        CheckKeyBoardDevice();
        SetButton();
    }

    /// <summary>
    /// キーボードデバイスの存在を確認する
    /// </summary>
    private void CheckKeyBoardDevice()
    {
        if (Keyboard.current != null)
        {
            SetMoveInfomation();
        }
    }

    /// <summary>
    /// キーボード入力から移動情報を設定する
    /// WASD キーで移動方向を決定する
    /// </summary>
    private void SetMoveInfomation()
    {
        float inputX = Keyboard.current.aKey.isPressed ? -1 : Keyboard.current.dKey.isPressed ? 1 : 0;
        float inputY = Keyboard.current.sKey.isPressed ? -1 : Keyboard.current.wKey.isPressed ? 1 : 0;

        _reactivePropertyMove.Value = new Vector2(inputX, inputY);
    }

    /// <summary>
    /// ボタン入力を設定する
    /// マウスとキーボードから各アクションのボタン状態を取得する
    /// </summary>
    private void SetButton()
    {
        // 左クリックで攻撃
        _reactivePropertyAttack.Value = Mouse.current.leftButton.wasPressedThisFrame;

        // 右クリックでガード
        _reactivePropertyGuard.Value = Mouse.current.rightButton.isPressed;

        // スペースキーで回避
        _reactivePropertyAvoidance.Value = Keyboard.current.spaceKey.wasPressedThisFrame;

        // 左シフトキーでダッシュ
        _reactivePropertyDash.Value = Keyboard.current.leftShiftKey.isPressed;

        
    }

}
