#nullable enable
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

/// <summary>
/// ユーザーの移動軌跡を扱うクラス
/// </summary>
public class UserTrajectHandler
{
    private UserMono _userMono;

    [Inject]
    public UserTrajectHandler(
        UserMono userMono)
    {
        _userMono = userMono;
    }

    // 軌跡トグルの変化に応じた処理を行うメソッド
    public void OnToggleChanged(bool isTrajectOn)
    {
        if (isTrajectOn)
        {
            InitializeLineRenderer();
            return;
        }

        // 軌跡の無効化(＝LineRendererの削除)
        GameObject.Destroy(_userMono.GetComponent<LineRenderer>());
    }

    // LineRendererの初期化を行うメソッド
    void InitializeLineRenderer()
    {
        LineRenderer lineRenderer = _userMono.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f; // 線の太さを設定
        lineRenderer.endWidth = 0.1f; // 線の太さを設定
        lineRenderer.startColor = Color.red; // 線の色を設定
        lineRenderer.endColor = Color.red; // 線の色を設定
        lineRenderer.positionCount = 0; // 頂点数を初期化
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // マテリアルを設定
    }
}
