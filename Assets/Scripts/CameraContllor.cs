using Cinemachine;
using UnityEngine;
using R3;
using Unity.VisualScripting;

/// <summary>
/// カメラの制御を行うクラス
/// </summary>
public class CameraContllor : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _mainCamera;

    [SerializeField]
    private CinemachineVirtualCamera _lockOnCamera;

    [SerializeField]
    private RectTransform _lockOn;

    [SerializeField]
    private Transform _PlayerPos;

    [SerializeField]
    private LayerMask _hitLayerMask;

    [SerializeField]
    private float _lockOnRange = 10f;

    [SerializeField]
    private float _searchSize = 5f;

    private Transform _cameraTransform;

    public Transform CameraTransform => _cameraTransform;

    private Camera _camera;

    private ReactiveProperty<Transform> _currentTarget = new ReactiveProperty<Transform>();
    public ReadOnlyReactiveProperty<Transform> RPCurrentTarget => _currentTarget;


    private ReactiveProperty<bool> _isLockOn = new ReactiveProperty<bool>();
    public ReadOnlyReactiveProperty<bool> RPIsLockOn => _isLockOn;

    private void Awake()
    {
        // 初期設定
        _mainCamera.Priority = 10;
        _lockOnCamera.Priority = 1;

        _camera = Camera.main;
        _cameraTransform = _camera.transform;

        // ロックオン状態の監視
        _isLockOn.Subscribe(isLockOn =>
        {
            UpdateCameraPriority(isLockOn);
        });

        // ターゲットの監視
        _currentTarget.Subscribe(target =>
        {
            if (target != null)
            {
                UpdateLockOnCamera(target);
            }
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            SearchAndLockOnTarget();
        }

        if (_isLockOn.Value)
        {
            // ターゲットが範囲外に出た場合はロックオンを解除
            if (_currentTarget.Value != null)
            {
                float distance = Vector3.Distance(_PlayerPos.position, _currentTarget.Value.position);
                if (distance > _lockOnRange)
                {
                    SetLockOn(false);
                }

                _lockOn.position = _camera.WorldToScreenPoint(_currentTarget.Value.position); // ターゲットの位置をロックオンカメラに設定
                _lockOn.position += Vector3.up * 100; // 上にオフセット
            }
        }
    }

    /// <summary>
    /// 最も近い敵を探してロックオン
    /// </summary>
    public void SearchAndLockOnTarget()
    {
        if (_isLockOn.Value)
        {
            SetLockOn(false);
            return;
        }

        // 周囲の敵を検索
        RaycastHit[] hitColliders = Physics.BoxCastAll(_PlayerPos.position + (_cameraTransform.forward * _searchSize), Vector3.one * _searchSize, _cameraTransform.forward, Quaternion.identity, _lockOnRange, _hitLayerMask);
        
        Transform nearestTarget = null;

        foreach (RaycastHit hitCollider in hitColliders)
        {
            DebugUtility.Log("hiy");

            if (hitCollider.collider.TryGetComponent<CharacterStatus>(out CharacterStatus character))
            {
                nearestTarget = hitCollider.transform;
            }
        }

        if (nearestTarget != null)
        {
            _currentTarget.Value = nearestTarget;
            SetLockOn(true);
        }
    }

    /// <summary>
    /// ロックオン状態を設定
    /// </summary>
    private void SetLockOn(bool isLockOn)
    {
        _isLockOn.Value = isLockOn;
        if (!isLockOn)
        {
            _currentTarget.Value = null;
        }
    }

    /// <summary>
    /// カメラの優先度を更新
    /// </summary>
    private void UpdateCameraPriority(bool isLockOn)
    {
        _mainCamera.Priority = isLockOn ? 1 : 10;
        _lockOnCamera.Priority = isLockOn ? 10 : 1;

        _lockOn.gameObject.SetActive(isLockOn); // ロックオンUIの表示/非表示
    }

    /// <summary>
    /// ロックオンカメラの設定を更新
    /// </summary>
    private void UpdateLockOnCamera(Transform target)
    {
        var composer = _lockOnCamera.GetCinemachineComponent<CinemachineComposer>();
        if (composer != null)
        {
            // ターゲットの位置に追従
            _lockOnCamera.LookAt = target;
            _lockOnCamera.Follow = _PlayerPos;

            // カメラの設定を調整
            composer.m_TrackedObjectOffset = new Vector3(0, 1.5f, 0); // ターゲットのオフセット
        }
    }

    /// <summary>
    /// 現在のロックオンターゲットを取得
    /// </summary>
    public Transform GetCurrentTarget()
    {
        return _currentTarget.Value;
    }

    /// <summary>
    /// ロックオン中かどうかを取得
    /// </summary>
    public bool IsLockOn()
    {
        return _isLockOn.Value;
    }
} 