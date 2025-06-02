#nullable enable
using UnityEngine.SceneManagement;

/// <summary>
/// シーンの読み込みを行うクラス
/// </summary>
public class SceneLoader
{
    // シーンをリロードするメソッド
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
