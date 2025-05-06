using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserTrajectHandler : MonoBehaviour
{
    [SerializeField] Toggle userTrajectoryToggle;
    [SerializeField] UserManager userManager; 
    [SerializeField] Material lineRendererMaterial; 

    void Start()
    {
        if (userTrajectoryToggle == null)
        {
            Debug.LogError("UserTrajectoryToggle is not assigned in the inspector.");
            return;
        }
        
        userTrajectoryToggle.onValueChanged.AddListener(OnToggleChanged);
        

    }

    
    public void OnToggleChanged(bool isTrajectOn)
    {
        if (isTrajectOn)
        {
            InitializeLineRenderer();
            return;
        }

        Destroy(userManager.GetComponent<LineRenderer>()); // ユーザーマネージャーからLineRendererを削除
        
    }

    void InitializeLineRenderer()
    {
        LineRenderer lineRenderer = userManager.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f; // 線の太さを設定
        lineRenderer.endWidth = 0.1f; // 線の太さを設定
        lineRenderer.startColor = Color.red; // 線の色を設定
        lineRenderer.endColor = Color.red; // 線の色を設定
        lineRenderer.positionCount = 0; // 頂点数を初期化
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // マテリアルを設定
    }

}
