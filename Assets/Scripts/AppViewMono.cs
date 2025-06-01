#nullable enable
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using VContainer;

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

    [Header("マップマッチング")]
    [SerializeField] private Toggle _mapMatchingToggle;

    public event Action<int> OnFloorSelected = null!;
    public event Action OnUserPositionConfirmButtonClicked = null!;
    public event Action OnUserDirectionConfirmButtonClicked = null!;
    public event Action<bool> OnUserTrajectoryToggleChanged = null!;
    public event Action<bool> OnMapMatchingToggleChanged = null!;

    [Inject]
    public void Initialize()
    {
        _floorLevelDropdown.onValueChanged
            .AddListener(selectedIndex => OnFloorSelected?.Invoke(selectedIndex));

        _userPositionConfirmButton.onClick
            .AddListener(() => OnUserPositionConfirmButtonClicked?.Invoke());

        _userDirectionConfirmButton.onClick
            .AddListener(() => OnUserDirectionConfirmButtonClicked?.Invoke());

        _userTrajectoryToggle.onValueChanged
            .AddListener(isOn => OnUserTrajectoryToggleChanged?.Invoke(isOn));

        _mapMatchingToggle.onValueChanged
            .AddListener(isOn => OnMapMatchingToggleChanged?.Invoke(isOn));
    }

    // ユーザーの位置をテキストに反映するメソッド
    void SetUserPositionText(Vector3 userPosition)
    {
        _userPositionText.text = $"User Position: {userPosition.x:F2}, {userPosition.y:F2}";
    }

    // ユーザーの向きをテキストに反映するメソッド
    void SetUserDirectionText(float cumulativeYaw)
    {
        _userRotationText.text = $"Heading: {cumulativeYaw * Mathf.Rad2Deg:F2}°";
    }

    // ユーザーのいるフロアをテキストに反映するメソッド
    public void SetFloorLevelText(int floorLevel)
    {
        _floorLevelText.text = $"Floor Level: {floorLevel}";
    }

}
