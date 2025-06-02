#nullable enable
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using VContainer;

/// <summary>
/// UIの管理をするクラス
/// </summary>
public class AppViewMono : MonoBehaviour
{
    [Header("フロアレベル")]
    [SerializeField] private TMP_Dropdown _floorLevelDropdown; // フロアレベル選択用のドロップダウン
    [SerializeField] private TMP_Text _floorLevelText; // フロアレベルを表示するテキスト

    [Header("ユーザーの位置")]
    [SerializeField] private Button _userPositionConfirmButton; // ユーザー位置確認ボタン
    [SerializeField] private TMP_Text _userPositionText; // ユーザーの位置を表示するテキスト

    [Header("ユーザーの方向")]
    [SerializeField] private Button _userDirectionConfirmButton; // 初期向き設定ボタン
    [SerializeField] private TMP_Text _userRotationText; // ユーザーの向きを表示するテキスト

    [Header("移動軌跡")]
    [SerializeField] private Toggle _userTrajectoryToggle; // ユーザーの軌跡を表示するトグル

    [Header("リロード")]
    [SerializeField] private Button _reloadButton; // リロード

    [Header("マップマッチング")]
    [SerializeField] private Toggle _mapMatchingToggle; // マップマッチングのonoffを切り替えるトグル

    public event Action<int> OnFloorSelected = null!;
    public event Action OnUserPositionConfirmButtonClicked = null!;
    public event Action OnUserDirectionConfirmButtonClicked = null!;
    public event Action<bool> OnUserTrajectoryToggleChanged = null!;
    public event Action<bool> OnMapMatchingToggleChanged = null!;
    public event Action OnReloadButtonClicked = null!;

    [Inject]
    public void Initialize()
    {
        _userPositionConfirmButton.interactable = false;
        _userDirectionConfirmButton.interactable = false;

        _floorLevelDropdown.onValueChanged
            .AddListener(selectedIndex => OnFloorSelected?.Invoke(selectedIndex));

        _userPositionConfirmButton.onClick
            .AddListener(() => OnUserPositionConfirmButtonClicked?.Invoke());

        _userDirectionConfirmButton.onClick
            .AddListener(() => OnUserDirectionConfirmButtonClicked?.Invoke());

        _userTrajectoryToggle.onValueChanged
            .AddListener(isOn => OnUserTrajectoryToggleChanged?.Invoke(isOn));

        _reloadButton.onClick
            .AddListener(() => OnReloadButtonClicked?.Invoke());

        _mapMatchingToggle.onValueChanged
            .AddListener(isOn => OnMapMatchingToggleChanged?.Invoke(isOn));
    }

    // ユーザー位置設定への移行用のメソッド
    public void SwitchToPositionSetup()
    {
        _userPositionConfirmButton.interactable = true;
    }

    // ユーザー向き設定への移行用のメソッド
    public void SwitchToDirectionSetup()
    {
        _userPositionConfirmButton.interactable = false;
        _userDirectionConfirmButton.interactable = true;
    }

    // 初期設定用のUIを非activeにするメソッド
    public void DeactivateInitialSetupUI()
    {
        _floorLevelDropdown.gameObject.SetActive(false);
        _userPositionConfirmButton.gameObject.SetActive(false);
        _userDirectionConfirmButton.gameObject.SetActive(false);
    }

    // ユーザーのいるフロアをテキストに反映するメソッド
    public void SetFloorLevelText(int floorLevel)
    {
        _floorLevelText.text = $"Floor Level: {floorLevel}";
    }

    // ユーザーの位置をテキストに反映するメソッド
    public void SetUserPositionText(Vector3 userPosition)
    {
        _userPositionText.text = $"User Position: {userPosition.x:F2}, {userPosition.y:F2}";
    }

    // ユーザーの向きをテキストに反映するメソッド
    public void SetUserDirectionText(float cumulativeYaw)
    {
        _userRotationText.text = $"Heading: {cumulativeYaw * Mathf.Rad2Deg:F2}°";
    }
}
