using UnityEngine;
using UnityEngine.UI;

public class UserTrajectHandler : MonoBehaviour
{
    [SerializeField] Toggle userTrajectoryToggle;
    [SerializeField] UserManager userManager; // プレハブをインスペクタから設定
    
    void Start()
    {
        if (userTrajectoryToggle == null)
        {
            Debug.LogError("UserTrajectoryToggle is not assigned in the inspector.");
            return;
        }else{
            userTrajectoryToggle.onValueChanged.AddListener(OnToggleChanged);
        }

    }

    
    public void OnToggleChanged(bool isTrajectOn)
    {
        if (isTrajectOn)
        {
            userManager.gameObject.AddComponent<LineRenderer>(); // ユーザーマネージャーにLineRendererを追加
        }
        else
        {
            Destroy(userManager.GetComponent<LineRenderer>()); // ユーザーマネージャーからLineRendererを削除
        }
    }

}
