using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserTrajectHandler
{
    private GameObject _userObject;

    public UserTrajectHandler(Toggle userTrajectoryToggle, GameObject userObject)
    {
        if (userTrajectoryToggle == null)
        {
            Debug.LogError("UserTrajectoryToggle is not assigned in the inspector.");
            return;
        }

        userTrajectoryToggle.onValueChanged.AddListener(OnToggleChanged);
        _userObject = userObject;
    }

    
    public void OnToggleChanged(bool isTrajectOn)
    {
        if (isTrajectOn)
        {
            InitializeLineRenderer();
            return;
        }

        GameObject.Destroy(_userObject.GetComponent<LineRenderer>()); // ユーザーマネージャーからLineRendererを削除
        
    }

    void InitializeLineRenderer()
    {
        Debug.Log("LineRendererをアタッチ");
        LineRenderer lineRenderer = _userObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f; // 線の太さを設定
        lineRenderer.endWidth = 0.1f; // 線の太さを設定
        lineRenderer.startColor = Color.red; // 線の色を設定
        lineRenderer.endColor = Color.red; // 線の色を設定
        lineRenderer.positionCount = 0; // 頂点数を初期化
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // マテリアルを設定
    }

}
